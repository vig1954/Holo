using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using OpenTK.Graphics.OpenGL;
using Processing.Computing;
using Processing.Utils;

namespace Processing
{
    public enum ImagePixelFormat
    {
        [SizeInBytes(sizeof(byte))] Byte = 32,
        [SizeInBytes(sizeof(float))] Float = 64
    }

    public enum ImageFormat
    {
        [ChannelsCount(1)] Greyscale = 1,
        [ChannelsCount(3)] RGB = 2,
        [ChannelsCount(4)] RGBA = 4,
        [ChannelsCount(2)] AmplitudePhase = 8,
        [ChannelsCount(2)] RealImaginative = 16
    }

    public class ImageUpdatedEventData
    {
        public bool ReloadControls { get; private set; }

        public ImageUpdatedEventData(bool reloadControls)
        {
            ReloadControls = reloadControls;
        }
    }

    public interface IImageHandler
    {
        bool Ready { get; }
        IDictionary<string, object> Tags { get; }
        event Action<ImageUpdatedEventData> ImageUpdated;
        int Width { get; }
        int Height { get; }
        int PixelSizeInBytes { get; }
        ImagePixelFormat PixelFormat { get; }
        ImageFormat Format { get; }
        int? OpenGlTextureId { get; }
        ComputeImage2D ComputeBuffer { get; }
        void UploadToComputingDevice(bool forceUpdate = false);
        void FreeComputingDevice();
        void Update();
    }

    public class ImageHandler : IImageHandler, IDisposable
    {
        protected Dictionary<string, object> _tags { get; set; } = new Dictionary<string, object>();

        protected byte[] Data;

        public bool Ready => true;

        public event Action<ImageUpdatedEventData> ImageUpdated;
        public IDictionary<string, object> Tags => _tags;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ImagePixelFormat PixelFormat { get; private set; }
        public ImageFormat Format { get; private set; }
        public int? OpenGlTextureId { get; private set; }
        public int PixelSizeInBytes => PixelFormat.GetAttribute<SizeInBytesAttribute>().Value * Format.GetAttribute<ChannelsCountAttribute>().Value;
        public ComputeImage2D ComputeBuffer { get; private set; }

        protected ImageHandler()
        {
        }

        public void UploadToComputingDevice(bool forceUpdate = false)
        {
            if (OpenGlTextureId.HasValue && !forceUpdate)
                return;
            
            var textureId = OpenGlTextureId ?? GL.GenTexture();
            OpenGlErrorThrower.ThrowIfAny();

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            OpenGlErrorThrower.ThrowIfAny();

            PixelInternalFormat pixelInternalFormat;
            OpenTK.Graphics.OpenGL.PixelFormat pixelFormat;
            PixelType pixelType;

            // TODO: Нужно создавать текстуру заново при изменении размера изображения!

            unsafe
            {
                fixed (byte* p = Data)
                {
                    if (PixelFormat == ImagePixelFormat.Byte)
                        pixelType = PixelType.UnsignedByte;
                    else if (PixelFormat == ImagePixelFormat.Float)
                        pixelType = PixelType.Float;
                    else
                        throw new NotSupportedException();

                    switch ((uint) Format)
                    {
                        case (uint) ImageFormat.Greyscale:
                            if (PixelFormat == ImagePixelFormat.Byte)
                                pixelInternalFormat = PixelInternalFormat.R8;
                            else
                                pixelInternalFormat = PixelInternalFormat.R32f; // ?? R8?

                            pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Red;
                            break;
                        case (uint) ImageFormat.AmplitudePhase:
                        case (uint) ImageFormat.RealImaginative:
                            pixelInternalFormat = PixelInternalFormat.Rg8;
                            pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rg;
                            break;
                        case (uint) ImageFormat.RGB:
                            pixelInternalFormat = PixelInternalFormat.Rgb8;
                            pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rgb;
                            break;
                        case (uint) ImageFormat.RGBA:
                            pixelInternalFormat = PixelInternalFormat.Rgba8;
                            pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rgba;
                            break;
                        default:
                            throw new NotSupportedException($"Неподдерживаемая комбинация {nameof(PixelFormat)} = {PixelFormat}, {nameof(Format)} = {Format}.");
                    }
                }
            }

            if (OpenGlTextureId.HasValue)
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, pixelFormat, pixelType, Data);
            else
                GL.TexImage2D(TextureTarget.Texture2D, 0, pixelInternalFormat, Width, Height, 0, pixelFormat, pixelType, Data);

            OpenGlErrorThrower.ThrowIfAny();

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new[] {(int) TextureMinFilter.Nearest});
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new[] {(int) TextureMagFilter.Nearest});

            OpenGlErrorThrower.ThrowIfAny();
            OpenGlTextureId = textureId;

            var computeContext = Singleton.Get<OpenClApplication>().ComputeContext;
            ComputeBuffer = ComputeImage2D.CreateFromGLTexture2D(computeContext, ComputeMemoryFlags.ReadWrite, (int) TextureTarget.Texture2D, 0, textureId);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void FreeComputingDevice()
        {
            try
            {
                ComputeBuffer?.Dispose();
            }
            catch (Exception ex)
            {
                DebugLogger.Warning(ex);
            }

            if (OpenGlTextureId.HasValue)
                GL.DeleteTexture(OpenGlTextureId.Value);
        }

        public void Update()
        {
            ImageUtils.PopulateMinMax(this);
            ImageUpdated?.Invoke(new ImageUpdatedEventData(false));
        }

        public void UpdateFromBitmap(Bitmap bmp)
        {
            if (PixelFormat != ImagePixelFormat.Byte)
                throw new InvalidOperationException($"{nameof(ImageHandler)} имеет недопустимый формат пикселей: {PixelFormat}");

            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb && bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                throw new InvalidOperationException($"Неподдерживаемый формат изображения [{bmp.PixelFormat}]. Поддерживаются только форматы {nameof(System.Drawing.Imaging.PixelFormat.Format24bppRgb)} и {nameof(System.Drawing.Imaging.PixelFormat.Format32bppArgb)}.");
            
            var bitmapData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            Data = new byte[bmp.Width * bmp.Height * (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4)];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, Data, 0, Data.Length);
            bmp.UnlockBits(bitmapData);
            
            Format = bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb
                ? ImageFormat.RGB
                : ImageFormat.RGBA;

            Width = bmp.Width;
            Height = bmp.Height;
            
            _tags.SetOrAdd(ImageHandlerTagKeys.Thumbnail, ThumbnailGenerator.Generate(bmp));

            if (OpenGlTextureId.HasValue)
                UploadToComputingDevice(true);

            Update();
        }
        
        public static ImageHandler FromBitmap(Bitmap bmp, string title = null)
        {
            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb && bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                throw new InvalidOperationException($"Неподдерживаемый формат изображения [{bmp.PixelFormat}]. Поддерживаются только форматы {nameof(System.Drawing.Imaging.PixelFormat.Format24bppRgb)} и {nameof(System.Drawing.Imaging.PixelFormat.Format32bppArgb)}.");

            var imageHandler = new ImageHandler();

            var bitmapData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            imageHandler.Data = new byte[bmp.Width * bmp.Height * (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4)];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, imageHandler.Data, 0, imageHandler.Data.Length);
            bmp.UnlockBits(bitmapData);

            imageHandler.PixelFormat = ImagePixelFormat.Byte;
            imageHandler.Format = bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb
                ? ImageFormat.RGB
                : ImageFormat.RGBA;
            imageHandler.Width = bmp.Width;
            imageHandler.Height = bmp.Height;

            Singleton.Get<ImageHandlerRepository>().Add(imageHandler);

            if (title == null)
                title = "Bitmap " + Singleton.Get<ImageHandlerRepository>().GetAll().Count;

            imageHandler._tags.Add(ImageHandlerTagKeys.Title, title);
            imageHandler._tags.Add(ImageHandlerTagKeys.Thumbnail, Utils.ThumbnailGenerator.Generate(bmp));
            imageHandler.Update();

            return imageHandler;
        }

        public static ImageHandler Create(string title, int width, int height, ImageFormat imageFormat, ImagePixelFormat imagePixelFormat)
        {
            var imageHandler = new ImageHandler();
            imageHandler.Data = new byte[width * height * imageFormat.GetAttribute<ChannelsCountAttribute>().Value * imagePixelFormat.GetAttribute<SizeInBytesAttribute>().Value];
            imageHandler.Width = width;
            imageHandler.Height = height;
            imageHandler.PixelFormat = imagePixelFormat;
            imageHandler.Format = imageFormat;
            imageHandler._tags.Add(ImageHandlerTagKeys.Title, title);
            imageHandler._tags.Add(ImageHandlerTagKeys.Thumbnail, Utils.ThumbnailGenerator.Generate());
            imageHandler.Update();

            return imageHandler;
        }

        public static ImageHandler FromBitmapAsGreyscale(Bitmap bmp, string title = null)
        {
            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb && bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb && bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                throw new InvalidOperationException($"Неподдерживаемый формат изображения [{bmp.PixelFormat}]. Поддерживаются только форматы {nameof(System.Drawing.Imaging.PixelFormat.Format24bppRgb)},{nameof(System.Drawing.Imaging.PixelFormat.Format32bppRgb)} и {nameof(System.Drawing.Imaging.PixelFormat.Format32bppArgb)}.");

            var imageHandler = new ImageHandler();

            var bitmapData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            imageHandler.Data = new byte[bmp.Width * bmp.Height * sizeof(float)];
            imageHandler.PixelFormat = ImagePixelFormat.Float;
            imageHandler.Format = ImageFormat.Greyscale;
            imageHandler.Width = bmp.Width;
            imageHandler.Height = bmp.Height;

            float? min = null, max = null;
            unsafe
            {
                fixed (byte* pd = imageHandler.Data)
                {
                    var p0 = (byte*) bitmapData.Scan0;
                    var fpd = (float*) pd;
                    float v;
                    var dataStep = bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4;
                    var dataLength = bmp.Width * bmp.Height * dataStep;
                   

                    for (var i = 0; i < dataLength; i += dataStep)
                    {
                        v = (p0[i] * 0.33f + p0[i + 1] * 0.33f + p0[i + 2] * 0.33f) / 255f;

                        if (!min.HasValue || min > v)
                            min = v;

                        if (!max.HasValue || max < v)
                            max = v;

                        *fpd++ = v;
                    }
                }
            }

            bmp.UnlockBits(bitmapData);

            Singleton.Get<ImageHandlerRepository>().Add(imageHandler);

            if (title == null)
                title = "Bitmap " + Singleton.Get<ImageHandlerRepository>().GetAll().Count;

            imageHandler._tags.Add(ImageHandlerTagKeys.MaximumValueF, max);
            imageHandler._tags.Add(ImageHandlerTagKeys.MinimumValueF, min);
            imageHandler._tags.Add(ImageHandlerTagKeys.Title, title);
            imageHandler._tags.Add(ImageHandlerTagKeys.Thumbnail, Utils.ThumbnailGenerator.Generate(bmp));
            imageHandler.Update();

            return imageHandler;
        }
        
        public void Dispose()
        {
            FreeComputingDevice();
        }
    }

    internal class SizeInBytesAttribute : Attribute
    {
        public int Value { get; }

        public SizeInBytesAttribute(int value)
        {
            Value = value;
        }
    }

    internal class ChannelsCountAttribute : Attribute
    {
        public int Value { get; set; }

        public ChannelsCountAttribute(int value)
        {
            Value = value;
        }
    }

    public static class ImageHandlerExtensions
    {
        public static float GetRatio(this IImageHandler self)
        {
            return (float) self.Width / self.Height;
        }

        public static int GetChannelsCount(this IImageHandler self)
        {
            return self.Format.GetAttribute<ChannelsCountAttribute>().Value;
        }

        public static bool SizeEquals(this IImageHandler self, IImageHandler other)
        {
            return self.Width == other.Width && self.Height == other.Height;
        }

        public static FloatRange GetValueRangeForChannel(this IImageHandler self, int channel)
        {
            if (channel == 0)
                return new FloatRange((float)self.Tags[ImageHandlerTagKeys.MinimumValueF], (float)self.Tags[ImageHandlerTagKeys.MaximumValueF]);
            if (channel == 1)
                return new FloatRange((float) self.Tags[ImageHandlerTagKeys.MinimumValue2F], (float) self.Tags[ImageHandlerTagKeys.MaximumValue2F]);
            throw new NotImplementedException();
        }

        public static bool IsReady(this IImageHandler self)
        {
            return self != null && self.Ready;
        }

        public static string GetTitle(this IImageHandler self)
        {
            self.Tags.TryGetValue(ImageHandlerTagKeys.Title, out object title);
            return (string) title ?? "Image";
        }
    }

    public static class ImageHandlerTagKeys
    {
        public const string Title = "title";
        public const string Source = "source";
        public const string FileSystemPath = "fs_path";
        public const string Thumbnail = "thumbnail";
        public const string MinimumValueF = "minimum_value_float";
        public const string MaximumValueF = "maximum_value_float";
        public const string MinimumValue2F = "minimum_value_2_float";
        public const string MaximumValue2F = "maximum_value_2_float";
    }
}
using System;
using System.Collections.Generic;
using Common;

namespace Processing.DataAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ImageHandlerFilterAttribute : Attribute
    {
        public AllowedImageFormat AllowedImageFormat { get; set; }
        public AllowedImagePixelFormat AllowedPixelFormat { get; set; }
        public int? RequiredChannelCount { get; set; }
        public bool OnlyImages { get; set; }

        public ImageHandlerFilterAttribute(AllowedImageFormat allowedImageFormat = AllowedImageFormat.All, AllowedImagePixelFormat allowedPixelFormat = AllowedImagePixelFormat.All)
        {
            AllowedImageFormat = allowedImageFormat;
            AllowedPixelFormat = allowedPixelFormat;
        }

        public IEnumerable<ImageFormat> GetAllowedImageFormats()
        {
            return EnumExtensions.DecomposeFlags<ImageFormat>((int) AllowedImageFormat);
        }

        public IEnumerable<ImagePixelFormat> GetAllowedPixelFormats()
        {
            return EnumExtensions.DecomposeFlags<ImagePixelFormat>((int) AllowedPixelFormat);
        }
    }

    [Flags]
    public enum AllowedImagePixelFormat
    {
        None = 0,
        Byte = 32,
        Float = 64,
        All = Byte | Float
    }
    
    [Flags]
    public enum AllowedImageFormat
    {
        None = 0,
        Greyscale = 1,
        RGB = 2,
        RGBA = 4,
        AmplitudePhase = 8,
        RealImaginative = 16,
        All = Greyscale | RGB | RGBA | AmplitudePhase | RealImaginative
    }
}
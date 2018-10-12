using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using Cloo;
using Common;
using Processing;
using Processing.Computing;
using Processing.DataAttributes;
using Processing.DataProcessors;
using UserInterface.DataEditors.InterfaceBinding;

namespace UserInterface.DataProcessorViews
{
    public class SingleImageOutputDataProcessorView : DataProcessorDescriptor, IDataProcessorView, IImageHandler, IBindingProvider
    {
        public DataProcessorInfo Info { get; }

        public event Action<IImageHandler> OnImageCreate;
        public event Action OnUpdated;
        protected DataProcessorParameter<IImageHandler> OutputParameter => Output.As<IImageHandler>();

        public SingleImageOutputDataProcessorView(MethodInfo processorMethod) : base(processorMethod)
        {
            if (!typeof(IImageHandler).IsAssignableFrom(Output.ValueType))
                throw new InvalidOperationException();

            Info = new DataProcessorInfo(processorMethod);
        }

        public void Initialize()
        {
            foreach (var parameter in Parameters)
            {
                parameter.ValueUpdated += e =>
                {
                    if (e.Sender != this)
                        Compute();
                };
            }
        }

        public IEnumerable<object> GetOutputValues()
        {
            yield return Output.GetValue();
        }

        public IEnumerable<IBinding> GetBindings()
        {
            return Parameters;
        }

        public void Compute()
        {
            if (!AreAllParametersSet())
                return;

            var imageHandlerParameters = Parameters.OfType<DataProcessorParameter<IImageHandler>>();
            var imageHandlerInput = imageHandlerParameters.FirstOrDefault();

            bool redrawControls = false;
            if (imageHandlerInput != null)
            {
                if (!OutputParameter.HasValue)
                    redrawControls = true;

                var outputImageFilterAttribute = OutputParameter.GetAttribute<ImageHandlerFilterAttribute>();
                CreateOrUpdateOutputWithSameParametres(imageHandlerInput.GetValue(), Info.Name + " result", outputImageFilterAttribute?.GetAllowedPixelFormats().FirstOrDefault(), outputImageFilterAttribute?.GetAllowedImageFormats().FirstOrDefault());
            }

            using (StartOperationScope(redrawControls, imageHandlerParameters.Select(p => p.GetValue()).Concat(new[] { OutputParameter.GetValue() }).ToArray()))
            {
                Invoke();
            }
        }

        protected void CreateOrUpdateOutputWithSameParametres(IImageHandler image, string title, ImagePixelFormat? pixelFormat = null, ImageFormat? format = null)
        {
            CreateOrUpdateImageParameterWithSameProperties(image, title, OutputParameter, pixelFormat, format);
        }

        protected void CreateOrUpdateImageParameterWithSameProperties(IImageHandler image, string title, DataProcessorParameter<IImageHandler> parameterToCheck, ImagePixelFormat? pixelFormat = null, ImageFormat? format = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var imageToCheck = parameterToCheck.GetValue();
            if (imageToCheck == null || !image.SizeEquals(imageToCheck) || image.Format != imageToCheck.Format || image.PixelFormat != imageToCheck.PixelFormat)
            {
                imageToCheck?.FreeComputingDevice();

                parameterToCheck.SetValue(ImageHandler.Create(title, image.Width, image.Height, format ?? image.Format, pixelFormat ?? image.PixelFormat), this);
            }
        }

        protected ImageOperationScope StartOperationScope(bool redrawControls, params IImageHandler[] affectedImages)
        {
            return new ImageOperationScope(this, redrawControls, affectedImages);
        }

        protected class ImageOperationScope : IDisposable
        {
            private SingleImageOutputDataProcessorView _processor;
            private OpenClApplication.SingleOperationContext _singleOperationContext;
            private bool _redrawControls;

            public ImageOperationScope(SingleImageOutputDataProcessorView processor, params IImageHandler[] affectedImages) : this(processor, false, affectedImages)
            {
            }

            public ImageOperationScope(SingleImageOutputDataProcessorView processor, bool redrawControls, params IImageHandler[] affectedImages)
            {
                foreach (var affectedImage in affectedImages)
                {
                    if (!affectedImage.OpenGlTextureId.HasValue)
                        affectedImage.UploadToComputingDevice();
                }

                _processor = processor;
                _singleOperationContext = new OpenClApplication.SingleOperationContext(affectedImages);
                _redrawControls = redrawControls;
            }

            public void Dispose()
            {
                _singleOperationContext.Dispose();

                _processor.OnUpdated();
                _processor.OutputParameter.GetValue().Update();
                _processor.ImageUpdated?.Invoke(new ImageUpdatedEventData(_redrawControls));
            }
        }

        #region IImageHandler

        public bool Ready => OutputParameter.HasValue && OutputParameter.GetValue().Ready;
        public IDictionary<string, object> Tags => GetImageProperty<IDictionary<string, object>>();
        public event Action<ImageUpdatedEventData> ImageUpdated;
        public int Width => GetImageProperty<int>();
        public int Height => GetImageProperty<int>();
        public int PixelSizeInBytes => GetImageProperty<int>();
        public ImagePixelFormat PixelFormat => GetImageProperty<ImagePixelFormat>();
        public ImageFormat Format => GetImageProperty<ImageFormat>();
        public int? OpenGlTextureId => GetImageProperty<int?>();
        public ComputeImage2D ComputeBuffer => GetImageProperty<ComputeImage2D>();

        public void UploadToComputingDevice(bool forceUpdate = false)
        {
            OutputParameter.GetValue()?.UploadToComputingDevice(forceUpdate);
        }

        public void FreeComputingDevice()
        {
            OutputParameter.GetValue()?.FreeComputingDevice();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void DownloadFromComputingDevice()
        {
            OutputParameter.GetValue()?.DownloadFromComputingDevice();
        }

        public Bitmap ToBitmap(int channel = 0)
        {
            return OutputParameter.GetValue()?.ToBitmap(channel);
        }

        public IImageHandler ExtractSelection(ImageSelection selection)
        {
            return OutputParameter.GetValue()?.ExtractSelection(selection);
        }

        private T GetImagePropertyOrDefaultValue<T>(Func<IImageHandler, T> getValue)
        {
            return OutputParameter.HasValue ? getValue(OutputParameter.GetValue()) : (T) typeof(T).GetDefaultValue();
        }

        private T GetImageProperty<T>([CallerMemberName] string callerProperty = null)
        {
            if (!OutputParameter.HasValue)
                return (T) typeof(T).GetDefaultValue();

            return (T) typeof(IImageHandler).GetProperty(callerProperty).GetValue(OutputParameter.GetValue());
        }

        #endregion
    }
}
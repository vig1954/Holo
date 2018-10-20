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
        private bool _outputImageParametersUpdated;
        protected DataProcessorParameter<IImageHandler> OutputParameter => Output.As<IImageHandler>();
        public DataProcessorInfo Info { get; }

        public event Action<IImageHandler> OnImageCreate;
        public event Action OnUpdated;

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
                    if (e is DataProcessorParameterUpdatedEventArgs pe)
                    {
                        if (typeof(IImageHandler).IsAssignableFrom(parameter.ValueType) && parameter.HasValue)
                        {
                            if (pe.OldValue != null)
                            {
                                ((IImageHandler) pe.OldValue).ImageUpdated -= OnParameterImageUpdated;
                            }

                            ((IImageHandler) parameter.GetValue()).ImageUpdated += OnParameterImageUpdated;
                        }
                    }

                    if (e.Sender != this)
                        Compute();
                };
            }
        }

        private void OnParameterImageUpdated(ImageUpdatedEventData obj)
        {
            Compute();
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

            _outputImageParametersUpdated = false;

            var imageHandlerParameters = Parameters.OfType<DataProcessorParameter<IImageHandler>>();
            var imageHandlerInput = imageHandlerParameters.FirstOrDefault();
            
            if (imageHandlerInput != null)
            {

                var outputImageFilterAttribute = OutputParameter.GetAttribute<ImageHandlerFilterAttribute>();
                CreateOrUpdateOutputWithSameParametres(imageHandlerInput.GetValue(), Info.Name + " result", outputImageFilterAttribute?.GetAllowedPixelFormats().FirstOrDefault(), outputImageFilterAttribute?.GetAllowedImageFormats().FirstOrDefault());
            }
            else
            {
                var outputImageWidth = (int)(Parameters.FirstOrDefault(p => p.HasAttribute<OutputImageWidthAttribute>())?.GetValue() ?? throw new InvalidOperationException());
                var outputImageHeight = (int)(Parameters.FirstOrDefault(p => p.HasAttribute<OutputImageHeightAttribute>())?.GetValue() ?? throw new InvalidOperationException());
                var outputImageFormatAttribute = OutputParameter.GetAttribute<ImageHandlerFilterAttribute>() ?? throw new InvalidOperationException();
                var imageFormat = outputImageFormatAttribute.GetAllowedImageFormats().First();
                var imagePixelFormat = outputImageFormatAttribute.GetAllowedPixelFormats().First();

                var outputValue = OutputParameter.GetValue();
                if (outputValue == null || outputValue.Width != outputImageWidth || outputValue.Height != outputImageHeight || outputValue.PixelFormat != imagePixelFormat || outputValue.Format != imageFormat)
                {
                    var output = ImageHandler.Create(Info.Name + "result", outputImageWidth, outputImageHeight, imageFormat, imagePixelFormat);
                    OutputParameter.SetValue(output, this);
                    _outputImageParametersUpdated = true;
                }
            }

            // Output может быть единственным ImageHangler-ом. Нужно добавить механизм для указания параметров, представляющих размеры изображения

            using (StartOperationScope(_outputImageParametersUpdated, imageHandlerParameters.Select(p => p.GetValue()).Concat(new[] { OutputParameter.GetValue() }).ToArray()))
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

                _outputImageParametersUpdated = true;
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

                _processor.OnUpdated?.Invoke();
                _processor.OutputParameter.GetValue()?.Update();
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
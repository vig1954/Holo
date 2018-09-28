using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using Cloo;
using Common;
using Processing;
using Processing.DataProcessors;
using UserInterface.DataEditors.InterfaceBinding;

namespace UserInterface.DataProcessorViews
{
    public class SingleImageOutputDataProcessorView : IDataProcessorView, IImageHandler, IBindingProvider
    {
        protected DataProcessorDescriptor ProcessorDescriptor;
        protected DataProcessorParameter<IImageHandler> OutputParameter => ProcessorDescriptor.Output.As<IImageHandler>();

        public SingleImageOutputDataProcessorView(DataProcessorDescriptor processorDescriptor)
        {
            if (!typeof(IImageHandler).IsAssignableFrom(processorDescriptor.Output.Type))
                throw new InvalidOperationException();

            ProcessorDescriptor = processorDescriptor;
        }

        public IEnumerable<IBinding> GetBindings()
        {
            throw new NotImplementedException();
        }

        #region IImageHandler

        public bool Ready => OutputParameter.HasValue && OutputParameter.Get().Ready;
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
            OutputParameter.Get().UploadToComputingDevice(forceUpdate);
        }

        public void FreeComputingDevice()
        {
            OutputParameter.Get().FreeComputingDevice();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void DownloadFromComputingDevice()
        {
            OutputParameter.Get().DownloadFromComputingDevice();
        }

        public Bitmap ToBitmap(int channel = 0)
        {
            return OutputParameter.Get().ToBitmap(channel);
        }

        public IImageHandler ExtractSelection(ImageSelection selection)
        {
            return OutputParameter.Get().ExtractSelection(selection);
        }

        private T GetImagePropertyOrDefaultValue<T>(Func<IImageHandler, T> getValue)
        {
            return OutputParameter.HasValue ? getValue(OutputParameter.Get()) : (T) typeof(T).GetDefaultValue();
        }

        private T GetImageProperty<T>([CallerMemberName] string callerProperty = null)
        {
            if (!OutputParameter.HasValue)
                return (T) typeof(T).GetDefaultValue();

            return (T) typeof(IImageHandler).GetProperty(callerProperty).GetValue(OutputParameter.Get());
        }

        #endregion
        
    }
}
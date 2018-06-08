using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using OpenTK;
using Processing.Computing;
using Processing.DataBinding;
using Processing.Utils;

namespace Processing.DataProcessors
{
    [DataProcessor(Group = "Interferometry", Name="Freshnel", Tooltip = "Преобразование Френеля")]
    public class FrehnelTransformProcessor : SingleImageOutputDataProcessorBase
    {
        private Fourier _fourier;
        private IImageHandler _oldInput;
        private ComputeBuffer<Vector2> _freshnelInnerMultipliersX;
        private ComputeBuffer<Vector2> _freshnelInnerMultipliersY;

        private OpenClApplication App => Singleton.Get<OpenClApplication>();

        [Input]
        [ImageSlotWithSubfields("Голограмма", "Default", OnPropertyChanged = "InputImageChanged")]
        public IImageHandler Input { get; set; }

        [Input]
        [Number("Размер объекта, мм", 0, 50, 0.01f, OnPropertyChanged = "Process")]
        public float ObjectSize { get; set; } = 7;

        [Input]
        [Number("Расстояние, мм", 1, 5000, 1, OnPropertyChanged = "Process")]
        public float Distance { get; set; } = 135;

        [Input]
        [Number("Длина волны, нм", 380, 760, 1, OnPropertyChanged = "Process")]
        public float Wavelength { get; set; } = 500f;

        public override void Awake()
        {
        }

        public override void FreeResources()
        {
            _freshnelInnerMultipliersX?.Dispose();
            _freshnelInnerMultipliersY?.Dispose();
        }
        
        public void InputImageChanged()
        {
            // TODO: Code duplication (FourierTransformProcessor)
            if (Input == null)
            {
                Output?.FreeComputingDevice();
                Output = null;
                OnImageUpdated(new ImageUpdatedEventData(true));
                return;
            }

            if (Input != _oldInput)
            {
                if (_oldInput != null)
                    _oldInput.ImageUpdated -= InputOnImageUpdated;

                Input.ImageUpdated += InputOnImageUpdated;
                _oldInput = Input;
            }

            if (Output != null && Output.SizeEquals(Input))
                return;
            
            _freshnelInnerMultipliersX = new ComputeBuffer<Vector2>(App.ComputeContext, ComputeMemoryFlags.None, Input.Width);
            _freshnelInnerMultipliersY = new ComputeBuffer<Vector2>(App.ComputeContext, ComputeMemoryFlags.None, Input.Height);

            Output = ImageHandler.Create("fft result", Input.Width, Input.Height, ImageFormat.RealImaginative, ImagePixelFormat.Float);
            Output.UploadToComputingDevice();
            OnImageUpdated(new ImageUpdatedEventData(true));

            Process();
        }

        private void InputOnImageUpdated(ImageUpdatedEventData imageUpdatedEventData)
        {
            Process();
        }

        public void Process()
        {
            if (Input != null && Output != null && Input.Ready && Output.Ready)
            {
                using (new Timer("Freshnel"))
                {
                    using (new OpenClApplication.SingleOperationContext(Input, Output))
                    {
                        // TODO пересчитывать нужно только при смене настроек (не при обновлении изображения)
                        App.ExecuteKernel("freshnelGenerateInnerMultipliers", Input.Width, 1, _freshnelInnerMultipliersX, Wavelength / 1000f, Distance * 1000f, (float) Input.Width, ObjectSize * 1000f);
                        App.ExecuteKernel("freshnelGenerateInnerMultipliers", Input.Height, 1, _freshnelInnerMultipliersY, Wavelength / 1000f, Distance * 1000f, (float) Input.Height, ObjectSize * 1000f);

                        App.ExecuteKernel("freshnelMultiplyInner", Input.Width, Input.Height, Input, Output, _freshnelInnerMultipliersX, _freshnelInnerMultipliersY);

                        Fourier.Transform(Output);

                        App.ExecuteKernel("freshnelMultiplyInner", Input.Width, Input.Height, Output, Output, _freshnelInnerMultipliersX, _freshnelInnerMultipliersY);

                        ImageUtils.CyclicShift(Output);
                    }
                }
                OnUpdated();
                Output.Update();
            }
        }
    }
}

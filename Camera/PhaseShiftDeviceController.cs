using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace Camera
{
    public class PhaseShiftDeviceController
    {
        private const int MaximumStepCount = 4;

        private int[] _shiftValues = new int[CameraInputView.SeriesLength];
        private LowLevelPhaseShiftDeviceControllerAdapter LowLevelPhaseShiftController => Singleton.Get<LowLevelPhaseShiftDeviceControllerAdapter>();
        
        [BindToUI]
        public int ShiftStep { get; set; } = 400;
        
        [BindToUI]
        public bool UseShiftValues { get; set; }
        
        [BindToUI]
        public int ShiftValue1 { get => _shiftValues[0]; set => _shiftValues[0] = value; }
        
        [BindToUI]
        public int ShiftValue2 { get => _shiftValues[1]; set => _shiftValues[1] = value; }
        
        [BindToUI]
        public int ShiftValue3 { get => _shiftValues[2]; set => _shiftValues[2] = value; }
        
        [BindToUI]
        public int ShiftValue4 { get => _shiftValues[3]; set => _shiftValues[3] = value; }

        [BindToUI("Время установления сдвига, мс")]
        public int ShiftDelay { get; set; } = 1000;

        public async void ExecuteStep(int stepNumber)
        {
            if (!LowLevelPhaseShiftController.Connected)
                return;

            if (stepNumber >= MaximumStepCount)
                throw new InvalidOperationException($"{nameof(stepNumber)} must be less than {nameof(MaximumStepCount)}");

            var shift = UseShiftValues ? _shiftValues[stepNumber] : stepNumber * ShiftStep;
            
            await LowLevelPhaseShiftController.SetShift(shift, ShiftDelay);
        }

        public Task ExecuteStepAsync(int stepNumber)
        {
            if (!LowLevelPhaseShiftController.Connected)
                return Task.Delay(TimeSpan.FromMilliseconds(100));

            if (stepNumber >= MaximumStepCount)
                throw new InvalidOperationException($"{nameof(stepNumber)} must be less than {nameof(MaximumStepCount)}");

            var shift = UseShiftValues ? _shiftValues[stepNumber] : stepNumber * ShiftStep;
            
            return LowLevelPhaseShiftController.SetShift(shift, ShiftDelay);
        }

        public Task SetShiftAsync(short shiftParameterValue, int delayInMilliseconds = 500)
        {
            return LowLevelPhaseShiftController.SetShift(shiftParameterValue, delayInMilliseconds);
        }
    }
}

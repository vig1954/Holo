using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using rab1;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.DataEditors.InterfaceBinding.BindingEvents;

namespace Camera
{
    public class LowLevelPhaseShiftDeviceControllerAdapter
    {
        private const short ZeroPhaseShiftValue = 0x2000;
        private bool _phaseShiftDeviceConnected = false;
        private rab1.PhaseShiftDeviceController _inner;

        public string PortName { get; private set; }

        public bool Connected => _phaseShiftDeviceConnected;

        public int Shift { get; set; }

        public void Connect(string portName)
        {
            if (!_phaseShiftDeviceConnected && !PortName.IsNullOrEmpty())
            {
                PortName = portName;
                _inner = new rab1.PhaseShiftDeviceController(PortName);
                _inner.Initialize();
                _phaseShiftDeviceConnected = true;
            }
        }
        
        public void SetShift(int shift)
        {
            if (shift < 0 || shift > short.MaxValue - ZeroPhaseShiftValue)
                throw new InvalidOperationException();

            if (_phaseShiftDeviceConnected)
                _inner.SetShift((short) ((short) shift + ZeroPhaseShiftValue));
        }
        
        public async Task SetShift(int shiftValue, float delay, bool compensateHysteresis = false)
        {
            if (!_phaseShiftDeviceConnected)
                return;

            if (compensateHysteresis)
            {
                delay = delay / 2;
                SetShift(0);
                await Task.Delay(TimeSpan.FromMilliseconds(delay));
            }

            SetShift(shiftValue);
            await Task.Delay(TimeSpan.FromMilliseconds(delay));
        }

        [BindToUI]
        public void Disconnect()
        {
            _inner?.Dispose();
            _inner = null;
            _phaseShiftDeviceConnected = false;
            PortName = null;
        }

        ~LowLevelPhaseShiftDeviceControllerAdapter()
        {
            _inner?.Dispose();
            _inner = null;
        }
    }
}

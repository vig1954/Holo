using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace Camera
{
    public class LowLevelPhaseShiftDeviceControllerAdapter
    {
        private const short ZeroPhaseShiftValue = 0x2000;
        private bool _phaseShiftDeviceConnected = false;
        private PhaseShiftDeviceController _inner;

        public event Action<string> PhaseShiftDeviceDataReceived;

        public string PortName { get; private set; }

        public bool Connected => _phaseShiftDeviceConnected;

        public int Shift { get; set; }

        public void Connect(string portName)
        {
            if (!_phaseShiftDeviceConnected && !portName.IsNullOrEmpty())
            {
                PortName = portName;
                _inner = new PhaseShiftDeviceController(PortName);
                _inner.SerialPortDataRecieved += InnerOnSerialPortDataRecieved;
                _inner.Initialize();
                _phaseShiftDeviceConnected = true;
            }
        }

        private void InnerOnSerialPortDataRecieved(byte[] bytes, string s)
        {
            PhaseShiftDeviceDataReceived?.Invoke(s);
        }

        public void SetShift(int shift)
        {
            if (shift < 0 || shift > short.MaxValue - ZeroPhaseShiftValue)
                throw new InvalidOperationException();

            if (_phaseShiftDeviceConnected)
                _inner.SetShift((short) ((short) shift + ZeroPhaseShiftValue));
        }

        public void WriteRawBytes(byte byte1, byte byte2)
        {
            _inner.WriteBytes(new[] {byte1, byte2});
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

        public void Disconnect()
        {
            if (_inner != null)
                _inner.SerialPortDataRecieved -= InnerOnSerialPortDataRecieved;

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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using rab1;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace Camera
{
    public class PhaseShiftDeviceControllerAdapter
    {
        private const short ZeroPhaseShiftValue = 0x2000;
        private bool _phaseShiftDeviceConnected = false;
        private PhaseShiftDeviceController _inner;

        public IBindingManager<PhaseShiftDeviceControllerAdapter> BindingManager { get; set; }

        [BindToUI, ValueCollection(ValueCollectionProviderPropertyName = nameof(PortNames))]
        public string PortName { get; set; }
        public ObservableCollection<string> PortNames { get; }

        public PhaseShiftDeviceControllerAdapter()
        {
            PortNames = new ObservableCollection<string>();
            UpdatePortNames();
        }

        public void UpdatePortNames()
        {
            PortNames.Clear();
            PortNames.AddRange(SerialPort.GetPortNames());
        }

        [BindToUI]
        public void Connect()
        {
            if (!_phaseShiftDeviceConnected && !PortName.IsNullOrEmpty())
            {
                _inner = new PhaseShiftDeviceController(PortName);
                _inner.Initialize();
                _phaseShiftDeviceConnected = true;
            }
        }

        public void SetShift(float shift)
        {
            if (shift < 0 || shift > short.MaxValue - ZeroPhaseShiftValue)
                throw new InvalidOperationException();

            if (_phaseShiftDeviceConnected)
                _inner.SetShift((short)((short) shift + ZeroPhaseShiftValue));
        }

        public void SetShift(float step, int stepNumber, float delay)
        {
            if (!_phaseShiftDeviceConnected)
                return;

            SetShift(step * stepNumber);
            Thread.Sleep((int) delay); // TODO: сделать ожидание не блокирующим!
        }

        [BindToUI]
        public void Disconnect()
        {
            BindingManager.SetPropertyValue(c => c.PortName, null);
            _inner?.Dispose();
            _inner = null;
            _phaseShiftDeviceConnected = false;
        }

        ~PhaseShiftDeviceControllerAdapter()
        {
            _inner?.Dispose();
            _inner = null;
        }
    }
}

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
    public class PhaseShiftDeviceControllerAdapter
    {
        private const short ZeroPhaseShiftValue = 0x2000;
        private bool _phaseShiftDeviceConnected = false;
        private PhaseShiftDeviceController _inner;

        public IBindingManager<PhaseShiftDeviceControllerAdapter> BindingManager { get; set; }

        [BindToUI, ValueCollection(ValueCollectionProviderPropertyName = nameof(PortNames))]
        public string PortName { get; set; }
        public ObservableCollection<string> PortNames { get; }

        public bool Connected => _phaseShiftDeviceConnected;

        [BindToUI]
        public int Shift { get; set; }

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

                ToggleConnectAndDisconnectButtons(false);
            }
        }

        [OnBindedPropertyChanged(nameof(Shift))]
        public void OnShiftUpdated(ValueUpdatedEventArgs e)
        {
            if (e.Sender == this)
                return;

            SetShift(Shift);
        }

        public void SetShift(int shift)
        {
            if (shift < 0 || shift > short.MaxValue - ZeroPhaseShiftValue)
                throw new InvalidOperationException();

            if (_phaseShiftDeviceConnected)
            {
                BindingManager.SetPropertyValue(a => a.Shift, shift);
                _inner.SetShift((short) ((short) shift + ZeroPhaseShiftValue));
            }
        }
        
        public async Task SetShift(int shiftValue, float delay, bool compensateHysteresis)
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
            BindingManager.SetPropertyValue(c => c.PortName, null);
            _inner?.Dispose();
            _inner = null;
            _phaseShiftDeviceConnected = false;

            ToggleConnectAndDisconnectButtons(true);
        }

        ~PhaseShiftDeviceControllerAdapter()
        {
            _inner?.Dispose();
            _inner = null;
        }

        private void ToggleConnectAndDisconnectButtons(bool connect)
        {
            BindingManager.RaiseMethodBindingEvent(a => a.Connect(), new PerformBindableControlActionEvent(c => (c as Control).Enabled = connect, this));
            BindingManager.RaiseMethodBindingEvent(a => a.Disconnect(), new PerformBindableControlActionEvent(c => (c as Control).Enabled = !connect, this));
        }
    }
}

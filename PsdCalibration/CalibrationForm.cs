using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera;
using Common;
using Infrastructure;
using Newtonsoft.Json;

namespace PsdCalibration
{
    public partial class CalibrationForm : Form
    {
        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();
        private LowLevelPhaseShiftDeviceControllerAdapter LowLevelPhaseShiftController =>
            Singleton.Get<LowLevelPhaseShiftDeviceControllerAdapter>();

        private PictureBoxController _pictureBoxController;
        private bool _loadingNumericValues = false;
        private bool _autoSteppingInProgress = false;
        private IReadOnlyCollection<Control> _allControlsCache;
        
        public CalibrationForm()
        {
            InitializeComponent();

            _pictureBoxController = new PictureBoxController(LiveView);
            _allControlsCache = GetAllControls().ToArray();

            LoadNumericValues();

            foreach (var numericInput in _allControlsCache.OfType<NumericUpDown>())
            {
                numericInput.ValueChanged += (sender, args) =>
                {
                    if (_loadingNumericValues || _autoSteppingInProgress)
                        return;

                    Debounce.Invoke("NumericInputValueChanged", SaveNumericValues, TimeSpan.FromSeconds(1));
                };
            }

            CameraConnector.AvailableCamerasUpdated += CameraConnectorOnAvailableCamerasUpdated;
            CameraConnector.SessionOpened += CameraConnectorOnSessionOpened;
            CameraConnector.SessionClosed += CameraConnectorOnSessionClosed;
            CameraConnector.LiveViewUpdated += CameraConnectorOnLiveViewUpdated;

            CameraConnectorOnAvailableCamerasUpdated(CameraConnector.AvailableCameras);

            var ports = System.IO.Ports.SerialPort.GetPortNames();
            PortDropdown.Items.Add("Не подключена");
            foreach (var port in ports)
            {
                PortDropdown.Items.Add(port);
            }

            TogglePhaseShiftControls(enabled: false, togglePortSelector: false, toggleSteppingControls: true);

            LowLevelPhaseShiftController.PhaseShiftDeviceDataReceived += LowLevelPhaseShiftControllerOnPhaseShiftDeviceDataReceived;
        }

        private void LowLevelPhaseShiftControllerOnPhaseShiftDeviceDataReceived(string s)
        {
            Action action = () => PsdResponse.Text = $"[{DateTime.Now:hh:mm:ss}] {s}";

            if (PsdResponse.InvokeRequired)
                PsdResponse.Invoke(action);
            else
                action();
        }

        private void SaveNumericValues()
        {
            var numericInputs = _allControlsCache.OfType<NumericUpDown>();
            var numericValues = numericInputs.ToDictionary(n => n.Name, n => n.Value);
            var numericValuesJson = JsonConvert.SerializeObject(numericValues);

            Properties.Settings.Default.NumericValues = numericValuesJson;
            Properties.Settings.Default.Save();
        }

        private void LoadNumericValues()
        {
            _loadingNumericValues = true;

            var numericValues = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(Properties.Settings.Default.NumericValues);

            if (numericValues != null)
            {
                foreach (var numericInput in _allControlsCache.OfType<NumericUpDown>())
                {
                    if (numericValues.TryGetValue(numericInput.Name, out var value))
                    {
                        numericInput.Value = value;
                    }
                }
            }

            _loadingNumericValues = false;
        }

        private async void StartAutostepping()
        {
            if (_autoSteppingInProgress || !LowLevelPhaseShiftController.Connected || CameraConnector.ActiveCamera == null)
                return;

            _autoSteppingInProgress = true;

            TogglePhaseShiftControls(enabled: false, togglePortSelector: true, toggleSteppingControls: false);

            ToggleAutoStepping.Text = "Остановить автоперебор";

            var multiplier = StartPsdValue.Value < EndPsdValue.Value ? 1 : -1;
            CurrentPsdValue.Value = StartPsdValue.Value;

            while (_autoSteppingInProgress)
            {
                await Task.Delay(TimeSpan.FromSeconds((double) PsdValueChangeInterval.Value));

                LowLevelPhaseShiftController.SetShift((int) CurrentPsdValue.Value);

                if (EndPsdValue.Value > StartPsdValue.Value && CurrentPsdValue.Value >= EndPsdValue.Value ||
                    EndPsdValue.Value < StartPsdValue.Value && CurrentPsdValue.Value <= EndPsdValue.Value)
                {
                    CurrentPsdValue.Value = StartPsdValue.Value;
                }
                else
                {
                    CurrentPsdValue.Value += multiplier * PsdValueStep.Value;
                }
            }
        }

        private void StopAutostepping()
        {
            _autoSteppingInProgress = false;

            TogglePhaseShiftControls(enabled: true, togglePortSelector: true, toggleSteppingControls: false);

            ToggleAutoStepping.Text = "Начать автоперебор";
        }

        private void TogglePhaseShiftControls(bool enabled, bool togglePortSelector, bool toggleSteppingControls)
        {
            CurrentPsdValue.Enabled = enabled;
            EndPsdValue.Enabled = enabled;
            StartPsdValue.Enabled = enabled;
            Byte1Text.Enabled = enabled;
            Byte2Text.Enabled = enabled;
            WriteRawBytesButton.Enabled = enabled;

            if (togglePortSelector)
                PortDropdown.Enabled = enabled;

            if (toggleSteppingControls)
            {
                PsdValueChangeInterval.Enabled = enabled;
                PsdValueStep.Enabled = enabled;
            }
        }

        private void CameraConnectorOnLiveViewUpdated(Bitmap bitmap)
        {
            _pictureBoxController.SetImage(bitmap, true);
        }

        private void CameraConnectorOnSessionClosed()
        {
            CameraStatusLabel.Text = "Камера не подключена";
        }

        private void CameraConnectorOnSessionOpened()
        {
            CameraStatusLabel.Text = "Камера: " + CameraConnector.ActiveCamera.Info.szDeviceDescription;
        }

        private void CameraConnectorOnAvailableCamerasUpdated(IEnumerable<EDSDKLib.Camera> availableCameras)
        {
            if (availableCameras.Any() && CameraConnector.ActiveCamera == null)
            {
                CameraConnector.SetActiveCamera(availableCameras.First());
            }
        }

        private void CameraSettingsButton_Click(object sender, EventArgs e)
        {

        }

        private void PortDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var portName = (string) PortDropdown.SelectedItem;
            if (portName.StartsWith("COM", StringComparison.OrdinalIgnoreCase) )
            {
                if (LowLevelPhaseShiftController.Connected)
                    LowLevelPhaseShiftController.Disconnect();

                LowLevelPhaseShiftController.Connect(portName);
                LowLevelPhaseShiftController.SetShift((short)CurrentPsdValue.Value);

                TogglePhaseShiftControls(enabled: true, togglePortSelector: false, toggleSteppingControls: true);
            }
            else
            {
                if (LowLevelPhaseShiftController.Connected)
                    LowLevelPhaseShiftController.Disconnect();

                TogglePhaseShiftControls(enabled: false, togglePortSelector: false, toggleSteppingControls: true);
            }
        }

        private void CurrentPsdValue_ValueChanged(object sender, EventArgs e)
        {
            LowLevelPhaseShiftController.SetShift((short) CurrentPsdValue.Value);
        }

        private void StartPsdValue_ValueChanged(object sender, EventArgs e)
        {

        }

        private void EndPsdValue_ValueChanged(object sender, EventArgs e)
        {

        }

        private void PsdValueStep_ValueChanged(object sender, EventArgs e)
        {

        }

        private void PsdValueChangeInterval_ValueChanged(object sender, EventArgs e)
        {

        }

        private void CalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CameraConnector.SetActiveCamera(null);
            LowLevelPhaseShiftController.Disconnect();

            CameraConnector.Dispose();
        }

        private void ToggleAutoStepping_Click(object sender, EventArgs e)
        {
            if (_autoSteppingInProgress)
                StopAutostepping();
            else
                StartAutostepping();
        }

        private IEnumerable<Control> GetAllControls()
        {
            var children = new List<Control>();
            children.AddRange(Controls.Cast<Control>());

            foreach (var child in Controls)
            {
                children.AddRange(GetAllControls((Control)child));
            }

            return children;
        }

        private IEnumerable<Control> GetAllControls(Control control)
        {
            var children = new List<Control>();
            children.AddRange(control.Controls.Cast<Control>());

            foreach (var child in control.Controls)
            {
                children.AddRange(GetAllControls((Control)child));
            }

            return children;
        }

        private void Byte1Text_KeyUp(object sender, KeyEventArgs e)
        {
            var valid = TryParseByte(Byte1Text.Text, out _);

            Byte1Text.BackColor = valid ? Color.White : Color.Orange;
        }

        private void Byte2Text_KeyUp(object sender, KeyEventArgs e)
        {
            var valid = TryParseByte(Byte2Text.Text, out _);

            Byte2Text.BackColor = valid ? Color.White : Color.Orange;
        }

        private void WriteRawBytesButton_Click(object sender, EventArgs e)
        {
            if (!TryParseByte(Byte1Text.Text, out var byte1))
                return;

            if (!TryParseByte(Byte2Text.Text, out var byte2))
                return;

            LowLevelPhaseShiftController.WriteRawBytes(byte1, byte2);
        }

        private bool TryParseByte(string text, out byte result)
        {
            return byte.TryParse(text, NumberStyles.AllowHexSpecifier, null, out result);
        }
    }
}

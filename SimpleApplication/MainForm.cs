using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera;
using HolographicInterferometryVNext;
using Infrastructure;
using Processing;
using Processing.Computing;
using UserInterface.DataEditors;
using UserInterface.DataProcessorViews;
using UserInterface.ImageSeries;

namespace SimpleApplication
{
    public partial class MainForm : Form, IImageSeriesProvider
    {
        private const string ComPortIsDisabledString = "Выкл";

        private readonly CameraImageProvider _cameraImageProvider = new CameraImageProvider();
        private readonly Size _seriesSize = new Size(512, 512);

        // private CameraInputView _cameraInputView; // todo: use series controller instead of this
        private SeriesController _seriesController;
        private PhaseShiftDeviceController _phaseShiftDeviceController;
        private PictureBoxController _pictureBoxController;
        private RectangleSelectionTool _rectangleSelectionTool;
        private DataEditorManager _dataEditorManager;
        private DataEditorView _firstSeriesView;
        private DataEditorView _secondSeriesView;
        private ImageSeries _firstSeries;
        private ImageSeries _secondSeries;
        private PsdCalibrationForm _psdCalibrationForm;
        private IDataProcessorView _firstSeriesPsi4Processor;
        private IDataProcessorView _firstSeriesFreshnelProcessor;
        private IDataProcessorView _secondSeriesPsi4Processor;
        private IDataProcessorView _secondSeriesFreshnelProcessor;


        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();
        private LowLevelPhaseShiftDeviceControllerAdapter LowLevelPhaseShiftController => Singleton.Get<LowLevelPhaseShiftDeviceControllerAdapter>();

        public ImageSeries ImageSeries { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _cameraImageProvider.CaptureFromLiveView = true;

            _phaseShiftDeviceController = new PhaseShiftDeviceController();
            _seriesController = new SeriesController(this, _cameraImageProvider, _phaseShiftDeviceController);

            _cameraImageProvider.LiveViewImageUpdated += CameraImageProviderOnLiveViewImageUpdated;

            CameraConnector.AvailableCamerasUpdated += CameraConnectorOnAvailableCamerasUpdated;
            CameraConnector.SessionOpened += CameraConnectorOnSessionOpened;
            CameraConnector.SessionClosed += CameraConnectorOnSessionClosed;

            _rectangleSelectionTool = new RectangleSelectionTool {FixedSize = _seriesSize};
            _rectangleSelectionTool.OnRectangleUpdated += rect => _cameraImageProvider.LiveViewSelection = rect;
            _cameraImageProvider.LiveViewSelection = _rectangleSelectionTool.Rectangle;


            _pictureBoxController = new PictureBoxController(LiveView);
            _pictureBoxController.SetTool(_rectangleSelectionTool);

            LiveView.Paint += LiveViewOnPaint;

            _dataEditorManager = new DataEditorManager(PhaseDifferenceView);
            _firstSeriesView = _dataEditorManager.Add(PhaseDifferenceView, Orientation.Horizontal);
            _firstSeriesView.CloseEnabled = false;
            _firstSeriesView.SplitEnabled = false;

            _secondSeriesView = _dataEditorManager.Add(_firstSeriesView, Orientation.Vertical);
            _secondSeriesView.CloseEnabled = false;
            _secondSeriesView.SplitEnabled = false;

            _firstSeries = new ImageSeries(_seriesSize, "Серия 1");
            _firstSeriesPsi4Processor = DataProcessorViewCreator.For(typeof(Psi), nameof(Psi.Psi4)).Create();
            _firstSeriesFreshnelProcessor = DataProcessorViewCreator.For(typeof(Freshnel), nameof(Freshnel.Transform)).Create();

            _firstSeries.AddDataProcessor(_firstSeriesPsi4Processor);
            _firstSeries.AddDataProcessor(_firstSeriesFreshnelProcessor);
            _firstSeriesFreshnelProcessor.OnValueUpdated += () =>
            {
                if (!_firstSeriesView.HasData)
                {
                    _firstSeriesView.SetData((IImageHandler) _firstSeriesFreshnelProcessor.GetOutputValues().Single());
                }
            };

            _secondSeries = new ImageSeries(_seriesSize, "Серия 2");
            _secondSeriesPsi4Processor = DataProcessorViewCreator.For(typeof(Psi), nameof(Psi.Psi4)).Create();
            _secondSeriesFreshnelProcessor = DataProcessorViewCreator.For(typeof(Freshnel), nameof(Freshnel.Transform)).Create();

            _secondSeries.AddDataProcessor(_secondSeriesPsi4Processor);
            _secondSeries.AddDataProcessor(_secondSeriesFreshnelProcessor);
            _secondSeriesFreshnelProcessor.OnValueUpdated += () =>
            {
                if (!_secondSeriesView.HasData)
                {
                    _secondSeriesView.SetData((IImageHandler) _secondSeriesFreshnelProcessor.GetOutputValues().Single());
                }
            };

            CameraConnectorOnAvailableCamerasUpdated(CameraConnector.AvailableCameras);

            
            SerialPortNames.Items.Add(ComPortIsDisabledString);
            foreach (var portName in SerialPort.GetPortNames())
            {
                SerialPortNames.Items.Add(portName);
            }

            _psdCalibrationForm = new PsdCalibrationForm(_phaseShiftDeviceController);
        }

        private void CameraConnectorOnSessionClosed()
        {
            cameraStatusLabel.Text = "Камера не подключена";
        }

        private void CameraConnectorOnSessionOpened()
        {
            cameraStatusLabel.Text = "Камера: " + CameraConnector.ActiveCamera.Info.szDeviceDescription;
        }

        private void CameraConnectorOnAvailableCamerasUpdated(IEnumerable<EDSDKLib.Camera> availableCameras)
        {
            if (availableCameras.Any() && CameraConnector.ActiveCamera == null)
            {
                CameraConnector.SetActiveCamera(availableCameras.First());
            }
        }

        private void CameraImageProviderOnLiveViewImageUpdated()
        {
            _pictureBoxController.SetImage(_cameraImageProvider.LiveViewImage, true);
        }

        private void LiveViewOnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            _rectangleSelectionTool.DrawUi(e.Graphics);
        }

        private void CaptureFirstSeriesButton_Click(object sender, EventArgs e)
        {
            _seriesController.StartCapturing();
            ImageSeries = _firstSeries;
        }

        private void CaptureSecondSeriesButton_Click(object sender, EventArgs e)
        {
            _seriesController.StartCapturing();
            ImageSeries = _secondSeries;
        }

        private void SerialPortNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            var portName = (string) SerialPortNames.SelectedItem;
            if (portName == ComPortIsDisabledString)
            {
                if (LowLevelPhaseShiftController.Connected)
                {
                    LowLevelPhaseShiftController.Disconnect();
                    psdStatusLabel.Text = "Пьезокерамика - не подключена";
                }
            }
            else
            {
                LowLevelPhaseShiftController.Connect(portName);
                psdStatusLabel.Text = $"Пьезокерамика - {portName}";
            }
        }

        private void psdValue1_ValueChanged(object sender, EventArgs e)
        {
            _phaseShiftDeviceController.ShiftValue1 = (int) psdValue1.Value;
        }

        private void psdValue2_ValueChanged(object sender, EventArgs e)
        {
            _phaseShiftDeviceController.ShiftValue2 = (int) psdValue2.Value;
        }

        private void psdValue3_ValueChanged(object sender, EventArgs e)
        {
            _phaseShiftDeviceController.ShiftValue3 = (int) psdValue3.Value;
        }

        private void psdValue4_ValueChanged(object sender, EventArgs e)
        {
            _phaseShiftDeviceController.ShiftValue4 = (int) psdValue4.Value;
        }

        private void psiValue1_ValueChanged(object sender, EventArgs e)
        {
            SetPsi4Value(1, (float)psiValue1.Value);
        }
        
        private void psiValue2_ValueChanged(object sender, EventArgs e)
        {
            SetPsi4Value(2, (float)psiValue2.Value);
        }

        private void psiValue3_ValueChanged(object sender, EventArgs e)
        {
            SetPsi4Value(3, (float)psiValue3.Value);
        }

        private void psiValue4_ValueChanged(object sender, EventArgs e)
        {
            SetPsi4Value(4, (float)psiValue4.Value);
        }

        private void SetPsi4Value(int index, float value)
        {
            if (index < 1 || index > 4)
                throw new InvalidOperationException($"{nameof(index)} должен быть в диапазоне от 1 до 4.");

            _firstSeriesPsi4Processor["phaseShift" + index].SetValue(value, this);
            _secondSeriesPsi4Processor["phaseShift" + index].SetValue(value, this);
        }

        private void CalibrationButton_Click(object sender, EventArgs e)
        {
            _seriesController.StopCapturing();
            _psdCalibrationForm.Show();
        }

        private void freshnelDistance_ValueChanged(object sender, EventArgs e)
        {
            _firstSeriesFreshnelProcessor["distance"].SetValue((float) freshnelDistance.Value, this);
            _secondSeriesFreshnelProcessor["distance"].SetValue((float) freshnelDistance.Value, this);
        }

        private void freshnelObjectSize_ValueChanged(object sender, EventArgs e)
        {
            _firstSeriesFreshnelProcessor["objectSize"].SetValue((float) freshnelObjectSize.Value, this);
            _secondSeriesFreshnelProcessor["objectSize"].SetValue((float) freshnelObjectSize.Value, this);
        }
    }
}
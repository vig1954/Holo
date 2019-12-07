using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using Camera;
using Common;
using Infrastructure;
using Processing;
using Processing.Computing;
using UserInterface.DataEditors;
using UserInterface.DataProcessorViews;
using UserInterface.ImageSeries;
using UserInterface.WorkspacePanel.ImageSeries;

namespace SimpleApplication
{
    public partial class MainForm : Form, IImageSeriesProvider
    {
        private const string ComPortIsDisabledString = "Выкл";

        private readonly CameraImageProvider _cameraImageProvider = new CameraImageProvider();
        private readonly Size _seriesSize = new Size(512, 512);

        private bool _isLoading = false;

        // private CameraInputView _cameraInputView; // todo: use series controller instead of this
        private SeriesController _seriesController;
        private PhaseShiftDeviceView _phaseShiftDeviceController;
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
        private AccumulatorInfo _firstAccumulator = new AccumulatorInfo();
        private AccumulatorInfo _secondAccumulator = new AccumulatorInfo();
        private bool _enableAccumulation;
        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();
        private CameraSettingsForm _cameraSettingsForm = new CameraSettingsForm(new CameraSettings());

        private LowLevelPhaseShiftDeviceControllerAdapter LowLevelPhaseShiftController =>
            Singleton.Get<LowLevelPhaseShiftDeviceControllerAdapter>();

        public ImageSeries ImageSeries { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _cameraImageProvider.CaptureFromLiveView = true;

            _phaseShiftDeviceController = new PhaseShiftDeviceView();
            _phaseShiftDeviceController.UseShiftValues = true;

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

//            var comparisonProcessor = DataProcessorViewCreator.For(typeof(ImageProcessing), nameof(ImageProcessing.Divide)).Create();
            var comparisonProcessor = DataProcessorViewCreator
                .For(typeof(ImageProcessing), nameof(ImageProcessing.ExtractWithAutoWeight)).Create();


            comparisonProcessor["image1"].SetValue(_firstSeriesFreshnelProcessor, this);
            comparisonProcessor["image2"].SetValue(_secondSeriesFreshnelProcessor, this);
            comparisonProcessor.OnValueUpdated += () =>
            {
                if (!PhaseDifferenceView.HasData)
                {
                    PhaseDifferenceView.SetData((IImageHandler) comparisonProcessor.GetOutputValues().Single());
                }
            };

            PhaseDifferenceView.CloseEnabled = false;

            _dataEditorManager = new DataEditorManager(PhaseDifferenceView);
            _firstSeriesView = _dataEditorManager.Add(PhaseDifferenceView, Orientation.Horizontal);
            _firstSeriesView.CloseEnabled = false;
            _firstSeriesView.SplitEnabled = false;

            _secondSeriesView = _dataEditorManager.Add(_firstSeriesView, Orientation.Vertical);
            _secondSeriesView.CloseEnabled = false;
            _secondSeriesView.SplitEnabled = false;

            PhaseDifferenceView.SplitEnabled = false;

            _firstSeries = new ImageSeries(_seriesSize, "Серия 1");
            _firstSeriesPsi4Processor = DataProcessorViewCreator.For(typeof(Psi), nameof(Psi.Psi4)).Create();
            _firstSeriesFreshnelProcessor =
                DataProcessorViewCreator.For(typeof(Freshnel), nameof(Freshnel.Transform)).Create();

            _firstSeries.AddDataProcessor(_firstSeriesPsi4Processor);
            _firstSeries.AddDataProcessor(_firstSeriesFreshnelProcessor);
            _firstSeriesFreshnelProcessor.OnValueUpdated += () =>
            {
                var imageHandler = (IImageHandler) _firstSeriesFreshnelProcessor.GetOutputValues().Single();
                if (!_firstSeriesView.HasData)
                {
                    _firstSeriesView.SetData(imageHandler);
                }

                comparisonProcessor["image1"].SetValue(imageHandler, this);
                comparisonProcessor.Compute();
            };

            _secondSeries = new ImageSeries(_seriesSize, "Серия 2");
            _secondSeriesPsi4Processor = DataProcessorViewCreator.For(typeof(Psi), nameof(Psi.Psi4)).Create();
            _secondSeriesFreshnelProcessor =
                DataProcessorViewCreator.For(typeof(Freshnel), nameof(Freshnel.Transform)).Create();

            _secondSeries.AddDataProcessor(_secondSeriesPsi4Processor);
            _secondSeries.AddDataProcessor(_secondSeriesFreshnelProcessor);
            _secondSeriesFreshnelProcessor.OnValueUpdated += () =>
            {
                var imageHandler = (IImageHandler) _secondSeriesFreshnelProcessor.GetOutputValues().Single();
                if (!_secondSeriesView.HasData)
                {
                    _secondSeriesView.SetData(imageHandler);
                }

                comparisonProcessor["image2"].SetValue(imageHandler, this);
                comparisonProcessor.Compute();
            };

            CameraConnectorOnAvailableCamerasUpdated(CameraConnector.AvailableCameras);

            SerialPortNames.Items.Add(ComPortIsDisabledString);
            foreach (var portName in SerialPort.GetPortNames())
            {
                SerialPortNames.Items.Add(portName);
            }

            _psdCalibrationForm = new PsdCalibrationForm(_phaseShiftDeviceController);

            this.Closing += (o, args) =>
            {
                _seriesController.StopCapturing();

                CameraConnector.SetActiveCamera(null);
                LowLevelPhaseShiftController.Disconnect();

                CameraConnector.Dispose();
            };

            LoadSettings();
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
            if (ImageSeries == _firstSeries && _seriesController.CaptureImages)
            {
                _seriesController.StopCapturing();
            }
            else
            {
                _seriesController.StartCapturing();
                ImageSeries = _firstSeries;
            }
        }

        private void CaptureSecondSeriesButton_Click(object sender, EventArgs e)
        {
            if (ImageSeries == _secondSeries && _seriesController.CaptureImages)
            {
                _seriesController.StopCapturing();
            }
            else
            {
                _seriesController.StartCapturing();
                ImageSeries = _secondSeries;
            }
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
            UpdateSettings(s => s.Psd1 = _phaseShiftDeviceController.ShiftValue1);
        }

        private void psdValue2_ValueChanged(object sender, EventArgs e)
        {
            _phaseShiftDeviceController.ShiftValue2 = (int) psdValue2.Value;
            UpdateSettings(s => s.Psd2 = _phaseShiftDeviceController.ShiftValue2);
        }

        private void psdValue3_ValueChanged(object sender, EventArgs e)
        {
            _phaseShiftDeviceController.ShiftValue3 = (int) psdValue3.Value;
            UpdateSettings(s => s.Psd3 = _phaseShiftDeviceController.ShiftValue3);
        }

        private void psdValue4_ValueChanged(object sender, EventArgs e)
        {
            _phaseShiftDeviceController.ShiftValue4 = (int) psdValue4.Value;
            UpdateSettings(s => s.Psd4 = _phaseShiftDeviceController.ShiftValue4);
        }

        private void psiValue1_ValueChanged(object sender, EventArgs e)
        {
            SetPsi4Value(1, (float) psiValue1.Value);
            UpdateSettings(s => s.Psi1 = (float) psiValue1.Value);
        }

        private void psiValue2_ValueChanged(object sender, EventArgs e)
        {
            SetPsi4Value(2, (float) psiValue2.Value);
            UpdateSettings(s => s.Psi2 = (float) psiValue2.Value);
        }

        private void psiValue3_ValueChanged(object sender, EventArgs e)
        {
            SetPsi4Value(3, (float) psiValue3.Value);
            UpdateSettings(s => s.Psi3 = (float) psiValue3.Value);
        }

        private void psiValue4_ValueChanged(object sender, EventArgs e)
        {
            SetPsi4Value(4, (float) psiValue4.Value);
            UpdateSettings(s => s.Psi4 = (float) psiValue4.Value);
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
            UpdateFreshnelDistance();

            UpdateSettings(s => s.FreshnelDistance = (float) freshnelDistance.Value);
        }

        private void UpdateFreshnelDistance()
        {
            var distance = (float) (freshnelDistance.Value + FreshnelDistanceDecimals.Value / 10);

            _firstSeriesFreshnelProcessor["distance"].SetValue(distance, this);
            _secondSeriesFreshnelProcessor["distance"].SetValue(distance, this);

            if (UpdateBoth.Checked)
            {
                _firstSeriesFreshnelProcessor.Compute();
                _secondSeriesFreshnelProcessor.Compute();
            }
        }

        private void freshnelObjectSize_ValueChanged(object sender, EventArgs e)
        {
            _firstSeriesFreshnelProcessor["objectSize"].SetValue((float) freshnelObjectSize.Value, this);
            _secondSeriesFreshnelProcessor["objectSize"].SetValue((float) freshnelObjectSize.Value, this);

            if (UpdateBoth.Checked)
            {
                _firstSeriesFreshnelProcessor.Compute();
                _secondSeriesFreshnelProcessor.Compute();
            }
            UpdateSettings(s => s.FreshnelObjectSize = (float) freshnelObjectSize.Value);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            UpdateSettings(s => s.Splitter1Distance = splitContainer1.SplitterDistance);
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            UpdateSettings(s => s.Splitter2Distance = splitContainer2.SplitterDistance);
        }

        private void UpdateSettings(Action<Properties.Settings> modify)
        {
            if (_isLoading)
                return;

            modify(Properties.Settings.Default);
            Properties.Settings.Default.Save();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            UpdateSettings(s =>
            {
                s.FormSize = this.Size;
                s.IsFormMaximized = this.WindowState == FormWindowState.Maximized;
            });
        }

        private void LoadSettings()
        {
            _isLoading = true;

            var s = Properties.Settings.Default;

            this.Size = s.FormSize;
            if (s.IsFormMaximized)
                this.WindowState = FormWindowState.Maximized;

            psiValue1.Value = (decimal) s.Psi1;
            psiValue2.Value = (decimal) s.Psi2;
            psiValue3.Value = (decimal) s.Psi3;
            psiValue4.Value = (decimal) s.Psi4;

            psdValue1.Value = s.Psd1;
            _phaseShiftDeviceController.ShiftValue1 = s.Psd1;

            psdValue2.Value = s.Psd2;
            _phaseShiftDeviceController.ShiftValue2 = s.Psd2;

            psdValue3.Value = s.Psd3;
            _phaseShiftDeviceController.ShiftValue3 = s.Psd3;

            psdValue4.Value = s.Psd4;
            _phaseShiftDeviceController.ShiftValue4 = s.Psd4;

            freshnelDistance.Value = (decimal) s.FreshnelDistance;
            FreshnelDistanceDecimals.Value = (decimal) s.FreshnelDistanceDecimals;
            freshnelObjectSize.Value = (decimal) s.FreshnelObjectSize;

            try
            {
                splitContainer1.SplitterDistance = s.Splitter1Distance;
                splitContainer2.SplitterDistance = s.Splitter2Distance;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка при загрузке настроек!", MessageBoxButtons.OK);
            }

            if (!s.DateEditorManagerSettings.IsNullOrEmpty())
            {
                try
                {
                    _dataEditorManager.ApplySettings(s.DateEditorManagerSettings);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Несмертельная ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            _isLoading = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateSettings(s => s.DateEditorManagerSettings = _dataEditorManager.GetSettings());
        }

        private void FreshnelDistanceDecimals_ValueChanged(object sender, EventArgs e)
        {
            UpdateFreshnelDistance();

            UpdateSettings(s => s.FreshnelDistanceDecimals = (float)FreshnelDistanceDecimals.Value);
        }

        private void ToggleAccumulation_Click(object sender, EventArgs e)
        {
            if (_enableAccumulation)
                DisableAccumulation();
            else
                EnableAccumulation();
        }

        private void EnableAccumulation()
        {
            ToggleAccumulation.Text = "Отключить накопление";

            CreateAccumulatorIfNeeded(_firstAccumulator, _firstSeriesFreshnelProcessor, _firstSeriesView);
            CreateAccumulatorIfNeeded(_secondAccumulator, _secondSeriesFreshnelProcessor, _secondSeriesView);

            _enableAccumulation = true;
        }

        private void CreateAccumulatorIfNeeded(AccumulatorInfo accumulator, IDataProcessorView freshnelProcessor,
            DataEditorView dataView)
        {
            accumulator.Initialized = false;

            if (accumulator.DataProcessorView != null)
                return;

            accumulator.DataProcessorView = DataProcessorViewCreator.For(typeof(ImageProcessing), nameof(ImageProcessing.Accumulate)).Create();

            freshnelProcessor.OnValueUpdated += () =>
            {
                InitOrUpdateAccumulator(accumulator, freshnelProcessor, dataView);
            };
        }

        private void InitOrUpdateAccumulator(AccumulatorInfo accumulator, IDataProcessorView freshnelProcessor, DataEditorView dataView)
        {
            if (!_enableAccumulation)
                return;

            if (!accumulator.Initialized)
            {
                var freshnelOutput = (IImageHandler)freshnelProcessor.GetOutputValues().Single();
                if (freshnelOutput == null)
                    return;

                accumulator.Accumulator = freshnelOutput.Duplicate();
                accumulator.Counter = 1;

                // accumulator.DataProcessorView["output"].SetValue(accumulator.Accumulator, this);
                accumulator.DataProcessorView.GetOutputs().Single().SetValue(accumulator.Accumulator, this);
                accumulator.DataProcessorView["input"].SetValue(freshnelOutput, this);

                dataView.SetData(accumulator.Accumulator);

                accumulator.Initialized = true;
            }
            else
            {
                accumulator.DataProcessorView["counter"].SetValue(accumulator.Counter, this);
                accumulator.DataProcessorView.Compute();

                accumulator.Counter++;
            }
        }

        private void DisableAccumulation()
        {
            ToggleAccumulation.Text = "Включить накопление";

            _firstSeriesView.SetData(null);
            _secondSeriesView.SetData(null);

            _enableAccumulation = false;
        }

        private class AccumulatorInfo
        {
            public float Counter { get; set; }
            public bool Initialized { get; set; }
            public IImageHandler Accumulator { get; set; }
            public IDataProcessorView DataProcessorView { get; set; }
        }

        private void CameraSettingsButton_Click(object sender, EventArgs e)
        {
            _cameraSettingsForm.Show();
        }

        private void SaveData_Click(object sender, EventArgs e)
        {
            _seriesController.StopCapturing();

            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var directory = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                var now = DateTime.Now;
                var saveDirectory =
                    directory.CreateSubdirectory(
                        $"{now.Day}-{now.Month}-{now.Year} {now.Hour:00}-{now.Minute:00}-{now.Second:00}");

                var counter = 1;
                if (_firstSeries.Inputs.All(i => i.Ready))
                {
                    foreach (var input in _firstSeries.Inputs)
                    {
                        using (var stream = File.Create(Path.Combine(saveDirectory.FullName, $"series1-{counter++}.bin")))
                        {
                            input.Save(stream);
                        }
                    }
                }

                if (_secondSeries.Inputs.All(i => i.Ready))
                {
                    counter = 1;
                    foreach (var input in _secondSeries.Inputs)
                    {
                        using (var stream = File.Create(Path.Combine(saveDirectory.FullName, $"series2-{counter++}.bin")))
                        {
                            input.Save(stream);
                        }
                    }
                }

                if (_firstSeriesFreshnelProcessor.GetOutputValues().Single() is ImageHandler firstFreshnelOutput)
                {
                    using (var stream = File.Create(Path.Combine(saveDirectory.FullName, "freshnel1.bin")))
                    {
                        firstFreshnelOutput.Save(stream);
                    }
                }

                if (_secondSeriesFreshnelProcessor.GetOutputValues().Single() is ImageHandler secondFreshnelOutput)
                {
                    using (var stream = File.Create(Path.Combine(saveDirectory.FullName, "freshnel2.bin")))
                    {
                        secondFreshnelOutput.Save(stream);
                    }
                }
            }


            _seriesController.StartCapturing();
        }

        private void ShowFunctions_Click(object sender, EventArgs e)
        {
            var frm = new PhaseDisambiguationNaiveForm(new Dictionary<string, ImageHandler>
            {
                ["Преобразование Френеля над первой серией"] = _firstSeriesFreshnelProcessor.GetOutputValues().Single() as ImageHandler,
                ["Преобразование Френеля над второй серией"] = _secondSeriesFreshnelProcessor.GetOutputValues().Single() as ImageHandler,
                ["Разность над преобразованиями"] = _secondSeriesFreshnelProcessor.GetOutputValues().Single() as ImageHandler,
            });
        }
    }
}
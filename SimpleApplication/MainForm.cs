using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera;
using Infrastructure;
using Processing.Computing;
using UserInterface.DataEditors;
using UserInterface.DataProcessorViews;
using UserInterface.ImageSeries;

namespace SimpleApplication
{
    public partial class MainForm : Form
    {
        private readonly Size SeriesSize = new Size(512, 512);

        private CameraInputView _cameraInputView;
        private PhaseShiftDeviceController _phaseShiftDeviceController;
        private PictureBoxController _pictureBoxController;
        private RectangleSelectionTool _rectangleSelectionTool;
        private DataEditorManager _dataEditorManager;
        private DataEditorView _firstSeriesView;
        private DataEditorView _secondSeriesView;
        private ImageSeries _firstSeries;
        private ImageSeries _secondSeries;

        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _phaseShiftDeviceController = new PhaseShiftDeviceController();
            _cameraInputView = new CameraInputView(_phaseShiftDeviceController);

            _cameraInputView.LiveViewImageUpdated += CameraInputViewOnLiveViewImageUpdated;

            CameraConnector.AvailableCamerasUpdated += CameraConnectorOnAvailableCamerasUpdated;
            CameraConnector.SessionOpened += CameraConnectorOnSessionOpened;
            CameraConnector.SessionClosed += CameraConnectorOnSessionClosed;

            _rectangleSelectionTool = new RectangleSelectionTool {FixedSize = SeriesSize};
            
            _pictureBoxController = new PictureBoxController(LiveView);
            _pictureBoxController.SetTool(_rectangleSelectionTool);

            LiveView.Paint += LiveViewOnPaint;

            _dataEditorManager = new DataEditorManager(PhaseDifferenceView);
            _firstSeriesView = _dataEditorManager.Add(PhaseDifferenceView, Orientation.Vertical);
            _secondSeriesView = _dataEditorManager.Add(_firstSeriesView, Orientation.Horizontal);

            _firstSeries = new ImageSeries(SeriesSize, "Серия 1");
            var firstSeriesPsi4Processor = DataProcessorViewCreator.For(typeof(Psi),nameof(Psi.Psi4)).Create();
            var firstSeriesFreshnelProcessor = DataProcessorViewCreator.For(typeof(Freshnel),nameof(Freshnel.Transform)).Create();

            _firstSeries.AddDataProcessor(firstSeriesPsi4Processor);
            _firstSeries.AddDataProcessor(firstSeriesFreshnelProcessor);
            _firstSeriesView.SetData(firstSeriesFreshnelProcessor);

            _secondSeries = new ImageSeries(SeriesSize, "Серия 2");
            var secondSeriesPsi4Processor = DataProcessorViewCreator.For(typeof(Psi),nameof(Psi.Psi4)).Create();
            var secondSeriesFreshnelProcessor = DataProcessorViewCreator.For(typeof(Freshnel),nameof(Freshnel.Transform)).Create();

            _secondSeries.AddDataProcessor(secondSeriesPsi4Processor);
            _secondSeries.AddDataProcessor(secondSeriesFreshnelProcessor);
            _secondSeriesView.SetData(secondSeriesFreshnelProcessor);
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
            if (availableCameras.Any())
            {
                _cameraInputView.Camera = availableCameras.First();
            }
        }
        private void CameraInputViewOnLiveViewImageUpdated()
        {
            _pictureBoxController.SetImage(_cameraInputView.LiveViewImage, true);
        }

        private void LiveViewOnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            _rectangleSelectionTool.DrawUi(e.Graphics);
        }

        private void CaptureFirstSeriesButton_Click(object sender, EventArgs e)
        {
            _cameraInputView.ImageSeries = _firstSeries;
        }

        private void CaptureSecondSeriesButton_Click(object sender, EventArgs e)
        {

        }
    }
}

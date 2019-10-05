using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera;
using Camera.PhaseShiftDeviceCalibration;
using Common;
using Infrastructure;
using UserInterface.DataEditors.Tools;

namespace SimpleApplication
{
    public partial class PsdCalibrationForm : Form
    {
        private readonly PhaseShiftDeviceController PhaseShiftDeviceController;

        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();

        private PlotDrawer _plotDrawer;
        private SegmentSelectionForm _segmentSelectionForm = new SegmentSelectionForm();
        private PictureBoxController _pictureBoxController;
        private Segment _segment;
        private Rectangle _segmentViewRectangle;
        private Point _segmentStartPoint;
        private SegmentSelectionTool _segmentSelectionTool;
        private Segment _previewSegment;
        private bool _calibrationInProgress;
        private Bitmap _currentPreviewRegion;
        private bool _recordView;
        private float[] _liveViewData;


        public PsdCalibrationForm(PhaseShiftDeviceController phaseShiftDeviceController)
        {
            InitializeComponent();

            _segmentSelectionForm.SegmentSelected += SegmentSelectionFormOnSegmentSelected;
            _pictureBoxController = new PictureBoxController(DataView);

            PhaseShiftDeviceController = phaseShiftDeviceController;

            _plotDrawer = new PlotDrawer();
        }

        private void SegmentSelectionFormOnSegmentSelected(Segment segment)
        {
            _segment = segment;

            var centerX = segment.X0 + (segment.X1 - segment.X0) / 2;
            var centerY = segment.Y0 + (segment.Y1 - segment.Y0) / 2;
            var width = Math.Abs(segment.X1 - segment.X0);
            var height = Math.Abs(segment.Y1 - segment.Y0);
            var maxSize = Math.Max(width, height);
            var padding = maxSize * 0.2f;
            var regionSize = maxSize + padding;

            _segmentViewRectangle = new Rectangle((int) (centerX - regionSize / 2), (int) (centerY - regionSize / 2), (int) regionSize, (int) regionSize);
            _segmentStartPoint = new Point((int) (padding + segment.X0), (int) (padding + segment.Y0));

            _previewSegment = new Segment
            {
                X0 = segment.X0 - _segmentStartPoint.X,
                X1 = segment.X1 - _segmentStartPoint.X,
                Y0 = segment.Y0 - _segmentStartPoint.Y,
                Y1 = segment.Y1 - _segmentStartPoint.Y,
            };

            _segmentSelectionTool.Segment = _previewSegment;
        }

        private void DataView_DoubleClick(object sender, EventArgs e)
        {
            _segmentSelectionForm.Show();
        }

        private void PsdCalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }

            CameraConnector.LiveViewUpdated -= CameraConnectorOnLiveViewUpdated;
        }

        private void PsdCalibrationForm_Shown(object sender, EventArgs e)
        {
            CameraConnector.LiveViewUpdated += CameraConnectorOnLiveViewUpdated;
        }

        private void CameraConnectorOnLiveViewUpdated(Bitmap bmp)
        {
            if (!_recordView)
            {
                var image = SelectionUtil.ExtractSelection(bmp, _segmentViewRectangle);
                _currentPreviewRegion = image;
                _pictureBoxController.SetImage(image, true);

                _liveViewData = BitmapUtil.ExtractSegment(_currentPreviewRegion, _previewSegment);
            }
        }

        private void DataView_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            _segmentSelectionTool.DrawUi(e.Graphics);
        }

        private void Calibrate_Click(object sender, EventArgs e)
        {
            if (_calibrationInProgress)
                StopCalibration();
            else
                StartCalibration();
        }

        private async void StartCalibration()
        {
            if (_calibrationInProgress)
                return;

            _calibrationInProgress = true;
            Calibrate.Text = "Остановить";

            Minimum.Enabled = false;
            Maximum.Enabled = false;
            Step.Enabled = false;
            Time.Enabled = false;
            Shift.Enabled = false;
            RecordList.Enabled = false;

            var recordContainer = new RecordContainer("Запись " + RecordList.Items.Count, (short) Minimum.Value, (short) Maximum.Value, (short) Step.Value, (short) Time.Value);
            RecordList.Items.Add(recordContainer);
            RecordList.SelectedItem = recordContainer;

            while (_calibrationInProgress)
            {
                var shift = (short) Shift.Value;
                await PhaseShiftDeviceController.SetShiftAsync(shift, (int) (1000 * Time.Value));

                var data = BitmapUtil.ExtractSegment(_currentPreviewRegion, _previewSegment);

                var record = new Record(_currentPreviewRegion, data, shift);
                recordContainer.Add(shift, record);
            }
        }

        private void StopCalibration()
        {
            if (!_calibrationInProgress)
                return;

            _calibrationInProgress = false;
            Calibrate.Text = "Начать";

            Minimum.Enabled = true;
            Maximum.Enabled = true;
            Step.Enabled = true;
            Time.Enabled = true;
            Shift.Enabled = true;
            RecordList.Enabled = true;
        }

        private class Record
        {
            public Bitmap PreviewRegion { get; }
            public float[] Data { get; }
            private short Shift { get; }

            public Record(Bitmap previewRegion, float[] data, short shift)
            {
                Data = data;
                PreviewRegion = previewRegion;
                Shift = shift;
            }
        }

        private class RecordContainer
        {
            private Dictionary<short, Record> _records = new Dictionary<short, Record>();

            public string Name { get; }
            public short Minimum { get; }
            public short Maximum { get; }
            public short Step { get; }
            public short Time { get; }
            public Record this[short shift] => _records[shift];

            public RecordContainer(string name, short min, short max, short step, short time)
            {
                Name = name;
                Minimum = min;
                Maximum = max;
                Step = step;
                Time = time;
            }

            public void Add(short shift, Record record)
            {
                _records.Add(shift, record);
            }

            public override string ToString() => Name;
        }

        private void Graph_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            var xScale = Math.Max(1,  Graph.Width / _liveViewData.Length);
            _plotDrawer.DrawArray(_liveViewData, e.Graphics, new ImageLayoutInfo(Graph.ClientSize.Width, Graph.ClientSize.Height), Common.Plots.PlotDrawer.PlotStyle.Default, xScale);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RecordList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var recordContainer =  RecordList.SelectedItem as RecordContainer;

            if (recordContainer == null)
                return;

            trackBar1.Minimum = recordContainer.Minimum;
            trackBar1.Maximum = recordContainer.Maximum;
            trackBar1.SmallChange = recordContainer.Step;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _recordView = tabControl1.SelectedTab.Text == "Просмотр";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            var recordContainer =  RecordList.SelectedItem as RecordContainer;

            if (recordContainer == null)
                return;

            var shift = (short)trackBar1.Value;
            var record = recordContainer[shift];

            _liveViewData = record.Data;
            _pictureBoxController.SetImage(record.PreviewRegion, true);
            Graph.Refresh();
        }
    }
}
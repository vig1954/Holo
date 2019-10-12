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
        private readonly PhaseShiftDeviceController _phaseShiftDeviceController;
        private readonly SegmentSelectionTool _segmentSelectionTool = new SegmentSelectionTool();
        private readonly SegmentSelectionForm _segmentSelectionForm = new SegmentSelectionForm();

        private readonly Common.Plots.PlotDrawer.PlotStyle _mainPlotStyle = new Common.Plots.PlotDrawer.PlotStyle
        {
            DrawPoints = true,
            PlotPen = new Pen(Color.DarkBlue, 2f),
            PointBrush = Brushes.DarkBlue,
            PointRadius = 3f
        };

        private readonly Common.Plots.PlotDrawer.PlotStyle _referencePlotStyle = new Common.Plots.PlotDrawer.PlotStyle
        {
            DrawPoints = true,
            PlotPen = new Pen(Color.CadetBlue, 2f),
            PointBrush = Brushes.CadetBlue,
            PointRadius = 3f
        };

        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();

        private PlotDrawer _plotDrawer;
        private PictureBoxController _pictureBoxController;
        private Segment _segment;
        private Rectangle _segmentViewRectangle;
        private Segment _previewSegment;
        private bool _calibrationInProgress;
        private Bitmap _currentPreviewRegion;
        private bool _recordView;
        private Record _currentRecord;
        private Record _referenceRecord;
        private float[] _liveViewData;
        private Sample _liveViewSample;
        private SampleEvaluation _liveViewSampleEvaluation;


        public PsdCalibrationForm(PhaseShiftDeviceController phaseShiftDeviceController)
        {
            InitializeComponent();

            _segmentSelectionForm.SegmentSelected += SegmentSelectionFormOnSegmentSelected;
            _pictureBoxController = new PictureBoxController(DataView);

            _phaseShiftDeviceController = phaseShiftDeviceController;

            _plotDrawer = new PlotDrawer();
            _segmentViewRectangle = new Rectangle(0, 0, DataView.Width, DataView.Height);

            _segmentSelectionTool.ImageLayoutInfo = new ImageLayoutInfo(DataView.Width, DataView.Height);

            RecordList.Items.Add("N/A");
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

            _segmentViewRectangle = new Rectangle((int) (centerX - regionSize / 2), (int) (centerY - regionSize / 2),
                (int) regionSize, (int) regionSize);
            // _segmentStartPoint = new Point((int) (padding + segment.X0), (int) (padding + segment.Y0));

            _previewSegment = new Segment
            {
                X0 = segment.X0 - _segmentViewRectangle.Left,
                X1 = segment.X1 - _segmentViewRectangle.Left,
                Y0 = segment.Y0 - _segmentViewRectangle.Top,
                Y1 = segment.Y1 - _segmentViewRectangle.Top,
            };

            _segmentSelectionTool.Segment = _previewSegment;
            _segmentSelectionTool.ImageLayoutInfo = new ImageLayoutInfo(DataView.Width, DataView.Height,
                _segmentViewRectangle.Width, _segmentViewRectangle.Height, 0, 0, 1);
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
            if (!_recordView && !_segmentViewRectangle.IsEmpty)
            {
                var image = SelectionUtil.ExtractSelection(bmp, _segmentViewRectangle);
                _currentPreviewRegion = image;
                _pictureBoxController.SetImage(image, true);

                _liveViewData = BitmapUtil.ExtractSegment(_currentPreviewRegion, _previewSegment);

                if (_liveViewSample == null || _liveViewSample.BufferSize != _liveViewData.Length)
                {
                    _liveViewSample = new Sample((short) Shift.Value, _liveViewData.Length, 10);
                    _liveViewSampleEvaluation = new SampleEvaluation(_liveViewSample);
                }

                _liveViewSample.AddSample(_liveViewData);

                if (Graph.InvokeRequired)
                    Graph.Invoke((Action) Graph.Refresh);
                else
                    Graph.Refresh();
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

            var recordContainer = new RecordContainer("Запись " + RecordList.Items.Count, (short) Minimum.Value,
                (short) Maximum.Value, (short) Step.Value, (short) Time.Value);
            RecordList.Items.Add(recordContainer);
            RecordList.SelectedItem = recordContainer;

            const int sampleCount = 20;
            const int sampleDelay = 100;
            int collectedSampleCount = 0;

            Shift.Value = Minimum.Value;
            while (_calibrationInProgress)
            {
                var shift = (short) Shift.Value;
                await _phaseShiftDeviceController.SetShiftAsync(shift, (int) (1000 * Time.Value));

                var data = BitmapUtil.ExtractSegment(_currentPreviewRegion, _previewSegment);
                var sample = new Sample(shift, data.Length, sampleCount);
                sample.AddSample(data);
                var record = new Record(_currentPreviewRegion, sample, shift);
                recordContainer.Add(shift, record);

                int retryCount = 0;
                for (collectedSampleCount = 0; collectedSampleCount < sampleCount; collectedSampleCount++)
                {
                    try
                    {
                        await Task.Delay(sampleDelay);
                        data = BitmapUtil.ExtractSegment(_currentPreviewRegion, _previewSegment);
                        sample.AddSample(data);
                    }
                    catch (Exception ex)
                    {
                        if (retryCount > sampleCount)
                            throw;

                        collectedSampleCount--;
                        retryCount++;
                    }
                }

                record.SampleEvaluation.Update();

                if (Shift.Value >= Maximum.Value)
                {
                    StopCalibration();
                    break;
                }

                Shift.Value += Step.Value;
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

        private void Graph_Paint(object sender, PaintEventArgs e)
        {
            if (_liveViewData == null || _liveViewData.Length == 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            _plotDrawer.ScaleY = Graph.ClientSize.Height;
            if (_recordView && _currentRecord != null)
            {
                var data = _currentRecord.SampleEvaluation.SampleAverage;

                var xScale = Math.Max(1, Graph.Width / data.Length);

                _plotDrawer.DrawArray(data, e.Graphics,
                    new ImageLayoutInfo(Graph.ClientSize.Width, Graph.ClientSize.Height),
                    _mainPlotStyle, xScale);

                if (_referenceRecord != null)
                {
                    _plotDrawer.DrawArray(_referenceRecord.SampleEvaluation.SampleAverage, e.Graphics,
                        new ImageLayoutInfo(Graph.ClientSize.Width, Graph.ClientSize.Height),
                        _mainPlotStyle, xScale);
                }
            }
            else
            {
                var data = _liveViewData;
                if (_liveViewSampleEvaluation != null && _liveViewSampleEvaluation.Sample.SampleCount > 0)
                {
                    _liveViewSampleEvaluation.Update();
                    data = _liveViewSampleEvaluation.SampleAverage;
                }

                var xScale = Math.Max(1, Graph.Width / data.Length);

                _plotDrawer.DrawArray(data, e.Graphics,
                    new ImageLayoutInfo(Graph.ClientSize.Width, Graph.ClientSize.Height),
                    Common.Plots.PlotDrawer.PlotStyle.Default, xScale);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void RecordList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var recordContainer = RecordList.SelectedItem as RecordContainer;

            if (recordContainer == null)
                return;

            trackBar1.Minimum = recordContainer.Minimum;
            trackBar1.Maximum = recordContainer.Maximum;
            trackBar1.TickFrequency = recordContainer.Step;
            trackBar1.LargeChange = recordContainer.Step;
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
            var record = GetSelectedRecord();

            if (record == null)
                return;

            _currentRecord = record;
            _pictureBoxController.SetImage(record.PreviewRegion, true);
            Graph.Refresh();

            RecordShiftLabel.Text = $"Значение сдвига: {record.Sample.PhaseShiftDeviceParameterValue}";
        }

        private Record GetSelectedRecord()
        {
            var recordContainer = RecordList.SelectedItem as RecordContainer;

            if (recordContainer == null)
                return null;

            var shift = (short) trackBar1.Value;
            return recordContainer[shift];
        }

        private void Step_ValueChanged(object sender, EventArgs e)
        {
        }

        private void Shift_ValueChanged(object sender, EventArgs e)
        {
            _phaseShiftDeviceController.SetShiftAsync((short) Shift.Value, 100);
        }

        private void SetReferenceButton_Click(object sender, EventArgs e)
        {
            _referenceRecord = GetSelectedRecord();
        }

        private class Record
        {
            public Bitmap PreviewRegion { get; }
            public Sample Sample { get; }
            private short Shift { get; }
            public SampleEvaluation SampleEvaluation { get; private set; }

            public Record(Bitmap previewRegion, Sample sample, short shift)
            {
                Sample = sample;
                PreviewRegion = previewRegion;
                Shift = shift;
                SampleEvaluation = new SampleEvaluation(Sample);
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

            public Record this[short shift]
            {
                get
                {
                    if (!_records.ContainsKey(shift))
                        return _records[(short) (Minimum + (shift / Step) * Step)];
                    return _records[shift];
                }
            }

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

        private void btnSelectData_Click(object sender, EventArgs e)
        {
            _segmentSelectionForm.Show();
        }

        private void DataView_Click(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Infrastructure;
using UserInterface;
using UserInterface.DataEditors;
using UserInterface.DataEditors.InterfaceBinding;

namespace Camera
{
    public partial class CameraInputViewForm : Form, IFloatArrayDataProvider
    {
        public enum PreviewOrientation
        {
            ShowLeft,
            ShowRight,
            ShowTop,
            ShowBottom
        }

        private bool _settingsLoaded;
        private PreviewOrientation _previewOrientation;
        private CameraInputView _cameraInputView;
        private IInterfaceController _interfaceController;
        private Bitmap _liveViewImage;
        private RectangleSelectionTool _rectangleSelectionTool;
        private SegmentSelectionTool _segmentSelectionTool;
        private PictureBoxController _previewController;
        private PhaseShiftDeviceView _phaseShiftDeviceController;

        private int FixedSize = 512;
        private bool IsSelectionSizeLocked;
        public float[] Data { get; private set; }

        public event Action<float[]> OnDataUpdated;

        public CameraInputViewForm()
        {
            InitializeComponent();
        }

        public void LockSelection()
        {
            ToggleRectangleSelectionTool.Enabled = false;
            LockSelectionSizeButton.Enabled = false;
            IncreaseSelectionSize.Enabled = false;
            DecreaseSelectionSize.Enabled = false;

            if (!_rectangleSelectionTool.FixedSize.HasValue)
            {
                IsSelectionSizeLocked = true;
                _rectangleSelectionTool.FixedSize = _rectangleSelectionTool.RectangleSize;
            }
        }

        public void UnlockSelection()
        {
            ToggleRectangleSelectionTool.Enabled = true;
            LockSelectionSizeButton.Enabled = true;
            IncreaseSelectionSize.Enabled = true;
            DecreaseSelectionSize.Enabled = true;

            if (IsSelectionSizeLocked)
            {
                _rectangleSelectionTool.FixedSize = null;
                IsSelectionSizeLocked = false;
            }
        }

        private void CameraInputViewForm_Load(object sender, EventArgs e)
        {
            LoadSettings();

            _previewController = new PictureBoxController(Preview);
            _phaseShiftDeviceController = new PhaseShiftDeviceView();

            _cameraInputView = new CameraInputView(_phaseShiftDeviceController);
            _cameraInputView.CaptureStarted += LockSelection;
            _cameraInputView.CaptureFinished += UnlockSelection;

            _interfaceController = new InterfaceController(ControlPanel, new PropertyListManager());

            _interfaceController.BindObjectToInterface(_cameraInputView);

            _cameraInputView.LiveViewImageUpdated += CameraInputViewOnLiveViewImageUpdated;

            _rectangleSelectionTool = new RectangleSelectionTool();
            _rectangleSelectionTool.OnRectangleUpdated += RectangleSelectionToolOnRectangleUpdated;

            _segmentSelectionTool = new SegmentSelectionTool {ImageLayoutInfo = _previewController.ImageLayout};
            _segmentSelectionTool.OnSegmentUpdated += SegmentSelectionToolOnSegmentUpdated;

            Preview.Paint += PreviewOnPaint;

            HandleLockSelectionSize();

            Preview.EnableDoubleBuffering(true);
        }

        private void SegmentSelectionToolOnSegmentUpdated(Segment obj)
        {
            Preview.Refresh();
        }

        private void PreviewOnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            _segmentSelectionTool.DrawUi(e.Graphics);

            if (ToggleRectangleSelectionTool.Checked)
                _rectangleSelectionTool.DrawUi(e.Graphics);
        }

        private void RectangleSelectionToolOnRectangleUpdated(Rectangle obj)
        {
            _cameraInputView.LiveViewSelection = _rectangleSelectionTool.Rectangle;
            UpdateSelectionSizeLabel();
        }

        private void CameraInputViewOnLiveViewImageUpdated()
        {
            _liveViewImage = _cameraInputView.LiveViewImage;

            if (!_segmentSelectionTool.Segment.IsZeroLength)
            {
                Data = BitmapUtil.ExtractSegment(_liveViewImage, _segmentSelectionTool.Segment);
                OnDataUpdated?.Invoke(Data);
            }

            _previewController.SetImage(_liveViewImage, ZoomFit.Checked);
        }

        private void UpdateSelectionSizeLabel()
        {
            var selection = _rectangleSelectionTool.Rectangle;
            SelectionSizeLabel.Text = $"{selection.X}, {selection.Y} {selection.Width} x {selection.Width}";
        }

        private void SetPreviewOrientation(PreviewOrientation orientation)
        {
            if (_previewOrientation == orientation && _settingsLoaded)
                return;

            _previewOrientation = orientation;

            Control panel1Control;
            Control panel2Control;

            DockTopButton.Checked = false;
            DockLeftButton.Checked = false;
            DockBottomButton.Checked = false;
            DockRightButton.Checked = false;

            if (orientation == PreviewOrientation.ShowTop)
            {
                splitContainer1.Orientation = Orientation.Horizontal;
                panel1Control = PreviewPanel;
                panel2Control = ControlPanel;
                DockTopButton.Checked = true;
            }
            else if (orientation == PreviewOrientation.ShowLeft)
            {
                splitContainer1.Orientation = Orientation.Vertical;
                panel1Control = PreviewPanel;
                panel2Control = ControlPanel;
                DockLeftButton.Checked = true;
            }
            else if (orientation == PreviewOrientation.ShowBottom)
            {
                splitContainer1.Orientation = Orientation.Horizontal;
                panel2Control = PreviewPanel;
                panel1Control = ControlPanel;
                DockBottomButton.Checked = true;
            }
            else
            {
                splitContainer1.Orientation = Orientation.Vertical;
                panel2Control = PreviewPanel;
                panel1Control = ControlPanel;
                DockRightButton.Checked = true;
            }

            splitContainer1.Panel1.Controls.Clear();
            splitContainer1.Panel1.Controls.Add(panel1Control);
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(panel2Control);
        }

        private void DockLeftButton_Click(object sender, EventArgs e)
        {
            SetPreviewOrientation(PreviewOrientation.ShowLeft);
        }

        private void DockTopButton_Click(object sender, EventArgs e)
        {
            SetPreviewOrientation(PreviewOrientation.ShowTop);
        }

        private void DockRightButton_Click(object sender, EventArgs e)
        {
            SetPreviewOrientation(PreviewOrientation.ShowRight);
        }

        private void DockBottomButton_Click(object sender, EventArgs e)
        {
            SetPreviewOrientation(PreviewOrientation.ShowBottom);
        }

        private void LoadSettings()
        {
            var settings = Properties.Settings.Default;

            if (settings.SettingsInitialized)
            {
                this.Location = settings.WindowAppearance.Location;
                this.Size = settings.WindowAppearance.Size;

                var orientation = (PreviewOrientation) settings.PreviewOrientation;
                SetPreviewOrientation(orientation);

                splitContainer1.SplitterDistance = settings.SplitContainerFirstPanelSize;

                _settingsLoaded = true;
                return;
            }

            _settingsLoaded = true;
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (!_settingsLoaded)
                return;

            var settings = Properties.Settings.Default;
            settings.WindowAppearance = new Rectangle(Location, Size);
            settings.SplitContainerFirstPanelSize = splitContainer1.SplitterDistance;
            settings.PreviewOrientation = (int) _previewOrientation;
            settings.SettingsInitialized = true;
            settings.Save();
        }

        private void CameraInputViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cameraInputView.Shutdown();
            _rectangleSelectionTool.Detach();

            SaveSettings();
        }

        private void ToggleSelectionTool_Click(object sender, EventArgs e)
        {
            if (ToggleRectangleSelectionTool.Checked)
            {
                ToggleSegmentSelection.Checked = false;

                _previewController.SetTool(_rectangleSelectionTool);
                _cameraInputView.LiveViewSelection = _rectangleSelectionTool.Rectangle;
                UpdateSelectionSizeLabel();
            }
            else
            {
                _previewController.SetTool(ToggleSegmentSelection.Checked ? _segmentSelectionTool : null);
                _cameraInputView.LiveViewSelection = null;
                SelectionSizeLabel.Text = "";
            }
        }

        private void HandleLockSelectionSize()
        {
            if (LockSelectionSizeButton.Checked)
            {
                LockSelectionSizeButton.Text = $"[{FixedSize} x {FixedSize}]";
                _rectangleSelectionTool.FixedSize = new Size(FixedSize, FixedSize);
            }
            else
            {
                LockSelectionSizeButton.Text = "";
                _rectangleSelectionTool.FixedSize = null;
            }
        }

        private void IncreaseSelectionSize_Click(object sender, EventArgs e)
        {
            FixedSize += 128;
            HandleLockSelectionSize();
        }

        private void DecreaseSelectionSize_Click(object sender, EventArgs e)
        {
            if (FixedSize <= 128)
                return;

            FixedSize -= 128;
            HandleLockSelectionSize();
        }

        private void LockSelectionSizeButton_Click(object sender, EventArgs e)
        {
            HandleLockSelectionSize();
        }

        private void ToggleSegmentSelection_Click(object sender, EventArgs e)
        {
            if (ToggleSegmentSelection.Checked)
            {
                _previewController.SetTool(_segmentSelectionTool);
            }
            else
            {
                _previewController.SetTool(ToggleRectangleSelectionTool.Checked ? _rectangleSelectionTool : null);
            }
        }

        private void ShowChart_Click(object sender, EventArgs e)
        {
            var plotView = new PlotView(this);
            plotView.Show();
        }

        private void TestWavefront_Click(object sender, EventArgs e)
        {
            var testImageForm = new TestImageForm();
            testImageForm.Show();
        }

        private void PhaseShiftDeviceCalibration_Click(object sender, EventArgs e)
        {
            var f = new PhaseShiftDeviceCalibrationForm(_phaseShiftDeviceController, this);
            f.Show();
        }
    }
}
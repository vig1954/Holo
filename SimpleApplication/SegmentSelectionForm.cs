using System;
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
using Common;
using Infrastructure;

namespace SimpleApplication
{
    public partial class SegmentSelectionForm : Form
    {
        private SegmentSelectionTool _segmentSelectionTool = new SegmentSelectionTool();
        private PictureBoxController _pictureBoxController;

        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();

        public event Action<Segment> SegmentSelected;

        public SegmentSelectionForm()
        {
            InitializeComponent();

            _pictureBoxController = new PictureBoxController(LiveView);
        }

        private void SegmentSelectionForm_Load(object sender, EventArgs e)
        {
            _segmentSelectionTool.ImageLayoutInfo = _pictureBoxController.ImageLayout;
            _segmentSelectionTool.OnSegmentUpdated += SegmentSelectionToolOnSegmentUpdated;

            _pictureBoxController.SetTool(_segmentSelectionTool);
        }

        private void SegmentSelectionToolOnSegmentUpdated(Segment obj)
        {
        }

        private void CameraConnectorOnLiveViewUpdated(Bitmap image)
        {
            _pictureBoxController.SetImage(image, true);
            _segmentSelectionTool.ImageLayoutInfo = _pictureBoxController.ImageLayout;
        }

        public new void Show()
        {
            CameraConnector.LiveViewUpdated += CameraConnectorOnLiveViewUpdated;

            base.Show();
        }

        private void SegmentSelectionForm_Shown(object sender, EventArgs e)
        {
            
        }

        private void SegmentSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();

                SegmentSelected?.Invoke(_segmentSelectionTool.Segment);
            }


            CameraConnector.LiveViewUpdated -= CameraConnectorOnLiveViewUpdated;
        }

        private void LiveView_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            _segmentSelectionTool.DrawUi(e.Graphics);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Common.Plots;

namespace Camera
{
    public partial class PlotView : Form
    {
        private ImageLayoutInfo _imageLayout;
        private PlotDrawer _plotDrawer;
        private IFloatArrayDataProvider _dataProvider;
        private float[] _data;

        public PlotView(IFloatArrayDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _plotDrawer = new PlotDrawer();

            InitializeComponent();

            UpdateImageLayout();

            _dataProvider.OnDataUpdated += DataProviderOnDataUpdated;
            _data = _dataProvider.Data;
        }

        private void DataProviderOnDataUpdated(float[] data)
        {
            _data = data;

            Canvas.Refresh();
        }

        private void UpdateImageLayout()
        {
            _plotDrawer.ScaleY = Canvas.ClientSize.Height;
            _imageLayout = new ImageLayoutInfo(Canvas.ClientSize.Width, Canvas.ClientSize.Height);
        }

        private void Canvas_Resize(object sender, EventArgs e)
        {
            UpdateImageLayout();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (_data == null)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.Clear(Color.White);

            _plotDrawer.DrawArray(_data, e.Graphics, _imageLayout, PlotDrawer.PlotStyle.Default);
        }
    }
}

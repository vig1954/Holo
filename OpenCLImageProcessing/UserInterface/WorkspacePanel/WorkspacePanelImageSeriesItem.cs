using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Processing;
using UserInterface.DataProcessorViews;
using UserInterface.WorkspacePanel.ImageSeries;

namespace UserInterface.WorkspacePanel
{
    public partial class WorkspacePanelImageSeriesItem : UserControl
    {
        private UserInterface.ImageSeries.ImageSeries _series;

        public UserInterface.ImageSeries.ImageSeries Series => _series;

        public WorkspacePanelImageSeriesItem(UserInterface.ImageSeries.ImageSeries series)
        {
            InitializeComponent();

            flowLayoutPanel1.Resize += FlowLayoutPanel1OnResize;

            SetSeries(series);
        }

        public void AddDataProcessor(IDataProcessorView dataProcessorView)
        {
            Series.AddDataProcessor(dataProcessorView);
        }

        private void SetSeries(UserInterface.ImageSeries.ImageSeries series)
        {
            _series = series;

            _series.OnDataProcessorAdded += SeriesOnDataProcessorAdded;

            flowLayoutPanel1.Controls.Clear();

            var inputImagesControl = new InputImages();
            inputImagesControl.SetInputImages(_series.Inputs);

            flowLayoutPanel1.Controls.Add(inputImagesControl);

            foreach (var dataProcessor in series.DataProcessors)
            {
                AddDataProcessorView(dataProcessor, skipResizing: true);
            }

            ResizeFlowLayoutPanel1Controls();
        }

        private void SeriesOnDataProcessorAdded(IDataProcessorView dataProcessor)
        {
            AddDataProcessorView(dataProcessor);
        }

        private void AddDataProcessorView(IDataProcessorView dataProcessorView, bool skipResizing = false)
        {
            var dataProcessorViewControl = new DataProcessorView(dataProcessorView);
            flowLayoutPanel1.Controls.Add(dataProcessorViewControl);

            if (skipResizing)
                ResizeFlowLayoutPanel1Controls();
        }

        private void FlowLayoutPanel1OnResize(object sender, EventArgs e)
        {
            ResizeFlowLayoutPanel1Controls();
        }

        private void ResizeFlowLayoutPanel1Controls()
        {
            for (var i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                var control = flowLayoutPanel1.Controls[i];
                control.Width = flowLayoutPanel1.ClientSize.Width - control.Margin.Left - control.Margin.Right - flowLayoutPanel1.Padding.Left - flowLayoutPanel1.Padding.Right;
            }
        }
    }
}
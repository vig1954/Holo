using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Processing;

namespace UserInterface.WorkspacePanel.ImageSeries
{
    public partial class InputImages : UserControl
    {
        public InputImages()
        {
            InitializeComponent();
        }

        public void SetInputImages(IEnumerable<IImageHandler> imageHandlers)
        {
            var inputImageControls = imageHandlers.Select(i =>
            {
                var inputImageControl = new InputImage();
                inputImageControl.SetImageHandler(i);

                return inputImageControl;
            }).Cast<Control>().ToArray();

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.AddRange(inputImageControls);
        }
    }
}

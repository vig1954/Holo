using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rab1
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();

            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        }

        public void SetImage(Image image)
        {
            this.imageBox.Image = image;
            this.imageBox.Invalidate();
        }
    }
}

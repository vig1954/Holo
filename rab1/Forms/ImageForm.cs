﻿using System;
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
        private CustomPictureBox imageBox = null;

        public ImageForm()
        {
            InitializeComponent();
            this.imageBox = new CustomPictureBox();

            this.imageBox.Dock = DockStyle.Fill;
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            this.Controls.Add(this.imageBox);
        }

        public void SetImage(Image image, float offsetX)
        {
            this.imageBox.Invoke
            (
                (MethodInvoker)delegate 
                {
                    this.imageBox.OffsetX = offsetX;
                    this.imageBox.Image = image;
                    this.imageBox.Refresh();
                }
            );
        }

        public void SetImage(Image image)
        {
            float offsetX = 0;
            this.SetImage(image, offsetX);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace Camera
{
    public abstract class PictureBoxToolBase
    {
        protected PictureBox PictureBox;
        protected Action BeforeDrawUi;

        public ImageLayoutInfo ImageLayoutInfo { get; set; }

        public void Attach(PictureBox pictureBox)
        {
            PictureBox = pictureBox;
            PictureBox.MouseDown += PictureBoxOnMouseDown;
            PictureBox.MouseUp += PictureBoxOnMouseUp;
            PictureBox.MouseMove += PictureBoxOnMouseMove;

            AttachInner(pictureBox);
        }

        public void Detach()
        {
            if (PictureBox == null)
                return;

            PictureBox.MouseDown -= PictureBoxOnMouseDown;
            PictureBox.MouseUp -= PictureBoxOnMouseUp;
            PictureBox.MouseMove -= PictureBoxOnMouseMove;

            DetachInner();

            PictureBox = null;
        }

        protected virtual void PictureBoxOnMouseMove(object sender, MouseEventArgs e)
        {
        }

        protected virtual void PictureBoxOnMouseUp(object sender, MouseEventArgs e)
        {
        }

        protected virtual void PictureBoxOnMouseDown(object sender, MouseEventArgs e)
        {
        }

        protected virtual void AttachInner(PictureBox pictureBox)
        {
        }

        protected virtual void DetachInner()
        {
        }
    }
}
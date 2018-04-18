using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace rab1
{
    class CustomPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe) 
        {
            pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.None;
            base.OnPaint(pe);
        }
    }
}

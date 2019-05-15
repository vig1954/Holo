using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;

namespace Camera
{
    public partial class TestImageForm : Form
    {
        const float Lambda = 0.000532f; // мм

        public TestImageForm()
        {
            InitializeComponent();
        }

        private unsafe Bitmap GenerateWavefront(float distance, float phase, float sizeX, float sizeY, float angleX, float angleY, float noise)
        {
            var w = pictureBox1.ClientSize.Width;
            var h = pictureBox1.ClientSize.Height;
            var deltaX = sizeX / w;
            var deltaY = sizeY / h;
            var k = (float) Math.PI / (Lambda * distance);
            var adx = (float) Math.Tan(angleX) * distance;
            var ady = (float) Math.Tan(angleY) * distance;

            var bitmap = new Bitmap(w, h);
            var bdata = bitmap.LockBits(bitmap.GetRectangle(), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var p = (BitmapUtil.ARGBPoint*) bdata.Scan0;
            var rnd = new Random();

            int x, y, i;
            float kx, ky, dxy;
            var hw = w / 2f;
            var hh = h / 2f;
            float val;
            byte bval;
            var pt = new BitmapUtil.ARGBPoint(0, 0, 0, 255);

            for (y = 0; y < h; y++)
            {
                for (x = 0; x < w; x++)
                {
                    i = x + y * w;
                    kx = deltaX * (hw + adx - x);
                    ky = deltaY * (hh + ady - y);
                    dxy = kx * kx + ky * ky;

                    val = (float) ((0.5 + Math.Sin(k * dxy + phase) * 0.5) + rnd.NextDouble() * noise);
                    bval = (byte) Math.Min(val * 255, 255);

                    pt.R = bval;
                    pt.G = bval;
                    pt.B = bval;

                    p[i] = pt;
                }
            }

            bitmap.UnlockBits(bdata);

            return bitmap;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void TestImageForm_Load(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void ShiftValue_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void NoiseValue_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private float GetShift()
        {
            return (float) ShiftValue.Value / 10f * (float)Math.PI;
        }

        private void UpdateImage()
        {
            pictureBox1.Image = GenerateWavefront( 200, GetShift(), 5, 5, 0, 0, (float)NoiseValue.Value * 0.01f);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * Самой же функцией является ограничение интерполяционного полинома 
 * Лагранжа на интервале [0,255] минимальным значением 0 и 
 * максимальным 255, полином вычисляется по задающим точкам 
 * (минимум две – левая и правая границы).
 */
namespace rab1
{
    public partial class CurvesGraph : Form
    {
        private VisualPolynomial //transmissionBright,
                                            transmissionR,
                                            transmissionG,
                                            transmissionB,
                                            currentTrans;
        private int[] transmR,
                          transmG,
                          transmB;
        private double prevPoint = double.NaN;
        private Pain_t_Core headquarters;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            var y = this.pictureBox1.Image.Height - e.Y;
            var x = e.X;
            this.prevPoint = this.currentTrans.IsTherePointNearly(x, y, 2d, 2d);
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            var y = this.pictureBox1.Image.Height - e.Y;
            var x = e.X;
            var pointNearClick = this.currentTrans.IsTherePointNearly(x, y, 5d, 30d);
            if (pointNearClick is double.NaN)
            {
                this.currentTrans.AddPoint(x, y);
                this.RefreshData();
            }
            this.pictureBox1.Invalidate();
            return;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var y = this.pictureBox1.Image.Height - e.Y;
            var x = e.X;
            this.labelFrom.Text = Math.Round(x * 256d / this.pictureBox1.Width).ToString();
            this.labelTo.Text = Math.Round(y * 256d / this.pictureBox1.Height - 1d).ToString();
            if (this.prevPoint is double.NaN || this.IsNotAtPictureBox(e.X, e.Y))
                return;
            this.prevPoint = this.currentTrans.ReplacePoint(this.prevPoint, x, y);
            //this.prevPoint = this.currentTrans.IsTherePointNearly(x, y, 3d, 3d);
            this.pictureBox1.Invalidate();
            this.RefreshData();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)  
                return;
            this.prevPoint = double.NaN;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.prevPoint = double.NaN;
        }

        private bool IsNotAtPictureBox(int x, int y)
        {
            return x < 0 || y < 0 || x >= this.pictureBox1.Width || y >= this.pictureBox1.Height;
        }

        private void RefreshData()
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        this.transmR = this.transmissionR.GetIntArr();
                        break;
                    }
                case 1:
                    {
                        this.transmG = this.transmissionG.GetIntArr();
                        break;
                    }
                case 2:
                    {
                        this.transmB = this.transmissionB.GetIntArr();
                        break;
                    }
                default:
                    {
                        this.transmR = this.transmissionR.GetIntArr();
                        break;
                    }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.headquarters.Draw();
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.Enabled = false;
            this.headquarters.ApplyTransRGB(this.transmR, this.transmG, this.transmB);
            this.Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.headquarters.TryOnTransRGB(this.transmR, this.transmG, this.transmB);
        }

        private void CurvesGraph_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer.Enabled = false;
            this.headquarters.Draw();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = this.comboBox1.SelectedIndex;
            switch (index)
            {
                /*
                case 0:
                    {
                        this.currentTrans = this.transmissionBright;
                        break;
                    }
                    */
                case 0:
                    {
                        this.currentTrans = this.transmissionR;
                        break;
                    }
                case 1:
                    {
                        this.currentTrans = this.transmissionG;
                        break;
                    }
                case 2:
                    {
                        this.currentTrans = this.transmissionB;
                        break;
                    }
                default:
                    {
                        this.currentTrans = this.transmissionR;
                        break;
                    }
            }
            this.currentTrans.SetActive();
            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Right != e.Button) 
                return;
            var y = this.pictureBox1.Image.Height - e.Y;
            var x = e.X;
            var pointNearClick = this.currentTrans.IsTherePointNearly(x, y, 2d, 2d);
            if (!(pointNearClick is double.NaN))
            {
                this.currentTrans.DeletePoint(pointNearClick);
                this.RefreshData();
            }
            this.pictureBox1.Invalidate();
            return;
        }

        private void CurvesGraph_Load(object sender, EventArgs e)
        {
            var bmp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            this.pictureBox1.Image = bmp;
            //this.transmissionBright = new VisualPolynomial(0, 255, 0, 255, 256, 0d, 255d, bmp);
            this.transmissionR = new VisualPolynomial(0, 255, 0, 255, 256, 0d, 255d, bmp);
            this.transmissionG = new VisualPolynomial(0, 255, 0, 255, 256, 0d, 255d, bmp);
            this.transmissionB = new VisualPolynomial(0, 255, 0, 255, 256, 0d, 255d, bmp);
            this.currentTrans = this.transmissionR;
            this.transmissionR.SetActive();
            this.transmR = this.transmissionR.GetIntArr();
            this.transmG = this.transmissionG.GetIntArr();
            this.transmB = this.transmissionB.GetIntArr();
            this.comboBox1.SelectedIndex = 0;
            this.timer.Enabled = true;
        }

        public CurvesGraph(Pain_t_Core core)
        {
            this.headquarters = core;
            InitializeComponent();
        }
    }
}

﻿using System;
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
        private VisualPolynomial transmission, currentTrans;
        private int[] transm;
        private double prevPoint = double.NaN;

        public event EventHandler ApplyCurve;
        public event EventHandler ApplyCurveAll;
        public event EventHandler ApplyPhaseDifferenceCalculation;

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
            this.transm = this.transmission.GetIntArr();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (this.ApplyCurve != null)
            {
                this.ApplyCurve(this, new EventArgs());
            }
        }   

        private void CurvesGraph_FormClosing(object sender, FormClosingEventArgs e)
        {
            
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
            this.transmission = new VisualPolynomial(0, 255, 0, 255, 256, 0d, 255d, bmp);
            this.currentTrans = this.transmission;
            this.transmission.SetActive();
            this.transm = this.transmission.GetIntArr();
        }

        public CurvesGraph()
        {
            InitializeComponent();
        }

        private void btnApplyAll_Click(object sender, EventArgs e)
        {
            if (this.ApplyCurveAll != null)
            {
                this.ApplyCurveAll(this, new EventArgs());
            }
        }

        public int[] GetRecodingArray()
        {
            return this.transm;
        }

        public int GetStartImageNumber()
        {
            return int.Parse(txtStartImageNumber.Text);
        }

        private void btnApplyPhaseDifferenceCalculation_Click(object sender, EventArgs e)
        {
            if (ApplyPhaseDifferenceCalculation != null)
            {
                ApplyPhaseDifferenceCalculation(this, new EventArgs());
            }
        }

        public int GetEndImageNumber()
        {
            return int.Parse(txtEndImageNumber.Text);
        }

        public double[] GetPhaseShifts()
        {
            int phaseShiftCount = 4;

            double[] phaseShiftArray = new double[phaseShiftCount];

            phaseShiftArray[0] = Convert.ToDouble(txtPhaseShift1.Text);
            phaseShiftArray[1] = Convert.ToDouble(txtPhaseShift2.Text);
            phaseShiftArray[2] = Convert.ToDouble(txtPhaseShift3.Text);
            phaseShiftArray[3] = Convert.ToDouble(txtPhaseShift4.Text);

            for (int i = 0; i < phaseShiftCount; i++) phaseShiftArray[i] = Math.PI * phaseShiftArray[i] / 180.0;

            return phaseShiftArray;
        }
    }
}

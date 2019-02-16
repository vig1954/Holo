﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rab1.Forms
{
    
    public partial class Lissagu : Form
    {
        private static double[] fz = { 0.0, 90.0, 180.0, 270.0, 0.0, 90.0, 180.0, 270.0 };
        private static int k1 = 1;   // Флаг для вывода 2 графика
        private static int k2 = 1;
        private static int k3 = 1;
        private static int k4 = 1;
        private static int n = 256;     // Номер строки
        private static int xx0 = 0;     // Начальное значение в графике

        private static double max = double.MinValue;  // Минимальное значение для 4 строк
        private static double min = double.MaxValue;

        private static  double[] buf1;    // массивы для 4 строк
        private static  double[] buf2;
        private static  double[] buf3;
        private static  double[] buf4;

        public Lissagu()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(n);
            textBox2.Text = Convert.ToString(fz[0]);
            textBox3.Text = Convert.ToString(fz[1]);
            textBox4.Text = Convert.ToString(fz[2]);
            textBox5.Text = Convert.ToString(fz[3]);
        }

        private void button1_Click(object sender, EventArgs e)                                // Построить фигуру Лиссажу по кадру
        {
            // MessageBox.Show("RegComplex = " + Form1.regComplex ); // 0, 1, 2
            if (Form1.zArrayDescriptor[Form1.regComplex*4] == null) { MessageBox.Show("Lissagu zArrayDescriptor == NULL"); return; }

            double[] fzrad = new double[4];                                                   // Фаза в радианах
            fz[0] = Convert.ToDouble(textBox2.Text); fzrad[0] = Math.PI * fz[0] / 180.0;
            fz[1] = Convert.ToDouble(textBox3.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox4.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox5.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            int w1 = Form1.zArrayDescriptor[Form1.regComplex].width;
            int h1 = Form1.zArrayDescriptor[Form1.regComplex].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            faza = FazaClass.ATAN_Gr(Form1.zArrayDescriptor, fzrad, Form1.regComplex);
            Vizual.Vizual_Picture(faza, pictureBox2);
        }

        private void button2_Click(object sender, EventArgs e)                                  // Построить фигуру Лиссажу по строке
        {
            if (Form1.zArrayDescriptor[Form1.regComplex * 4] == null) { MessageBox.Show("Lissagu zArrayDescriptor == NULL"); return; }
            double[] fzrad = new double[4];                                                   // Фаза в радианах

            fz[0] = Convert.ToDouble(textBox2.Text); fzrad[0] = Math.PI * fz[0] / 180.0;
            fz[1] = Convert.ToDouble(textBox3.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox4.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox5.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            int N_line = Convert.ToInt32(textBox1.Text);

            int w1 = Form1.zArrayDescriptor[Form1.regComplex].width;
            int h1 = Form1.zArrayDescriptor[Form1.regComplex].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            faza = FazaClass.ATAN_Gr_N(Form1.zArrayDescriptor, fzrad, Form1.regComplex, N_line);
            Vizual.Vizual_Picture(faza, pictureBox2);
        }



        // --------------------------------------------------------------------------------------------------------------------------------------------
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            if (checkBox.Checked == true) { k1 = 1; } else { k1 = 0; }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            if (checkBox.Checked == true) { k2 = 1; } else { k2 = 0; }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            if (checkBox.Checked == true) { k3 = 1; } else { k3 = 0; }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            if (checkBox.Checked == true) { k4 = 1; } else { k4 = 0; }
        }
        private void button3_Click(object sender, EventArgs e)   // Построить график
        {
            for (int i = 0; i < 4; i++)
                     if (Form1.zArrayDescriptor[Form1.regComplex * 4 + i] == null) { MessageBox.Show("Lissagu zArrayDescriptor == NULL"); return; }
            int w1 = Form1.zArrayDescriptor[Form1.regComplex * 4].width;
            int h1 = Form1.zArrayDescriptor[Form1.regComplex * 4].height;
            int N_line = Convert.ToInt32(textBox1.Text);

            buf1 = new double[w1];
            buf2 = new double[w1];
            buf3 = new double[w1];
            buf4 = new double[w1];

            for (int j = 0; j < w1; j++) buf1[j] = Form1.zArrayDescriptor[Form1.regComplex * 4].array[j, N_line];
            for (int j = 0; j < w1; j++) buf2[j] = Form1.zArrayDescriptor[Form1.regComplex * 4 + 1].array[j, N_line];
            for (int j = 0; j < w1; j++) buf3[j] = Form1.zArrayDescriptor[Form1.regComplex * 4 + 2].array[j, N_line];
            for (int j = 0; j < w1; j++) buf4[j] = Form1.zArrayDescriptor[Form1.regComplex * 4 + 3].array[j, N_line];

           
            for (int j = 0; j < w1; j++)
            {
                if (buf1[j] > max) max = buf1[j];  if (buf1[j] < min) min = buf1[j];
                if (buf2[j] > max) max = buf2[j];  if (buf2[j] < min) min = buf2[j];
                if (buf3[j] > max) max = buf3[j];  if (buf3[j] < min) min = buf3[j];
                if (buf4[j] > max) max = buf4[j];  if (buf4[j] < min) min = buf4[j];
            }

            for (int j = 0; j < w1; j++)
               {
                  buf1[j] = (buf1[j] - min) * 255/  (max-min);
                  buf2[j] = (buf2[j] - min) * 255 / (max - min);
                  buf3[j] = (buf3[j] - min) * 255 / (max - min);
                  buf4[j] = (buf4[j] - min) * 255 / (max - min);
               }
            //MessageBox.Show("n - "+ buf1.Length);
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = w1;
            xx0 = 0;
            Gr(xx0);
        }


        private void Gr(int x)
        {

            int w1 = pictureBox3.Width;
            int h1 = pictureBox3.Height;

            label2.Text = x.ToString();
            label3.Text = (x + w1).ToString();

            Bitmap btm = new Bitmap(w1, h1);
            Graphics gr_Image = Graphics.FromImage(btm);
            pictureBox3.Image = btm;


            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(Color.Red, 1);      // Red
            Pen p3 = new Pen(Color.Green, 1);
            Pen p4 = new Pen(Color.Purple, 1);

            
            Font drawFont = new Font("Courier", 8, FontStyle.Regular, GraphicsUnit.Point);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            int x0 = 28;
            int y0 = 10;
            //  -----------------------------------------------------------------------------------------------------Ось x
            gr_Image.DrawLine(p1, x0, h1 - y0, w1, h1 - y0);
            for (int i = 0; i < w1-8; i += 8) gr_Image.DrawLine(p1, i + x0, h1 - y0, i + x0, h1 - y0 + 5);
            //  -----------------------------------------------------------------------------------------------------Ось y
            gr_Image.DrawLine(p1, x0, h1 - y0, x0, 8);
            for (int i = y0; i < h1 - 8; i += 8) gr_Image.DrawLine(p1, x0, i, x0 - 5, i);
            //  -----------------------------------------------------------------------------------------------------вывод max и min
            string sx = min.ToString("###");       gr_Image.DrawString(sx, drawFont, drawBrush, 0, h1 - 20 ); //, drawFormat);
             sx = max.ToString("###");             gr_Image.DrawString(sx, drawFont, drawBrush, 0, h1 - 255); //, drawFormat);


            if (k1 == 1)                                                        // Флаг включен
            for (int i = 0; i < w1 - 8 ; i++)                                   // Вывод графика
            {
                int i1 = i + x;
                int i2 = i + 1 + x;
                if (i1 >= buf1.Length-1 || i2 >= buf1.Length-1) break;
                int y1 = (int)(h1 - buf1[i1]     - y0);
                int y2 = (int)(h1 - buf1[i2] - y0);
                gr_Image.DrawLine(p1, i + x0, y1, i + 1 + x0, y2);
            }
            if (k2 == 1)
                for (int i = 0; i < w1 - 8; i++)
            {
                    int i1 = i + x;
                    int i2 = i + 1 + x;
                    if (i1 >= buf2.Length-1 || i2 >= buf2.Length-1) break;
                    int y1 = (int)(h1 - buf2[i1] - y0);
                    int y2 = (int)(h1 - buf2[i2] - y0);
                    gr_Image.DrawLine(p2, i + x0, y1, i + 1 + x0, y2);
                }
            if (k3 == 1)
                for (int i = 0; i < w1 - 8; i++)
            {
                    int i1 = i + x;
                    int i2 = i + 1 + x;
                    if (i1 >= buf3.Length-1 || i2 >= buf3.Length-1) break;
                    int y1 = (int)(h1 - buf3[i1] - y0);
                    int y2 = (int)(h1 - buf3[i2] - y0);
                    gr_Image.DrawLine(p3, i + x0, y1, i + 1 + x0, y2);
                }
            if (k4 == 1)
                for (int i = 0; i < w1 - 8; i++)
            {
                    int i1 = i + x;
                    int i2 = i + 1 + x;
                    if (i1 >= buf4.Length-1 || i2 >= buf4.Length-1) break;
                    int y1 = (int)(h1 - buf4[i1] - y0);
                    int y2 = (int)(h1 - buf4[i2] - y0);
                    gr_Image.DrawLine(p4, i + x0, y1, i + 1 + x0, y2);
                }
            //pictureBox3.Refresh();
            pictureBox3.Invalidate();

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int ixx = hScrollBar1.Value;
            Gr(ixx);
        }
    }
}

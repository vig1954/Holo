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
    public partial class Graph3D : Form
    {

        double[,] arr = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
        ZArrayDescriptor zArray2D       = new ZArrayDescriptor();
        ZArrayDescriptor zArrayPicture  = new ZArrayDescriptor();
        int nx, ny, nx1, ny1;
        double sx=1, dx=0, dy=0;
        int ns = 6;  // Число сечений
        public Graph3D()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(ns);
        }
        public Graph3D(ZArrayDescriptor zArrayPicture)
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(ns);
            zArray2D      = new ZArrayDescriptor(picture3D.Width, picture3D.Height); // Массив для изображения
            Vizual_2D(zArrayPicture);
            Vizual.Vizual_Picture(zArray2D, picture3D);
        }
        private void Graph3D_Load(object sender, EventArgs e)
        {

        }
        private void Vizual_2D(ZArrayDescriptor zArrayPicture1)
        {
            if (zArrayPicture == null) { MessageBox.Show("Graph3D zArrayPicture == null"); return; }
            zArrayPicture = zArrayPicture1;

            nx = zArrayPicture.width;
            ny = zArrayPicture.height;

            if (nx == 0 || ny == 0) { MessageBox.Show("Graph3D nx, ny == null"); return; }

            nx1 = picture3D.Width;
            ny1 = picture3D.Height;

            sx  = (double)nx1 / (double)nx;
            double sy  = (double)ny1 / (double)ny;
            sx = Math.Min(sx, sy);
            if (sx > 1) sx = 1;
            sx = sx * 0.7;

            dx = 0; dy=0;

            if (nx1 > nx * sx) dx = (double)(nx1 - nx * sx) / 2;
            if (ny1 > ny * sx) dy = (double)(ny1 - ny * sx) / 2;
            //dx = nx1 / 2;
            //dy = ny1 / 2;


            //MessageBox.Show("sx = " + sx + "sy = " + sy );
            //MessageBox.Show("nx = " + nx*sx + "ny = " + ny*sy + "r = " + Math.Sqrt(nx * sx* nx * sx + ny * sy * ny * sx));

            //ZArrayDescriptor zArray2D = new ZArrayDescriptor(nx1, ny1);

            //arr = Sdv(arr, dx, dy);
            //arr = Scale(arr, sx, sy);   // Обобщенная матрица поворота
            arr = Sdv(arr, dx, dy);

            Refresh3D();
        }

        private double[,] MulArr(double[,]a , double[,] b)
        {
            double[,] c = new double[3, 3];
            for (int i = 0; i < 3; i++)
             for (int j = 0; j < 3; j++)
               for (int k = 0; k < 3; k++)
                 {
                    c[i, j] += a[i, k] * b[k, j];
                 }
            return c;
        }

        private double[,] Ini3D()
        {
            double[,] c = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            return c;
        }
        private double[,] Scale(double[,] a, double sx, double sy)
        {
            double[,] sc = new double[,] { { sx, 0, 0 }, { 0, sy, 0 }, { 0, 0, 1 } };
            double[,] c = new double[3, 3];
            c = MulArr(a, sc);
            return c;
        }

       

        private double[,] Sdv(double[,] a, double dx, double dy)
        {
            double[,] sc = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { dx, dy, 1 } };
            double[,] c = new double[3, 3];
            c = MulArr(a, sc);
            return c;
        }
        private double[,] Rot(double[,] a, double fi)
        {
            double sn = Math.Sin(fi);
            double cn = Math.Cos(fi);
            double[,] sc = new double[,] { { cn, sn, 0 }, { -sn, cn, 0 }, { 0, 0, 1 } };
            double[,] c = new double[3, 3];
            c = MulArr(a, sc);
            return c;
        }
        // Обработка ползунка поворота
        private void trackBar1_Scroll(object sender, EventArgs e)     // Поворот изображения
        {
            double fi = trackBar1.Value * Math.PI / 180;
            label1.Text = String.Format("Поворот: {0} градусов", trackBar1.Value);

            arr = Ini3D();
            arr = Scale(arr, sx, sx);
            arr = Sdv(arr, dx, dy);
            double dx1 = nx1/2;
            double dy1 = ny1/2;
            arr = Sdv(arr, -dx1, -dy1);
            arr = Rot(arr, fi);
            arr = Sdv(arr, dx1, dy1);

            Refresh3D();
            Vizual.Vizual_Picture(zArray2D, picture3D);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)   // Поворот графика
        {
            double fi = trackBar2.Value * Math.PI / 180;
            label2.Text = String.Format("Поворот графика: {0} градусов", trackBar1.Value);

            arr = Ini3D();
            arr = Scale(arr, sx, sx);
            arr = Sdv(arr, dx, dy);
            double dx1 = nx1 / 2;
            double dy1 = ny1 / 2;
            arr = Sdv(arr, -dx1, -dy1);
            arr = Rot(arr, fi);
            arr = Sdv(arr, dx1, dy1);

            Refresh3D();
            Gr3D();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)   // Поворот графика
        {
            double fi = trackBar3.Value;
           
            arr = Ini3D();
            arr = Scale(arr, sx, sx);
            arr = Sdv(arr, dx, dy);
            sx = trackBar3.Value / 50.0 + 1;  // от 0 до 2
           
            label3.Text = "Масштаб: " + sx.ToString();

            double dx1 = nx1 / 2;
            double dy1 = ny1 / 2;
            arr = Sdv(arr, -dx1, -dy1);
            arr = Scale(arr, sx, sx); ;
            arr = Sdv(arr, dx1, dy1);

            Refresh3D();
            Vizual.Vizual_Picture(zArray2D, picture3D);
        }

        private void Refresh3D()
        {
            for (int i = 0; i < nx1; i++)
              for (int j = 0; j < ny1; j++)
                { zArray2D.array[i, j] = 0; } 


            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    int x = (int)(i * arr[0, 0] + j * arr[1, 0] + arr[2, 0]);
                    int y = (int)(i * arr[0, 1] + j * arr[1, 1] + arr[2, 1]);
                    if (x < nx1 && y < ny1 && x > 0 && y > 0) zArray2D.array[x, y] = zArrayPicture.array[i, j];
                }

            //Vizual.Vizual_Picture(zArray2D, picture3D);

        }
        // Перерисовать button
        private void button1_Click(object sender, EventArgs e)
        {
            //arr = Ini3D();
            //arr = Scale(arr, sx, sx);
            //arr = Sdv(arr, dx, dy);
            Refresh3D();
            Vizual.Vizual_Picture(zArray2D, picture3D);
        }
        // ----------------------------------------------------------------------------------------------------
        //              График 3D
        // ----------------------------------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            Gr3D();
        }

        private void Gr3D()
        {
            ns = Convert.ToInt32(textBox1.Text);
            ZArrayDescriptor zArray3D = new ZArrayDescriptor(nx1, ny1);

            int[] arr_max    = new int[nx1];
            int[] arr_tmp    = new int[nx1];
            int[] arr_tmp_gr = new int[nx1];

            //for (int i = 0; i < nx1; i++)
            //{
            //    arr_max[i] = 0;
                //arr_tmp[i] = -1000;
           // }

            
            int ys=2;
            
            int y0 = 250;  //  Сдвиг снизу для красоты
            //int ns1 = ns;

           double max = SumClass.getMax(zArray2D);
           double min = SumClass.getMin(zArray2D);

            int k = 0;
            for (int j = 0; j < ny1; j += ns, k++)
            {
                for (int i = 0; i < nx1; i++)
                {
                    int ix = i - k;
                    if (ix < nx1 && ix>0) arr_tmp[i] = (int)((zArray2D.array[ix, j] - min) * 128 / (max - min)) + ys * k;
                }

                for (int i = 0; i < nx1; i++)
                {
                    if (arr_tmp[i] > arr_max[i]) arr_max[i] = arr_tmp[i];
                }

                int x1 = 0;
                int y1 = (int)(arr_max[0]);          
                for (int i = 1; i < nx1-100+k; i++)
                {
                    if (i >= nx1) break;
                    int x2 = i;
                    int y2 = (int)(arr_max[i]);
                    lineDDA(x1, y1, x2, y2, y0, zArray3D);
                    y1 = y2;
                    x1 = x2;
                }
           
          }
            Vizual.Vizual_Picture(zArray3D, picture3D);
        }

        
        private void lineDDA(int x1, int y1, int x2, int y2,  int y0, ZArrayDescriptor zArray3D)
                {
                    double dx, dy, steps;
                            dx = x2 - x1;
                            dy = y2 - y1;
                            if (dy < 0) { y1 = y2; dy = -dy; }
                            if (dx > dy) steps = Math.Abs(dx); else steps = Math.Abs(dy);
                            double Xinc = dx / (double)steps;
                            double Yinc = dy / (double)steps;
                            double xx = x1 + 0.5 * Math.Sign(dx),
                                   yy = y1 + 0.5 * Math.Sign(dy);

                            for (int ii = 0; ii < steps; ii++)
                            {
                                xx = (int)(xx + Xinc);
                                yy = (int)(yy + Yinc);
                                Put1((int)xx, (int)yy, y0, zArray3D);
                            }
                   
                }
         private void Put1(int x, int y, int y0, ZArrayDescriptor zArray3D)
         {
            int nx1 = zArray3D.width;
            int ny1 = zArray3D.width;
            int xi = x;
            int yi = nx1 - y - y0;        
            if (xi < nx1 && yi < ny1 && xi > 0 && yi > 0) zArray3D.array[xi, yi] = 255;

        }

    }
}

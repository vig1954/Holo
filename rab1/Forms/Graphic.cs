﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using OpenTK.Graphics.ES10;

namespace rab1.Forms
{
    public partial class Graphic : Form
    {
        private double[] buf_gl;           // Масштабированные значений
        private double[] buf1_gl;          // Истинные значения
        private int w;                     // Размер массива
        private int x0 = 70;
        int hh = 256;                     //  Размер по оси Y
        int h = 256 + 20;                 //  hh -размер 256 , рисуем немного ниже
        int ixx=0;

        double maxx = double.MinValue;
        double minx = double.MaxValue;
        Graphics grBack;

        
       
        static int k = 0;  // Для рисования зеленой линии, двигающейся по y
        static int xc = 0; // 
        static int yc = 0; // 


        //  w1 - размер массива
        //  ix - с какого значения выводить
        //
        public Graphic(int w1,  double[] buf)
        {
            InitializeComponent();
            w = w1;

            hScrollBar2.Minimum = 0;                                 //    hScrollBar2
            hScrollBar2.Maximum = w1;
            label6.Text = hScrollBar2.Minimum.ToString(); ;
            label7.Text = hScrollBar2.Maximum.ToString(); ;
            label8.Text = hScrollBar2.Value.ToString();


      
            pc1.BackColor = Color.White;                              // PictureBox pc1 - белый фон
            pc1.Location = new System.Drawing.Point(0, 8);
            //pc1.Size = new Size(w1 + 86, hh + 64);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;
            
            //Bitmap btmBack = new Bitmap(w1 + 86, hh + 64);           //изображение   
            Bitmap btmBack = new Bitmap(pc1.Width, hh + 64);           //изображение  
            grBack = Graphics.FromImage(btmBack);       
            pc1.BackgroundImage = btmBack;

            buf_gl  = new double[w1];                                // Масштабированные значений
            buf1_gl = new double[w1];                                // Истинные значения
            for (int i = 0; i < w1; i++) { double b = buf[i]; buf1_gl[i] = b; if (b < minx) minx = b; if (b > maxx) maxx = b; buf1_gl[i] = b; }
            if (maxx == minx) { MessageBox.Show("max == min = " + Convert.ToString(maxx)); return; }
            for (int i = 0; i < w1; i++) { buf_gl[i] = (buf[i] - minx) * hh / (maxx - minx); }
            ixx = 0;
            Gr(ixx);
        }
        // --------------График 
        //
        //
 private void Gr(int x)
 {
//     pc1.Image = null;

     if (maxx == minx) { MessageBox.Show("max == min = " + Convert.ToString(maxx)); return; }

       Bitmap btmBack = new Bitmap(pc1.Width, hh + 64);   
       grBack = Graphics.FromImage(btmBack);
       pc1.BackgroundImage = btmBack;
       pc1.Invalidate();
      
            // Font font = new Font("Default", 8); //, GraphicsUnit.Pixel)Regular;
            //StringFormat drawFormat = new StringFormat(StringFormatFlags.NoClip); //   .  NoClip);
            // string sx = " minx =  " + minx + "  maxx =  " + maxx;
         

            Pen p1 = new Pen(Color.Black, 1);
            Pen p2 = new Pen(Color.Black, 1);   // Red
            Pen p3 = new Pen(Color.Green, 1);           

            //  -----------------------------------------------------------------------------------------------------Ось x
            Font drawFont = new Font("Courier", 6, FontStyle.Regular,   GraphicsUnit.Point);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            grBack.DrawLine(p1, x0, h , w + x0, h );
            //grBack.DrawLine(p1, x0, hh,  w1 + x0, hh);
            for (int i = 0; i < w; i += 8) grBack.DrawLine(p1, i + x0, h , i + x0, h + 8);
            for (int i = x; i < w; i += 32)
            {
                string sx = i.ToString();
                grBack.DrawString(sx, drawFont, drawBrush, i + x0 - 8-x, h + 20); //, drawFormat);
             }
            //grBack.DrawString(sx, font, new SolidBrush(Color.Black), 40+x0, hh + 25, drawFormat);

            //  -----------------------------------------------------------------------------------------------------Ось y
            grBack.DrawLine(p1, x0, 8, x0, h + 8);
            for (int i = 8; i < hh + 8; i += 8) grBack.DrawLine(p1, x0, i, x0-5, i);

       

            double k = (hh) / 32;                                            
            double kx = (maxx - minx)/k;
            double nf = minx;
            double kf;
            for (int i = 0; i <= hh; i += 32)
            {
                kf = nf;
                string sx = kf.ToString("0.00");
                grBack.DrawString(sx, drawFont, drawBrush, 2, h - i); //, drawFormat);
                nf += kx;
               // grBack.DrawLine(p1, x0, i, x0 + w1, i);
            }
            

 //           grBack.DrawLine(p3, x0, 0, x0, h + 9);                                                                     // Значение координаты

            
                        for (int i = 0; i < w - 1 - x; i++)
                        {
                            int y1 =(int) (h - buf_gl[i + x]);
                            int y2 =(int) (h - buf_gl[i + 1 + x]);
                            grBack.DrawLine(p2, i + x0, y1, i + 1 + x0, y2);

                        }
            
            pc1.Refresh();

           // Controls.Add(pc1);

        }
 //---------------------------------------------------------------------------------------------------------------------------------------------------       
        
     

// Обработка мыши (вывод значений)    
        private void pc1_MouseMove(object sender, MouseEventArgs e)
        {
            Pen p2 = new Pen(Color.Black, 1);   // Red
            Pen p3 = new Pen(Color.Green, 1);
            Pen p0 = new Pen(Color.White, 1);
 
                int yPositon;
                int xPosition = e.X - x0;
                double с_buf1 = 0;
               
            if (xPosition >= 0 && xPosition < w)
              {
                  int i = xPosition + hScrollBar2.Value;
                  if (i < w && i>0)
                  {
                      yPositon = (int)buf_gl[xPosition + hScrollBar2.Value];
                      с_buf1 = buf1_gl[xPosition + hScrollBar2.Value];              // истинные значения  y
                      //label2.Text = Convert.ToString(i);
                      label1.Text = Convert.ToString(xPosition);
                      label2.Text = Convert.ToString(с_buf1);

                      if (k == 1)  {  grBack.DrawLine(p0, xc, h - 1, xc, yc); }

                      k = 0; 
                      xc = xPosition + x0; 
                      yc = h - yPositon + 1;

                      if ((yPositon - 1) > 2) 
                      {
                          xc = xPosition + x0; 
                          yc = h - yPositon + 1;
                          grBack.DrawLine(p3, xc, h - 1, xc, yc); 
                          k = 1;
                      }

                      for (int ii = 0; ii < w - 1 - ixx; ii++)
                      {
                          int y1 = (int)(h - buf_gl[ii + ixx]);
                          int y2 = (int)(h - buf_gl[ii + 1 + ixx]);
                          grBack.DrawLine(p2, ii + x0, y1, ii + 1 + x0, y2);

                      }

                    pc1.Refresh();
                  }
               
              }
            }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
           

            label6.Text = hScrollBar2.Minimum.ToString(); ;
            label7.Text = hScrollBar2.Maximum.ToString(); ;
            label8.Text = hScrollBar2.Value.ToString();
            ixx = hScrollBar2.Value;
            Gr(ixx);
 
        }
    }
    



    }


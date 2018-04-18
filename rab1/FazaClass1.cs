using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using rab1;
using System.Numerics;
using System.Threading;
using ClassLibrary;

public delegate void ImageProcessed(Bitmap resultBitmap);
//public delegate void ImageProcessedForOpenGL(List<Point3D> points);

namespace rab1
{
    public class FazaClass
    {
         public static event ImageProcessed imageProcessed;
        //public static event ImageProcessedForOpenGL imageProcessedForOpenGL;

        public static ZArrayDescriptor ATAN_N(ZArrayDescriptor[] zArrayDescriptor, double[] fz)
        {
            

            int n_sdv = fz.Length;                                             // Число фазовых сдвигов
            for (int i = 0; i < n_sdv; i++) if (zArrayDescriptor[i] == null) { MessageBox.Show("FazaCalass  zArrayDescriptor == NULL"); return null; }
           
            int w1 = zArrayDescriptor[0].width;
            int h1 = zArrayDescriptor[0].height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(w1, h1);        // Массив для фаз

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];
           
            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fz[i]); k_cos[i] = Math.Cos(fz[i]); }  //  Сдвиги фаз (4 сдвига - 0, 90, 180, 270  градусов)

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

            double pi2= Math.PI*2;
            for (int i = 0; i < w1; i++)
            {
                
                for (int j = 0; j < h1; j++)
                {
                  
                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[k].array[i, j]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }

                    double faza = Math.Atan2(-fz1, fz2);
                    if (faza < 0) faza = faza + pi2;
                    cmpl.array[i, j] = faza; 
                }
            }
            return cmpl;
        }

        public static ZArrayDescriptor ATAN_Gr(ZArrayDescriptor[] zArrayDescriptor, double[] fz)   // Фигура Лиссажу 1,2,3,4  => zArrayPicture
        {

            int n_sdv = fz.Length;                                             // Число фазовых сдвигов
            for (int i = 0; i < n_sdv; i++)
                if (zArrayDescriptor[i] == null) { MessageBox.Show("FazaCalass  zArrayDescriptor == NULL"); return null; }


            int w1 = zArrayDescriptor[0].width;
            int h1 = zArrayDescriptor[0].height;
            //ZArrayDescriptor cmpl = new ZArrayDescriptor(w1, h1);        // Массив для фаз

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];

            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fz[i]); k_cos[i] = Math.Cos(fz[i]); }  //  Сдвиги фаз (4 сдвига - 0, 90, 180, 270  градусов)

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

           // double pi2 = Math.PI * 2;
           
            double max_fz1 = double.MinValue; ;
            double max_fz2 = double.MinValue; ;
            double min_fz1 = double.MaxValue;
            double min_fz2 = double.MaxValue;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[k].array[i, j]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;
                    if (fz1 > max_fz1) max_fz1 = fz1;
                    if (fz2 > max_fz2) max_fz2 = fz2;
                    if (fz1 < min_fz1) min_fz1 = fz1;
                    if (fz2 < min_fz2) min_fz2 = fz2;
                }
            }
            //MessageBox.Show("min  fz1= " + min_fz1 + " max fz1= " + max_fz1 + "min  fz2= " + min_fz2 + " max fz2= " + max_fz2);
            int nn = 1024;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nn, nn);        // Массив для фаз
            nn = 255;
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[k].array[i, j]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;
                    int x = (int)  ((fz1 - min_fz1) * nn / (max_fz1 - min_fz1));
                    int y = (int)  ((fz2 - min_fz2) * nn / (max_fz2 - min_fz2));
                    cmpl.array[x+20, y+20] = 250;
                   
                   // cmpl.array[x+20, y+20] = 250;
                   
                }
            }

            return cmpl;
        }

        public static ZArrayDescriptor ATAN_Gr9101112(ZArrayDescriptor[] zArrayDescriptor, double[] fz)   // Фигура Лиссажу 9,10,11,12  => zArrayPicture
        {
            int n_img = 8;

            int n_sdv = fz.Length;                                             // Число фазовых сдвигов
            for (int i = 0; i < n_sdv; i++)
                if (zArrayDescriptor[n_img + i] == null) { MessageBox.Show("FazaCalass  zArrayDescriptor == NULL"); return null; }


            int w1 = zArrayDescriptor[n_img].width;
            int h1 = zArrayDescriptor[n_img].height;
            //ZArrayDescriptor cmpl = new ZArrayDescriptor(w1, h1);        // Массив для фаз

            double[] i_sdv = new double[n_sdv];
            double[] v_sdv = new double[n_sdv];                                  // Вектор коэффициентов
            double[] k_sin = new double[n_sdv];
            double[] k_cos = new double[n_sdv];

            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fz[i]); k_cos[i] = Math.Cos(fz[i]); }  //  Сдвиги фаз (4 сдвига - 0, 90, 180, 270  градусов)

            int[] ims1 = new int[h1];
            int[] ims2 = new int[h1];
            int[] ims3 = new int[h1];

            // double pi2 = Math.PI * 2;

            double max_fz1 = double.MinValue; ;
            double max_fz2 = double.MinValue; ;
            double min_fz1 = double.MaxValue;
            double min_fz2 = double.MaxValue;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {

                    // ------                                     Формула расшифровки
                    for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[k + n_img].array[i, j]; }

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;
                    if (fz1 > max_fz1) max_fz1 = fz1;
                    if (fz2 > max_fz2) max_fz2 = fz2;
                    if (fz1 < min_fz1) min_fz1 = fz1;
                    if (fz2 < min_fz2) min_fz2 = fz2;
                }
            }
            //MessageBox.Show("min  fz1= " + min_fz1 + " max fz1= " + max_fz1 + "min  fz2= " + min_fz2 + " max fz2= " + max_fz2);
            int nn1 = 256;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nn1, nn1);        // Массив для фаз
            int nn = nn1/2;
            // for (int i = 0; i < w1; i++)
            // {
            //      for (int j = 0; j < h1; j++)
            int t = 100;
            for (int i = w1/2-t; i < w1/2+t; i++)
            {
              for (int j = h1 / 2 - t; j < h1 / 2 + t; j++)
            {

                // ------                                     Формула расшифровки
                for (int k = 0; k < n_sdv; k++) { i_sdv[k] = zArrayDescriptor[k + n_img].array[i, j]; }
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1];
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2];
                    for (int k = 1; k < n_sdv - 1; k++) v_sdv[k] = i_sdv[k + 1] - i_sdv[k - 1];
                    double fz1 = 0;
                    double fz2 = 0;
                    for (int k = 0; k < n_sdv; k++) { fz1 += v_sdv[k] * k_sin[k]; fz2 += v_sdv[k] * k_cos[k]; }
                    fz1 = -fz1;

                    int x = (int)((fz1 - min_fz1) * nn / (max_fz1 - min_fz1));
                    int y = (int)((fz2 - min_fz2) * nn / (max_fz2 - min_fz2));
                    cmpl.array[x + nn1/4, y + nn1 / 4] += 20;                              // В центр
                }
            }
/*
            double min = SumClass.getMin(cmpl);
            double max = SumClass.getMax(cmpl);
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    cmpl.array[i, j] = (cmpl.array[i,j] - min) * 255 / (max - min);
                   
                }
            }
*/

                    return cmpl;
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        public static void Graph_ATAN(Image[] img, int x0_end, int x1_end, int y0_end, int y1_end, int n, double[] fzz, double Gamma)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            int w1 = img[0].Width;
            int h1 = img[0].Height;
           
            int n_sdv = n;                                                       // Число фазовых сдвигов

            int[] i_sdv = new int[4];
            int [] v_sdv = new int[4];
           
            Color c;

            Bitmap bmp1 = (Bitmap)img[0];
            Bitmap bmp2 = (Bitmap)img[1];
            Bitmap bmp3 = (Bitmap)img[2];
            Bitmap bmp4 = (Bitmap)img[3];


            BitmapData data1 = ImageProcessor.getBitmapData(bmp1);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);
            BitmapData data3 = ImageProcessor.getBitmapData(bmp3);
            BitmapData data4 = ImageProcessor.getBitmapData(bmp4);

            int x0=0, x1=w1, y0=0, y1=h1;
            if (x0_end !=0 || x1_end !=0 || y0_end !=0 || y1_end !=0 ) 
            { 
                x0=x0_end; x1=x1_end;  y0=y0_end; y1=y1_end;
            }

            int r;
            double fz1, fz2;

            int[,] buffer1 = new int[x1, y1];
            int[,] buffer2 = new int[x1, y1];
            int[,] buffer3 = new int[x1, y1];
            int[,] buffer4 = new int[x1, y1];

            double[] k_sin = new double[4];
            double[] k_cos = new double[4];
            double[,] vvs = new double[4, 512];
            double[,] vvc = new double[4, 512];
            double pi = Math.PI;
            for (int i = 0; i < n_sdv; i++) { k_sin[i] = Math.Sin(fzz[i] * pi / 180); k_cos[i] = Math.Cos(fzz[i] * pi / 180); }  //  Сдвиги фаз 
            for (int ii = 0; ii < n_sdv; ii++) for (int i = 0; i < 512; i++) { vvs[ii, i] = i * k_sin[ii]; vvc[ii, i] = i * k_cos[ii]; } 
  
            double min1 = 35000, max1 = -35000;

            int w11 = 600;
            int h11 = 600;

            int all = x1 * y1 * 2 + w11 + h11;
            int done = 0;
            
            for (int i = x0; i < x1; i++)
            {
                done = i * x1;

                PopupProgressBar.setProgress(done, all);

                for (int j = y0; j < y1; j++)
                {
                    c = ImageProcessor.getPixel(i, j, data1);
                    r = (c.R + c.G + c.B) / 3;      
                    i_sdv[0] = (int)Math.Pow(r, Gamma);     
                    buffer1[i, j] = i_sdv[0];

                    c = ImageProcessor.getPixel(i, j, data2);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[1] = (int)Math.Pow(r, Gamma);  
                    buffer2[i, j] = i_sdv[1];

                    c = ImageProcessor.getPixel(i, j, data3);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[2] = (int)Math.Pow(r, Gamma); 
                    buffer3[i, j] = i_sdv[2];

                    c = ImageProcessor.getPixel(i, j, data4);
                    r = (c.R + c.G + c.B) / 3;   
                    i_sdv[3] = (int)Math.Pow(r, Gamma);   
                    buffer4[i, j] = i_sdv[3];
               
                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1] + 255;
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2] +255;

                    for (int ii = 1; ii < n_sdv - 1; ii++)
                    {  
                        v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1] + 255; 
                    }

                    fz1 = fz2 = 0; 

                    for (int ii = 0; ii < n_sdv; ii++) 
                    { 
                       fz2 += vvs[ii, v_sdv[ii]];
                       fz1 += vvc[ii, v_sdv[ii]];
                    }

                    max1 = Math.Max(fz1, max1); 
                    max1 = Math.Max(fz2, max1);
                    min1 = Math.Min(fz1, min1); 
                    min1 = Math.Min(fz2, min1);                              
                }
            }
            
            int x, y;
            double fw = (int)w11;
            fw = fw / (max1-min1);
            Bitmap bmp5 = new Bitmap(w11, h11);
            BitmapData data5 = ImageProcessor.getBitmapData(bmp5);
            int[,] buffer5 = new int[w11, h11];

            
            for (int i = x0; i < x1; i++)
            {
                done += x1;
                PopupProgressBar.setProgress(done, all);

                for (int j = y0; j < y1; j++)
                {
                    i_sdv[0] = buffer1[i, j]; 
                    i_sdv[1] = buffer2[i, j]; 
                    i_sdv[2] = buffer3[i, j]; 
                    i_sdv[3] = buffer4[i, j];

                    v_sdv[0] = i_sdv[1] - i_sdv[n_sdv - 1] + 255;
                    v_sdv[n_sdv - 1] = i_sdv[0] - i_sdv[n_sdv - 2] + 255;
                    for (int ii = 1; ii < n_sdv - 1; ii++) { v_sdv[ii] = i_sdv[ii + 1] - i_sdv[ii - 1] + 255; }


                    fz1 = fz2 = 0;
                    for (int ii = 0; ii < n_sdv; ii++)
                    {
                        fz2 += vvs[ii, v_sdv[ii]];
                        fz1 += vvc[ii, v_sdv[ii]];
                    }
                   
                    //-------------------------------------------------------------------------------------------

                    x = (int)((fz1-min1) * fw);
                    y = (int)((fz2-min1) * fw) ;

                    if ((x < w11 && x >= 0) && (y < h11 && y >= 0))
                    {
                        r = buffer5[x, y]; r++;
                        if (r > 255) { r = 255;  }
                        buffer5[x, y]=r;
                    }
                }
            }
            for (int i = 0; i < w11; i++)
            {
                done += w11;

                PopupProgressBar.setProgress(done, all);

                for (int j = 0; j < h11; j++)
                {
                    ImageProcessor.setPixel(data5, i, j, Color.FromArgb(buffer5[i, j], 0, 0));
                }
            }

            PopupProgressBar.setProgress(all, all);

            bmp1.UnlockBits(data1);
            bmp2.UnlockBits(data2);
            bmp3.UnlockBits(data3);
            bmp4.UnlockBits(data4);
            bmp5.UnlockBits(data5);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (imageProcessed != null)
            {
                imageProcessed(bmp5);
            }

            if (imageProcessedForOpenGL != null)
            {
                List<Point3D> newList = new List<Point3D>();

                for (int i = x0; i < x1; i++)
                {
                    for (int j = y0; j < y1; j++)
                    {
                        newList.Add(new Point3D(buffer1[i, j], buffer2[i, j], buffer3[i, j])); 
                    }                    
                }

                imageProcessedForOpenGL(newList);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    */

    }
}

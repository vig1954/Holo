using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ClassLibrary;


namespace rab1
{
    public  class SumClass
    {

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения максимума из массива
        /// </summary>
        public static double getMax(ZArrayDescriptor newDescriptor)
        {
            if (newDescriptor == null)
            {
                return 0;
            }

            double max = double.MinValue; 

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                   // max = Math.Max(max, newDescriptor.array[i, j]);
                    if (max < newDescriptor.array[i, j]) max = newDescriptor.array[i, j];
                }
            }

            return max;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Метод для получения минимума из массива
        /// </summary>
        public static double getMin(ZArrayDescriptor newDescriptor)
        {
            if (newDescriptor == null)   {  return 0;   }

            double min = double.MaxValue;

            for (int i = 0; i < newDescriptor.width; i++)
            {
                for (int j = 0; j < newDescriptor.height; j++)
                {
                    //min = Math.Min(min, newDescriptor.array[i, j]);
                    if (min > newDescriptor.array[i, j]) min = newDescriptor.array[i, j];
                }
            }

            return min;
        }


        public static ZArrayDescriptor Inversia(ZArrayDescriptor amp)
        {
            int w1 = amp.width;
            int h1 = amp.height;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);
            double max = getMax(amp);
            //double min = getMin(amp);

            for (int i = 0; i < w1; i++)    // По строкам
                for (int j = 0; j < h1; j++)
                {
                    res_array.array[i, j] = (max - amp.array[i, j]);

                }

            return res_array;
        }

        public static ZArrayDescriptor Sum_zArray(ZArrayDescriptor a, ZArrayDescriptor b)
        {
            int w1 = a.width;
            int h1 = a.height;
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);
            
            //double min = getMin(amp);

            for (int i = 0; i < w1; i++)    // По строкам
                for (int j = 0; j < h1; j++)
                {
                    res_array.array[i, j] = a.array[i, j]+ b.array[i, j];
                }

            return res_array;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///          Приведение изображения к диапазону
        /// </summary>     
        public static void Range_Picture(PictureBox pictureBox01, ZArrayDescriptor zArrayPicture, double min, double max)
        {

            int width  = zArrayPicture.width;
            int height = zArrayPicture.height;
            Bitmap bmp2 = new Bitmap(width, height);
            BitmapData data2 = ImageProcessor.getBitmapData(bmp2);

            // c1 = ImageProcessor.getPixel(i, j, data1);                       // c1 = bmp1.GetPixel(i, j);   
            // ImageProcessor.setPixel(data5, i, j, Color.FromArgb(r, r, r));   // bmp2.SetPixel(j, i, c1);
            // bmp5.UnlockBits(data5);   
            if (pictureBox01  == null) {  }
            if (zArrayPicture == null) { MessageBox.Show("SumClass ZArrayDescriptor array == null"); return; }

           
            if (max == min)
            {   
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        int c = 0;
                        if (max < 255 && max > 0.0) c = Convert.ToInt32(max);
                        if (max > 255) c = 255;
                        if (max < 0) c = 0;
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }

            }
            if (max != min)
            {
                double mxmn = 255.0 / (max - min);
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        double fc = zArrayPicture.array[j, i];
                        if (fc > max) fc = max;
                        if (fc < min) fc = min;
                        int c = Convert.ToInt32((fc - min) * mxmn);
                        Color c1 = Color.FromArgb(c, c, c);
                        ImageProcessor.setPixel(data2, j, i, c1);
                    }

                }
            }
            pictureBox01.Image = bmp2;
            bmp2.UnlockBits(data2);

        }
        /// <summary>
        /// Прведение к диапазону
        /// Если zArrayPicture распределен от min1 до max1 => диапазон расширяется до  min max
        /// 
        /// </summary>
        /// <param name="zArrayPicture"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ZArrayDescriptor Range_Array(ZArrayDescriptor zArrayPicture, double min, double max)
        {

            int width  = zArrayPicture.width;
            int height = zArrayPicture.height;
            ZArrayDescriptor rezult = new ZArrayDescriptor(width, height);
                   
            if (zArrayPicture == null) { MessageBox.Show("SumClass ZArrayPicture == null"); return null; }

            if (max == min)
            {
                
                for (int j = 0; j < width; j++) 
                  {  
                      for (int i = 0; i < height; i++)  
                         {
                             if (max < 255 && max > 0.0) rezult.array[i, j] = max; 
                             if (max > 255)              rezult.array[i, j] = 255; 
                             if (max < 0)                rezult.array[i, j] = 0; 
                             
                         }  
                   }
                return rezult;
            }


            double min1 = getMin(zArrayPicture);
            double max1 = getMax(zArrayPicture);


            double mxmn = 1 / (max1 - min1);

            for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                       double fc = zArrayPicture.array[i, j];
                    //if (fc > max) fc = max;
                    //if (fc < min) fc = min;
                    //if (fc > max && fc < min) { fc = (fc - min) * mxmn; }
                       fc =(max-min)* (fc - min) * mxmn + min;
                       rezult.array[i, j] = fc;
                    }
                }
           
            return rezult;

        }

        //
        //                         Устранение фазовой неоднозначности
        //
        public static ZArrayDescriptor Shift_Picture(ZArrayDescriptor zArrayPicture, double o_gr, double o_mn)
        {
            if (zArrayPicture == null) { MessageBox.Show("SumClass ZArrayDescriptor array == null"); return null; }
            int width = zArrayPicture.width;
            int height = zArrayPicture.height;
            ZArrayDescriptor z_array = new  ZArrayDescriptor(width, height);
            double max = getMax(zArrayPicture);
            //double min = getMin(zArrayPicture);
            double min = o_mn;
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        double c = zArrayPicture.array[i,j];
                        if (c > o_gr ) c = min-(max-c);
                        z_array.array[i, j] = c;
                    }

                }

            
           return z_array;

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  
        /// </summary>        
        
        
        
        public static void Sum_Color(Image[] img, int k1, int k2, int k3)
        {
            int w1 = img[k1-1].Width;
            int h1 = img[k1-1].Height;
            Bitmap bmp1 = new Bitmap(img[k1-1], w1, h1);
            

            int w2 = img[k2 - 1].Width;
            int h2 = img[k2 - 1].Height;
            Bitmap bmp2 = new Bitmap(img[k2 - 1], w2, h2);
           

            w1 = (int)Math.Min(w1, w2);
            h1 = (int)Math.Min(h1, h2);
           
            Bitmap bmp3 = new Bitmap(img[k3 - 1], w1, h1);

            Color c1,c2;
            int r1, r2, rs , rg, rb;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j);
                    c2 = bmp2.GetPixel(i, j);
                    r1 = c1.R; r2 = c2.R; rs = r1 + r2; if (rs > 255) rs = 255;
                    r1 = c1.G; r2 = c2.G; rg = r1 + r2; if (rg > 255) rg = 255;
                    r1 = c1.B; r2 = c2.B; rb = r1 + r2; if (rb > 255) rb = 255;
                    bmp3.SetPixel(i, j, Color.FromArgb(rs, rg, rb));
                }
            }
            img[k3 - 1] = bmp3;
           
        }


        public static void Sub_Color(Image[] img, int k1, int k2, int k3, double N1, double N2)
        {
            int w1 = img[k1 - 1].Width;
            int h1 = img[k1 - 1].Height;
            Bitmap bmp1 = new Bitmap(img[k1 - 1], w1, h1);


            int w2 = img[k2 - 1].Width;
            int h2 = img[k2 - 1].Height;
            Bitmap bmp2 = new Bitmap(img[k2 - 1], w2, h2);


            w1 = (int)Math.Min(w1, w2);
            h1 = (int)Math.Min(h1, h2);

            Bitmap bmp3 = new Bitmap(img[k3 - 1], w1, h1);

            Color c1, c2;
            int r1, r2, rs;
            int max=-32000, min=32000;

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j);  r1 = (int) (c1.R * N1);
                    c2 = bmp2.GetPixel(i, j);  r2 = (int) (c2.R * N2);
                    rs = (r1 - r2);
                    min = Math.Min(rs, min);
                    max = Math.Max(rs, max);
                }
            }
// ---------------------------------------------------------------------------------------------------
            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    c1 = bmp1.GetPixel(i, j); r1 = (int) (c1.R * N1);
                    c2 = bmp2.GetPixel(i, j); r2 = (int) (c2.R * N2);
                    rs = (r1 - r2);
                    rs = (rs - min) * 255 / (max - min);
                    bmp3.SetPixel(i, j, Color.FromArgb(rs, rs, rs));
                }
            }
            img[k3 - 1] = bmp3;

        }
    }
}

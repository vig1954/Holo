using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;
using System.Diagnostics;
using rab1.Forms;

namespace rab1
{
    class File_Change_Size
    {
        private static int min4x(int X1, int X2, int X3, int X4)
        {
            int min = int.MaxValue;

            if (X1 < min) min = X1;
            if (X2 < min) min = X2;
            if (X3 < min) min = X3;
            if (X4 < min) min = X4;
            return min;
        }

        private static int max4x(int X1, int X2, int X3, int X4)
        {
            int max = 0;

            if (X1 > max) max = X1;
            if (X2 > max) max = X2;
            if (X3 > max) max = X3;
            if (X4 > max) max = X4;
            return max;
        }

        public static void Change_r(ZArrayDescriptor[] zArrayDescriptor,
                                    PictureBox pictureBox9, PictureBox pictureBox10, PictureBox pictureBox11, PictureBox pictureBox12,
                                    int X1, int X2, int X3, int X4, int Y1, int Y2, int Y3, int Y4)
        {
            for (int i = 8; i < 12; i++) 
                          { if (zArrayDescriptor[i] == null) { MessageBox.Show(" Change_r: zArrayDescriptor[" + i + "] == null"); return; } }
            
            int max_X = zArrayDescriptor[8].width;
            int max_Y = zArrayDescriptor[8].height;
            
            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);

            int rx = (maxx - minx) / 2;
            int ry = (maxy - miny) / 2;
            int rad = Math.Min(rx, ry);
            int x0 = minx + rad;
            int y0 = miny + rad;

            //Vizual.Vizual_Circle(zArrayPicture, pictureBox01, x0, y0, rad);    // Рисование круга
            // Заполнение нулями за пределами круга


            Vizual.Vizual_Circle(zArrayDescriptor[8],   pictureBox9,  x0, y0, rad); 
            Vizual.Vizual_Circle(zArrayDescriptor[9],   pictureBox10, x0, y0, rad);
            Vizual.Vizual_Circle(zArrayDescriptor[10],  pictureBox11, x0, y0, rad); 
            Vizual.Vizual_Circle(zArrayDescriptor[11],  pictureBox12, x0, y0, rad); 
           
            if ((maxx - minx) % 2 != 0) maxx++;
            if ((maxy - miny) % 2 != 0) maxy++;

            int X = maxx - minx, Y = maxy - miny;


            int k = X / 256;     X = (k + 1) * 256;        // Выравниваем на границe L=256 (для фурье с четным количеством точек)
                k = Y / 256;     Y = (k + 1) * 256;
            while (X + minx > max_X) X = X - 128;
            while (Y + miny > max_Y) Y = Y - 128;
            X = Math.Max(X, Y);
           
            MessageBox.Show("Реальный размер  minx=  " + (maxx - minx) + " miny=  " + (maxy - miny) + "Рекомендованный размер  =  " + X + " x " + Y);

            ZArrayDescriptor zArray = new ZArrayDescriptor(X, X);

            int x00 = 0, y00 = 0;
            int YY = X; int XX = X;

            if (   (X > (maxx - minx)) && (Y > (maxy - miny))   )
            {
                int kx = (X - (maxx - minx)) / 2; 
                int ky = (X - (maxy - miny)) / 2; 

                x00 = minx - kx; if (x00 > 0) XX = X - kx; else { x00 = 0; }
                y00 = miny - ky; if (y00 > 0) YY = X - ky; else { y00 = 0; }
               // MessageBox.Show(" kx=  " + kx + " x0=  " + x00 + " y0=  " + y00 + "X  =  " + X + "Y =  " + Y);
            }
           
            XX = Math.Max(XX, YY);

            if (XX + x00 > max_X) {   x00 = max_X - XX; }
            if (XX + y00 > max_Y) {   y00 = max_X - XX; }

            //int n = 8;
            for (int n = 8; n < 12; n++)
            {
                for (int i = x00; i < XX + x00; i++)
                    for (int j = y00; j < XX + y00; j++)
                       { zArray.array[i - x00, j - y00] = zArrayDescriptor[n].array[i, j]; }
                //zArrayDescriptor[n] = zArray;
                zArrayDescriptor[n] = new ZArrayDescriptor(zArray);   
            }

        }

        public static ZArrayDescriptor Change_4(ZArrayDescriptor zArrayPicture, int X1, int X2, int X3, int X4, int Y1, int Y2, int Y3, int Y4)
        {
            if (zArrayPicture == null) { MessageBox.Show(" zArrayPicture File_Change_Size.cs == null"); return null; }

           
            int max_X = zArrayPicture.width;
            int max_Y = zArrayPicture.height;

            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);
      
            if ((maxx - minx) % 2 != 0) maxx++;    // Четное количество точек
            if ((maxy - miny) % 2 != 0) maxy++;

            int X = maxx - minx, Y = maxy - miny;


            int k = X / 256; X = (k + 1) * 256;   // Выравниваем на границe L=256 (для фурье с четным количеством точек)
            k = Y / 256; Y = (k + 1) * 256;
            X = Math.Max(X, Y);
            while (X + minx > zArrayPicture.width) X = X - 128;
            while (X + miny > zArrayPicture.height) X = X - 128;

            MessageBox.Show("Реальный размер  minx=  " + (maxx - minx) + " miny=  " + (maxy - miny) + "Рекомендованный размер  =  " + X + " x " + X);


            ZArrayDescriptor zArray = new ZArrayDescriptor(X, X);
            int x0 = 0, y0 = 0;
            Y = X;
            if (X>(maxx-minx) && Y>(maxy - miny))
            {
                int kx = (X - (maxx - minx)) / 2; 
                int ky = (X - (maxy - miny)) / 2; 
           
              x0 = minx - kx; if (x0 > 0 ) X = X - kx; else { x0 = 0; }
              y0 = miny - ky; if (y0 > 0)  Y = X - ky; else { y0 = 0; }
            }
            X = Math.Max(X, Y);

           // MessageBox.Show("x0=  " + x0 + " y0=  " + y0 + "X  =  " + X );
            
            for (int i = x0; i < X+x0; i++)
                for (int j = y0; j < X+y0; j++)
                    zArray.array[i - x0, j - y0] = zArrayPicture.array[i, j];

            return zArray;

        }

       
    }
}

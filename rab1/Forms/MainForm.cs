using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Reflection;
using System.Diagnostics;
using rab1.Forms;
using ClassLibrary;
using System.Numerics;

namespace rab1
{
    public delegate void FunctionPointer(object sender, EventArgs eventArgs);

    public partial class Form1 : Form
    {
        /*
                public class VerticalProgressBar : ProgressBar
                {
                    protected override CreateParams CreateParams
                    {
                        get
                        {
                            CreateParams cp = base.CreateParams;
                            cp.Style |= 0x04;
                            return cp;
                        }
                    }
                }

        */
// -----------------------------------------------------------------------------------------------------------
        Image[] img = new Image[12];
        public static ZArrayDescriptor[] zArrayDescriptor = new ZArrayDescriptor[12];     // Иконки справа      
        public static ZArrayDescriptor zArrayPicture = new ZArrayDescriptor();            // Массив для главного окна
        public static ZComplexDescriptor[] zComplex = new ZComplexDescriptor[3];

        public static int regImage = 0;                           // Номер изображения (0-11)
        public static int regComplex = 0;                         // Номер Complex (0-3)

        int scaleMode = 0;                          // Масштаб изображения

       // int X1, Y1;                       // Первая точка (Шелкаем на экране)
       // int X2, Y2;
       // int X3, Y3;
       // int X4, Y4;

        Form f_filt;                             // Для Фильтрации
        TextBox tb1_filt; //, tb2_filt, tb3_filt;
        int k_filt = 1;

        //int x0_end = 0, y0_end = 0;
        //int x1_end = 0, y1_end = 0;



        string string_dialog; // = "D:\\Студенты\\Эксперимент";       


        //int pr_obr = 10;

        int cursorMode = 0;
        Point downPoint;
        //Point upPoint;


        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);
        // private delegate void StringParameterDelegate(List<Point3D> newList);

        //CustomPictureBox firsPictureBox;
        //CustomPictureBox secondPictureBox;

        //int batchProcessingFlag = 0;

        //private double currentScaleRatio = 1;
        private double initialScaleRatio = 1;
        private double afterRemovingScaleRatio = 1;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Form1()
        {
            InitializeComponent();

            //ShooterSingleton.init();

            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageTaken(Image newImage)
        {
            //изображение получено
            pictureBox1.Image = newImage;
            //  ShooterSingleton.imageCaptured -= imageTaken;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                Режимы графика
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton9_CheckedChanged(object sender, EventArgs e)        // Координаты 1 точки
        {
            if (radioButton913.Checked == true) { cursorMode = 0; }
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)        // Координаты 2 точки
        {
            if (radioButton16.Checked == true) { cursorMode = 1; }
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)       // Координаты 3 точки
        {
            if (radioButton17.Checked == true) { cursorMode = 2; }
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)       // Координаты 4 точки
        {
            if (radioButton18.Checked == true) { cursorMode = 3; }
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)       // График из zArray
        {
            //if (cursorMode == 7) { radioButton15.Checked = false; cursorMode = 0;  return;  }
            //if (cursorMode != 7) { radioButton15.Checked = true;  cursorMode = 7;  return; }

            if (radioButton15.Checked == true) { cursorMode = 7; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                    MouseMove  на основном окне
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox01_MouseMove(object sender, MouseEventArgs e)
        {
            /*
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            imageWidth.Text = zArrayPicture.width.ToString();
            imageHeight.Text = zArrayPicture.height.ToString();
            int x = (int)e.X;
            int y = (int)e.Y;
            imageWidth.Text  = x.ToString();
            imageHeight.Text = y.ToString();
            */
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                    MouseClick  на основном окне
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static int pdgn_scale(int w_max, int h_max, int rx, int ry, int x, int y, int k)
        {
            double kx = 1, ky = 1;

            if (w_max > h_max)               // Ширина больше высоты
            {
                kx = (double)w_max / rx;
                x = (int)(x * kx);
                int y0 = (int)(ry - h_max / kx) / 2;
                int y1 = ry - y0;

                if (y < y0) y = 0;
                if (y > y1) y = h_max;
                if ((y >= y0) && y <= y1) y = (int)((y - y0) * kx);
                // MessageBox.Show("y0= " + y0 +  " y1= " + y1 );
            }
            if (w_max < h_max)
            {
                ky = (double)h_max / ry;
                y = (int)(y * ky);
                int x0 = (int)(rx - w_max / ky) / 2;  //  int rx = pictureBox01.Width;
                int x1 = ry - x0;

                if (x < x0) x = 0;
                if (x > x1) x = w_max;
                if ((x >= x0) && x <= x1) x = (int)((x - x0) * ky);
            }

            if (w_max == h_max)
            {
                ky = (double)h_max / ry;
                y = (int)(y * ky);
                int x0 = (int)(rx - w_max / ky) / 2;

                //int x1 = ry - x0;
                int x1 = rx - x0;

                if (x < x0) x = 0;
                if (x > x1) x = w_max;
                if ((x >= x0) && x <= x1) x = (int)((x - x0) * ky);
            }
            if (k == 1) return x; else return y;

        }

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            //if (pictureBox01.Image == null)  {  return;   }
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }

            int w_max = zArrayPicture.width;
            int h_max = zArrayPicture.height;


            imageWidth.Text = w_max.ToString();                                                        // Отображение размера файла -> imageWidth.Text
            imageHeight.Text = h_max.ToString();

            int rx = pictureBox01.Width;
            int ry = pictureBox01.Height;

            int X1=0, Y1 = 0;                       // Первая точка (Шелкаем на экране)
             int X2, Y2;
             int X3, Y3;
             int X4, Y4;

            //double  kx = 1, ky = 1;
            int x = 0, y = 0;

            x = (int)e.X; y = (int)e.Y;

            // MessageBox.Show("Width= " + rx + " Height= " + ry + "x= " + x + " y= " + y);    
            if (scaleMode == 1)                                                                            // Подогнанный режим отображения      
            {
                x = pdgn_scale(w_max, h_max, rx, ry, x, y, 1);
                y = pdgn_scale(w_max, h_max, rx, ry, x, y, 2);
            }
            switch (cursorMode)   // Задание 4 точек
            {
                case 0: X1 = x; Y1 = y; //x1.Text = X1.ToString(); y1.Text = Y1.ToString();
                    textBox3.Text = x.ToString(); textBox4.Text = y.ToString();                                   break;
                case 1: X2 = x; Y2 = y; //xx2.Text = X2.ToString(); yy2.Text = Y2.ToString();
                    textBox5.Text = x.ToString(); textBox6.Text = y.ToString(); break;
                case 2: X3 = x; Y3 = y; //xx3.Text = X3.ToString(); yy3.Text = Y3.ToString();
                    textBox7.Text = x.ToString(); textBox8.Text = y.ToString(); break;
                case 3: X4 = x; Y4 = y; // xx4.Text = X4.ToString(); yy4.Text = Y4.ToString();
                    textBox9.Text = x.ToString(); textBox10.Text = y.ToString(); break;
            }
           // MessageBox.Show("zArrayPicture "+ zArrayPicture.height);
            if (zArrayPicture.height != 0 || zArrayPicture.width != 0) { textBox11.Text = zArrayPicture.array[X1, Y1].ToString(); }  // Величина точки           

            // z1.Text = pictureBox01.Image.Height.ToString();

            /*  if (cursorMode != 0)
              {
                  if (cursorMode == 2)  {    ImageProcessor.floodImage(e.X, e.Y, Color.Black, pictureBox01.Image);      }
                  return;
              }
             * */
            if (cursorMode == 7)   // Режим построения графика из массива (Масштаб должен быть реальным или подогнанным)
            {

                int xx = (int)e.X;
                int yy = (int)e.Y;

                if (scaleMode == 1)                                                                            // Подогнанный режим отображения      
                {
                    xx = pdgn_scale(w_max, h_max, rx, ry, xx, yy, 1);
                    yy = pdgn_scale(w_max, h_max, rx, ry, xx, yy, 2);
                }
                //imageWidth.Text = Convert.ToString(x);
                //imageHeight.Text = Convert.ToString(y);


                if (radioButton22.Checked != true)                                      // по X
                {
                    double[] buf = new double[zArrayPicture.width];
                    buf = Graphic_util.Graph_x(zArrayPicture, yy);
                    Graphic graphic_x = new Graphic(zArrayPicture.width, buf);
                    graphic_x.Show();
                }
                else                                                                     // по Y
                {
                    double[] buf1 = new double[zArrayPicture.height];
                    buf1 = Graphic_util.Graph_y(zArrayPicture, xx);
                    Graphic graphic_y = new Graphic(zArrayPicture.height, buf1);
                    graphic_y.Show();
                }
                pictureBox01.Invalidate();


                //ImageHelper.drawGraph(pictureBox01.Image, e.X, e.Y, currentScaleRatio);
            }


        }
        // -----------------------------------------------------------      Refresh
        private void button9_Click(object sender, EventArgs e)
        {
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // -----------------------------------------------------------------------------Ограничение четырех массивов (9,10,11,12)
        //private void выделениеКругаToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        // }
        private void рисованиеПрямоугольникаToolStripMenuItem_Click(object sender, EventArgs e) // Рисование прямоугольника по 4 выделенным точкам в zArrayPicture
        {

            if (zArrayPicture.height == 0) { MessageBox.Show(" zArrayPicture == null"); return; }

            int X1, X2, X3, X4, Y1, Y2, Y3, Y4;

            X1 = Convert.ToInt32(textBox3.Text);
            Y1 = Convert.ToInt32(textBox4.Text);

            X2 = Convert.ToInt32(textBox5.Text);
            Y2 = Convert.ToInt32(textBox6.Text);

            X3 = Convert.ToInt32(textBox7.Text);
            Y3 = Convert.ToInt32(textBox8.Text);

            X4 = Convert.ToInt32(textBox9.Text);
            Y4 = Convert.ToInt32(textBox10.Text);

            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);  

            Vizual.Vizual_Rect_Ris(zArrayPicture, pictureBox01, minx, miny, maxx, maxy);

        }

        private void рисованиеОкружностиToolStripMenuItem_Click(object sender, EventArgs e) // Рисование окружности по 4 выделенным точкам в zArrayPicture
        {
            //if (regImage != 8) { MessageBox.Show(" regImage =" + regImage + "!= 8"); return; }


            if (zArrayPicture.height == 0) { MessageBox.Show(" zArrayPicture == null"); return; }

            int X1, X2, X3, X4, Y1, Y2, Y3, Y4;

            X1 = Convert.ToInt32(textBox3.Text);
            Y1 = Convert.ToInt32(textBox4.Text);

            X2 = Convert.ToInt32(textBox5.Text);
            Y2 = Convert.ToInt32(textBox6.Text);

            X3 = Convert.ToInt32(textBox7.Text);
            Y3 = Convert.ToInt32(textBox8.Text);

            X4 = Convert.ToInt32(textBox9.Text);
            Y4 = Convert.ToInt32(textBox10.Text);

            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);

            int rx = (maxx - minx) / 2;
            int ry = (maxy - miny) / 2;
            int r = Math.Min(rx, ry);
            int x0 = minx + r;
            int y0 = miny + r;

            Vizual.Vizual_Circle_Ris(zArrayPicture, pictureBox01, x0, y0, r);

        }

        // Выделение круга по 4 выделенным точкам в 8,9,10 и 11 zArrayDescriptor[]

        private void выделениеКвадратаПо4ВыделеннымТочкамToolStripMenuItem_Click(object sender, EventArgs e)  
        {

            int X1 = Convert.ToInt32(textBox3.Text);
            int Y1 = Convert.ToInt32(textBox4.Text);

            int X2 = Convert.ToInt32(textBox5.Text);
            int Y2 = Convert.ToInt32(textBox6.Text);

            int X3 = Convert.ToInt32(textBox7.Text);
            int Y3 = Convert.ToInt32(textBox8.Text);

            int X4 = Convert.ToInt32(textBox9.Text);
            int Y4 = Convert.ToInt32(textBox10.Text);

            File_Change_Size.Change_r(zArrayDescriptor, pictureBox9, pictureBox10, pictureBox11, pictureBox12,X1, X2, X3, X4, Y1, Y2, Y3, Y4);
            Vizual_regImage(8); Vizual_regImage(9); Vizual_regImage(10); Vizual_regImage(11);
        }

        private int min4x(int X1, int X2, int X3, int X4)
        {
            int min = int.MaxValue;

            if (X1 < min) min = X1;
            if (X2 < min) min = X2;
            if (X3 < min) min = X3;
            if (X4 < min) min = X4;
            return min;
        }

        private int max4x(int X1, int X2, int X3, int X4)
        {
            int max = 0;

            if (X1 > max) max = X1;
            if (X2 > max) max = X2;
            if (X3 > max) max = X3;
            if (X4 > max) max = X4;
            return max;
        }

        private void выделениеКругаПо4ТочкамToolStripMenuItem_Click(object sender, EventArgs e)  // Выделение круга в zArrayPicture
        {
            //if (regImage != 8) { MessageBox.Show(" regImage =" + regImage + "!= 8"); return; }


            if (zArrayPicture.height == 0) { MessageBox.Show(" zArrayPicture == null"); return; }

            int X1, X2, X3, X4, Y1, Y2, Y3, Y4;

            X1 = Convert.ToInt32(textBox3.Text);
            Y1 = Convert.ToInt32(textBox4.Text);

            X2 = Convert.ToInt32(textBox5.Text);
            Y2 = Convert.ToInt32(textBox6.Text);

            X3 = Convert.ToInt32(textBox7.Text);
            Y3 = Convert.ToInt32(textBox8.Text);

            X4 = Convert.ToInt32(textBox9.Text);
            Y4 = Convert.ToInt32(textBox10.Text);

            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);

            int rx = (maxx - minx) / 2;
            int ry = (maxy - miny) / 2;
            int r = Math.Min(rx, ry);
            int x0 = minx + r;
            int y0 = miny + r;

            Vizual.Vizual_Circle(zArrayPicture, pictureBox01, x0, y0, r);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


        // -----------------Ограничение массива прямоугольником zArrayPiture
        private void выделениеКвадратаПо4ёмВыделеннымТочкам9101112ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            int X1 = Convert.ToInt32(textBox3.Text);
            int Y1 = Convert.ToInt32(textBox4.Text);

            int X2 = Convert.ToInt32(textBox5.Text);
            int Y2 = Convert.ToInt32(textBox6.Text);

            int X3 = Convert.ToInt32(textBox7.Text);
            int Y3 = Convert.ToInt32(textBox8.Text);

            int X4 = Convert.ToInt32(textBox9.Text);
            int Y4 = Convert.ToInt32(textBox10.Text); 

            zArrayPicture = File_Change_Size.Change_4(zArrayPicture, X1, X2, X3, X4, Y1, Y2, Y3, Y4);

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            regImage = 0;

            RadioButton rb = sender as RadioButton;

            if (rb == radioButton1) { regImage = 0; }
            if (rb == radioButton2) { regImage = 1; }
            if (rb == radioButton3) { regImage = 2; }
            if (rb == radioButton4) { regImage = 3; }
            if (rb == radioButton5) { regImage = 4; }
            if (rb == radioButton6) { regImage = 5; }
            if (rb == radioButton7) { regImage = 6; }
            if (rb == radioButton8) { regImage = 7; }
            if (rb == radioButton9) { regImage = 8; }
            if (rb == radioButton10) { regImage = 9; }
            if (rb == radioButton11) { regImage = 10; }
            if (rb == radioButton14) { regImage = 11; }

            if (rb == radioButton19) { regComplex = 0; }
            if (rb == radioButton20) { regComplex = 1; }
            if (rb == radioButton21) { regComplex = 2; }


            if (img[regImage] != null)
            {
                //imageWidth.Text = img[regImage].Width.ToString();
                //imageHeight.Text = img[regImage].Height.ToString();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     <-
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void button3_Click(object sender, EventArgs e)          // Стрелка влево
        {
            if (zArrayDescriptor[regImage] == null) return;
            applyScaleModeToPicturebox();


            int nx = zArrayDescriptor[regImage].width;
            int ny = zArrayDescriptor[regImage].height;
            zArrayPicture = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    zArrayPicture.array[i, j] = zArrayDescriptor[regImage].array[i, j];

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

            // currentScaleRatio = 1;
            applyScaleModeToPicturebox();
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     ->
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button12_Click(object sender, EventArgs e)  // Стрелка вправо
        {
            if (zArrayPicture == null) return;

            img[regImage] = pictureBox01.Image;

            int nx = zArrayPicture.width;
            int ny = zArrayPicture.height;

            zArrayDescriptor[regImage] = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    zArrayDescriptor[regImage].array[i, j] = zArrayPicture.array[i, j];

            Vizual_regImage(regImage);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ZGR_File(int i)
        {
            zArrayDescriptor[i] = File_Helper.loadImage();
            Vizual_regImage(i);
            //catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }					
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) { ZGR_File(0); }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e) { ZGR_File(1); }
        private void pictureBox3_MouseClick_1(object sender, MouseEventArgs e) { ZGR_File(2); }
        private void pictureBox4_MouseClick(object sender, MouseEventArgs e) { ZGR_File(3); }
        private void pictureBox5_MouseClick(object sender, MouseEventArgs e) { ZGR_File(4); }
        private void pictureBox6_MouseClick(object sender, MouseEventArgs e) { ZGR_File(5); }
        private void pictureBox7_MouseClick(object sender, MouseEventArgs e) { ZGR_File(6); }
        private void pictureBox8_MouseClick(object sender, MouseEventArgs e) { ZGR_File(7); }
        private void pictureBox9_MouseClick(object sender, MouseEventArgs e) { ZGR_File(8); }
        private void pictureBox10_MouseClick(object sender, MouseEventArgs e) { ZGR_File(9); }
        private void pictureBox11_MouseClick(object sender, MouseEventArgs e) { ZGR_File(10); }
        private void pictureBox12_MouseClick(object sender, MouseEventArgs e) { ZGR_File(11); }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* private void ZGRToolStripMenuItem_Click(object sender, EventArgs e)
         {
             var dialog1 = new OpenFileDialog();
             dialog1.InitialDirectory = string_dialog;

             if (dialog1.ShowDialog() == DialogResult.OK)
             {
                 try
                 {                    
                     dialog1.InitialDirectory = dialog1.FileName;
                     string_dialog = dialog1.FileName;

                     pictureBox01.Image = Image.FromFile(dialog1.FileName);

                     int w1 = pictureBox01.Image.Width;
                     int h1 = pictureBox01.Image.Height;
                     pictureBox01.Size = new Size(w1, h1);
                     pictureBox01.Show();
                                                                         // Вывод размера
                 }
                 catch (Exception ex) { MessageBox.Show("Ошибка " + ex.Message); }
             }
         }
         * */

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //               Отображение окна от 0 до 11
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Vizual_regImage(int k)
        {
            switch (k)
            {
                case 0: Vizual.Vizual_Picture(zArrayDescriptor[0], pictureBox1); break;
                case 1: Vizual.Vizual_Picture(zArrayDescriptor[1], pictureBox2); break;
                case 2: Vizual.Vizual_Picture(zArrayDescriptor[2], pictureBox3); break;
                case 3: Vizual.Vizual_Picture(zArrayDescriptor[3], pictureBox4); break;
                case 4: Vizual.Vizual_Picture(zArrayDescriptor[4], pictureBox5); break;
                case 5: Vizual.Vizual_Picture(zArrayDescriptor[5], pictureBox6); break;
                case 6: Vizual.Vizual_Picture(zArrayDescriptor[6], pictureBox7); break;
                case 7: Vizual.Vizual_Picture(zArrayDescriptor[7], pictureBox8); break;
                case 8: Vizual.Vizual_Picture(zArrayDescriptor[8], pictureBox9); break;
                case 9: Vizual.Vizual_Picture(zArrayDescriptor[9], pictureBox10); break;
                case 10: Vizual.Vizual_Picture(zArrayDescriptor[10], pictureBox11); break;
                case 11: Vizual.Vizual_Picture(zArrayDescriptor[11], pictureBox12); break;
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------------
        //                                        Ввод-вывод
        //--------------------------------------------------------------------------------------------------------------------------------------
        private void ZGRToolStripMenuItem_Click(object sender, EventArgs e)   // Загрузить файл в pictureBox01 и ZArrayDescriptor zArrayPicture
        {
            zArrayPicture = File_Helper.loadImage();
            if (zArrayPicture != null) Vizual.Vizual_Picture(zArrayPicture, pictureBox01);     // Отображение на pictureBox01
                                                                                               //zArrayPicture.Double_Picture(pictureBox01);                                  // Отображение на pictureBox01
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SAVEToolStripMenuItem_Click(object sender, EventArgs e)  // Сохранить файл из pictureBox01 в графический файл
        {
            File_Helper.saveImage(pictureBox01);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void сохранитьZaarayВФайлToolStripMenuItem_Click(object sender, EventArgs e)        //   Сохранить Zarray в файл (double)
        {
            File_Helper.saveZArray(zArrayPicture);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void загрузитьИзФайлаdoubleToolStripMenuItem_Click(object sender, EventArgs e)       //  Загрузить из файла (double  Zarray)
        {
            zArrayPicture = File_Helper.loadZArray();
            if (zArrayPicture != null) Vizual.Vizual_Picture(zArrayPicture, pictureBox01);           // Отображение на pictureBox01
        }

        private void загрузитьComplexToolStripMenuItem_Click(object sender, EventArgs e)             // Загрузить в ZComplex[regComplex] из файла
        {
            zComplex[regComplex] = File_Helper.loadZComplex();
            Complex_pictureBox(regComplex);

        }

        private void сщхранитьComplexZComplexToolStripMenuItem_Click(object sender, EventArgs e)         // Сохранить ZComplex[regComplex] в файл 
        {
            File_Helper.saveZComplex(zComplex[regComplex]);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string SaveString(string string_dialog, int k)
        {

            string strk = k.ToString();

            string string_rab = string_dialog;

            if (string_dialog.Contains("11.")) { string_rab = string_dialog.Replace("11.", "12."); return string_rab; }
            if (string_dialog.Contains("10.")) { string_rab = string_dialog.Replace("10.", "11."); return string_rab; }
            if (string_dialog.Contains("9.")) { string_rab = string_dialog.Replace("9.", "10."); return string_rab; }


            //MessageBox.Show("SaveString  ERROR - Первый файл должен оканчиваться на 9" + string_dialog + " string_rab - " + string_rab);

            return null;

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ZGR_File(string str, int i)   // Перегруженный ZGR_File(int i)
        {
            zArrayDescriptor[i] = File_Helper.loadImage_from_File(str);
            Vizual_regImage(i);
            //catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }					
        }

        private void загрузить418ToolStripMenuItem_Click(object sender, EventArgs e)  // Загрузить 4 файла в 9,10,11,12
        {
            var dialog1 = new OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;

                    progressBar1.Visible = true;
                    progressBar1.Minimum = 1;
                    progressBar1.Maximum = 5;
                    progressBar1.Value = 1;
                    progressBar1.Step = 1;

                    for (int i = 8; i < 12; i++)
                    {
                        ZGR_File(str, i);
                        str = SaveString(str, i); //if (str == null) break;  // Неправильное имя файла
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        progressBar1.PerformStep();
                    }

                    progressBar1.Value = 1;
                }
                catch (Exception ex) { MessageBox.Show("загрузить418ToolStripMenuItem_Click Ошибка " + ex.Message); }
            }
        }

        private void загрузить4912ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog1 = new OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;

                    progressBar1.Visible = true;
                    progressBar1.Minimum = 1;
                    progressBar1.Maximum = 5;
                    progressBar1.Value = 1;
                    progressBar1.Step = 1;

                    for (int i = 4; i < 8; i++)
                    {
                        ZGR_File(str, i);
                        str = SaveString(str, i); //if (str == null) break;  // Неправильное имя файла
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        progressBar1.PerformStep();
                    }

                    progressBar1.Value = 1;
                }
                catch (Exception ex) { MessageBox.Show("загрузить418ToolStripMenuItem_Click Ошибка " + ex.Message); }
            }
        }


        private void Save8ToolStripMenuItem_Click(object sender, EventArgs e)   // Загрузить 8 файлов в 0,1,2,3
        {
            var dialog1 = new OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;
                    for (int i = 0; i < 8; i++)
                    {
                        str = SaveString(string_dialog, i + 1);
                        switch (i)
                        {
                            case 0: pictureBox1.Image = Image.FromFile(str); pictureBox1.Invalidate(); img[0] = pictureBox1.Image; break;
                            case 1: pictureBox2.Image = Image.FromFile(str); pictureBox2.Invalidate(); img[1] = pictureBox2.Image; break;
                            case 2: pictureBox3.Image = Image.FromFile(str); pictureBox3.Invalidate(); img[2] = pictureBox3.Image; break;
                            case 3: pictureBox4.Image = Image.FromFile(str); pictureBox4.Invalidate(); img[3] = pictureBox4.Image; break;
                            case 4: pictureBox5.Image = Image.FromFile(str); pictureBox5.Invalidate(); img[4] = pictureBox5.Image; break;
                            case 5: pictureBox6.Image = Image.FromFile(str); pictureBox6.Invalidate(); img[5] = pictureBox6.Image; break;
                            case 6: pictureBox7.Image = Image.FromFile(str); pictureBox7.Invalidate(); img[6] = pictureBox7.Image; break;
                            case 7: pictureBox8.Image = Image.FromFile(str); pictureBox8.Invalidate(); img[7] = pictureBox8.Image; break;
                        }
                    }

                    if (img[regImage] != null)
                    {
                        //imageWidth.Text = img[regImage].Width.ToString();
                        //imageHeight.Text = img[regImage].Height.ToString();
                    }



                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if ((pictureBox1.Image.Size.Equals(pictureBox2.Image.Size))
             && (pictureBox1.Image.Size.Equals(pictureBox3.Image.Size))
             && (pictureBox1.Image.Size.Equals(pictureBox4.Image.Size))
             && (pictureBox1.Image.Size.Equals(pictureBox5.Image.Size))
             && (pictureBox1.Image.Size.Equals(pictureBox6.Image.Size))
             && (pictureBox1.Image.Size.Equals(pictureBox7.Image.Size))
             && (pictureBox1.Image.Size.Equals(pictureBox8.Image.Size)))
                    {
                        StretchImageForm newForm = new StretchImageForm();
                        newForm.initialSize = pictureBox1.Image.Size;
                        newForm.userChoosedSize += userChoosedSize;
                        newForm.Show();
                    }
                }
                catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     private void загрузить49ю12ToolStripMenuItem_Click(object sender, EventArgs e)
        //     {

        //     }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void userChoosedSize(Size newSize)
        {
            for (int i = 0; i < 8; i++)
            {
                img[i] = ImageProcessor.ResizeBitmap((Bitmap)img[i], newSize.Width, newSize.Height);

                switch (i)
                {
                    case 0: pictureBox1.Image = img[i]; pictureBox1.Invalidate(); break;
                    case 1: pictureBox2.Image = img[i]; pictureBox2.Invalidate(); break;
                    case 2: pictureBox3.Image = img[i]; pictureBox3.Invalidate(); break;
                    case 3: pictureBox4.Image = img[i]; pictureBox4.Invalidate(); break;
                    case 4: pictureBox5.Image = img[i]; pictureBox5.Invalidate(); break;
                    case 5: pictureBox6.Image = img[i]; pictureBox6.Invalidate(); break;
                    case 6: pictureBox7.Image = img[i]; pictureBox7.Invalidate(); break;
                    case 7: pictureBox8.Image = img[i]; pictureBox8.Invalidate(); break;
                }
            }

            if (img[regImage] != null)
            {
                //imageWidth.Text = img[regImage].Width.ToString();
                //imageHeight.Text = img[regImage].Height.ToString();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void сохранить8КадровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog1 = new SaveFileDialog();
            dialog1.InitialDirectory = string_dialog;
            dialog1.Filter = "Bitmap(*.bmp)|*.bmp";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap newBitmap = new Bitmap(pictureBox1.Image);
                    newBitmap.Save(dialog1.FileName + "1.bmp");
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;

                    newBitmap = new Bitmap(pictureBox2.Image);
                    newBitmap.Save(dialog1.FileName + "2.bmp");

                    newBitmap = new Bitmap(pictureBox3.Image);
                    newBitmap.Save(dialog1.FileName + "3.bmp");

                    newBitmap = new Bitmap(pictureBox4.Image);
                    newBitmap.Save(dialog1.FileName + "4.bmp");

                    newBitmap = new Bitmap(pictureBox5.Image);
                    newBitmap.Save(dialog1.FileName + "5.bmp");

                    newBitmap = new Bitmap(pictureBox6.Image);
                    newBitmap.Save(dialog1.FileName + "6.bmp");

                    newBitmap = new Bitmap(pictureBox7.Image);
                    newBitmap.Save(dialog1.FileName + "7.bmp");

                    newBitmap = new Bitmap(pictureBox8.Image);
                    newBitmap.Save(dialog1.FileName + "8.bmp");

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Ошибка при записи файла " + ex.Message);
                }
            }
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 


        private void сохранить4Кадра9101112ToolStripMenuItem_Click(object sender, EventArgs e)  // Сохранить 4 файла из 8,9,10,11 с добавлением цифр 9,10,11,12
        {
            var dialog1 = new SaveFileDialog();
            dialog1.InitialDirectory = string_dialog;
            dialog1.Filter = "Bitmap(*.bmp)|*.bmp";
            string str1;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;

                    str1 = str.Replace(".", "9.");
                    Bitmap newBitmap = new Bitmap(pictureBox9.Image);
                    newBitmap.Save(str1, System.Drawing.Imaging.ImageFormat.Bmp);


                    str1 = str.Replace(".", "10.");
                    newBitmap = new Bitmap(pictureBox10.Image);
                    newBitmap.Save(str1, System.Drawing.Imaging.ImageFormat.Bmp);


                    str1 = str.Replace(".", "11.");
                    newBitmap = new Bitmap(pictureBox11.Image);
                    newBitmap.Save(str1, System.Drawing.Imaging.ImageFormat.Bmp);


                    str1 = str.Replace(".", "12.");
                    newBitmap = new Bitmap(pictureBox12.Image);
                    newBitmap.Save(str1, System.Drawing.Imaging.ImageFormat.Bmp);


                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception ex) { MessageBox.Show(" Ошибка при записи файла " + ex.Message); }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageProcessed(Bitmap newImage)
        {
            FazaClass.imageProcessed -= imageProcessed;
            SetControlPropertyThreadSafe(pictureBox01, "Image", newImage);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void captureImage(object sender, EventArgs e)
        {
            // ShooterSingleton.imageCaptured += imageTaken;
            // ShooterSingleton.getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Cadr8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackgroundImagesGeneratorForm newForm = new BackgroundImagesGeneratorForm();
            newForm.oneImageOfSeries += oneImageCaptured;
            newForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void oneImageCaptured(Image newImage, int imageNumber)
        {
            if (imageNumber == 1) { pictureBox1.Image = newImage; }
            else if (imageNumber == 2) { pictureBox2.Image = newImage; }
            else if (imageNumber == 3) { pictureBox3.Image = newImage; }
            else if (imageNumber == 4) { pictureBox4.Image = newImage; }
            else if (imageNumber == 5) { pictureBox5.Image = newImage; }
            else if (imageNumber == 6) { pictureBox6.Image = newImage; }
            else if (imageNumber == 7) { pictureBox7.Image = newImage; }
            else if (imageNumber == 8) { pictureBox8.Image = newImage; }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* private void radioButton10_CheckedChanged(object sender, EventArgs e)
         {
             scaleMode = 0;
             radioButton12.Checked = true;
             radioButton13.Checked = false;
             applyScaleModeToPicturebox();

             if (radioButton103.Checked == true)
             {
                 cursorMode = 1;
             }
         }
         /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
         private void radioButton11_CheckedChanged(object sender, EventArgs e)
         {
             if (radioButton11.Checked == true)
             {
                 cursorMode = 2;
             }
         }
         * */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox01_MouseDown(object sender, MouseEventArgs e)
        {
            if (cursorMode == 1) { downPoint = new Point(e.X, e.Y); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void relayout()
        {
            //this.panel1.Size = new Size(this.Size.Width - 160, this.Size.Height - 150);
            this.pictureBox01.Size = new Size(this.panel1.Size.Width - 44, this.panel1.Size.Height - 36);
            //this.panel1.Size = new Size(this.Size.Width - 160, this.Size.Height - 150);
            //this.pictureBox01.Size = new Size(800, 600);

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton12_CheckedChanged(object sender, EventArgs e)    // Реальный
        {
            scaleMode = 0;
            applyScaleModeToPicturebox();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton13_CheckedChanged(object sender, EventArgs e)     // Подогнанный (все изображение в окне)
        {
            scaleMode = 1;
            applyScaleModeToPicturebox();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void applyScaleModeToPicturebox()
        {
            if (scaleMode == 0) { pictureBox01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize; }
            else
                  if (scaleMode == 1) { pictureBox01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; }

            relayout();
            pictureBox01.Invalidate();
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void restoderImageReceived(Bitmap newBitmap, double ratio)
        {
            pictureBox10.Image = newBitmap;
            pictureBox10.Invalidate();
            pictureBox10.Update();

            afterRemovingScaleRatio = ratio;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void phaseImageReceived(Bitmap newBitmap, double ratio)
        {
            pictureBox9.Image = newBitmap;
            pictureBox9.Invalidate();
            pictureBox9.Update();

            initialScaleRatio = ratio;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ------------------------------  Сглаживание
        private void сглаживаниеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_Click_SM);
        }
        private void filt_Click_SM(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_smothingSM(amp, k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }
        // ------------------------------  Удаление низких частот
        private void сглаживаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_Click);
        }
        private void filt_Click(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_smothing(amp, k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }
        // -------------------------------  Медианный
        private void медианныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_median_Click);
        }
        private void filt_median_Click(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_Mediana(amp, k_filt);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }
        // -------------------------------  Рамка
        private void рамкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_Ramka);
        }
        private void filt_Ramka(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_Ramka(amp, k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }

        // -------------------------------  Взвешенный фильтр для голографии
        private void взвешенныйФильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_Hologramm(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        // -------------------------------  Усреднение 2х2 точек с уменьшением размера файла
        private void усреднение2х2ТочекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_2х2(amp, 0, 0);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        // -------------------------------  Уменьшением размера файла в два раза
        private void уменьшениеМассиваВ2РазаToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_1_2(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }
        // -------------------------------  Сложение всех строк
        private void сложениеВсехСтрокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Sum_Line(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        // -------------------------------  Если выше или ниже границы => ноль
        private void неВГраницах0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double s1 = Convert.ToDouble(textBox1.Text);
            double s2 = Convert.ToDouble(textBox2.Text);
            int w1 = zArrayPicture.width;
            int h1 = zArrayPicture.height; 

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                {
                    if (zArrayPicture.array[i, j] >= s1 || zArrayPicture.array[i, j] <= s2) zArrayPicture.array[i, j]=0;
                }
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);        

        }

        // -------------------------------  
        // -------------------------------  Сверхразрешение
        // ----- Разряжение массива нулями
        private void разряжениеМассиваНулямиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.Decim0);
        }     
        private void Decim0(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);

            
            zArrayPicture = FiltrClass.Decim(zArrayDescriptor[regImage],k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }


        private void добвлениеМассиваПовторениемЗначенийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.Decim1);
        }

        private void Decim1(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);


            zArrayPicture = FiltrClass.Decim1(zArrayDescriptor[regImage], k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }


        // ----- 4 новых файла в два раза меньше в 1,2,3,4 
        private void массиваВ1234СоСдвигомНа12ПикселяToolStripMenuItem_Click(object sender, EventArgs e)
        // со cдвигом на половину пикселя
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);                                   // Из главного окна
            
            zArrayDescriptor[0] = FiltrClass.Filt_2х2(amp, 0, 0);
            zArrayDescriptor[1] = FiltrClass.Filt_2х2(amp, 1, 0);
            zArrayDescriptor[2] = FiltrClass.Filt_2х2(amp, 0, 1);
            zArrayDescriptor[3] = FiltrClass.Filt_2х2(amp, 1, 1);
            Vizual_regImage(0); Vizual_regImage(1); Vizual_regImage(2); Vizual_regImage(3);             // Отображение
        }

        // ----- 4 новых файла в четыре раза меньше в 1,2,3,4 
        private void массиваВ1234СоСдвигомНа12ПикселяToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);                                   // Из главного окна
            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = w1 / 4;
            int h2 = h1 / 4;
            //ZArrayDescriptor tmp = new ZArrayDescriptor(w2, h2);
            //tmp = FiltrClass.Filt_2х2(amp, 0, 0); zArrayDescriptor[0]=tmp;
            zArrayDescriptor[0] = FiltrClass.Filt_4х4(amp, 0, 0);
            zArrayDescriptor[1] = FiltrClass.Filt_4х4(amp, 1, 0);
            zArrayDescriptor[2] = FiltrClass.Filt_4х4(amp, 0, 1);
            zArrayDescriptor[3] = FiltrClass.Filt_4х4(amp, 1, 1);
            Vizual_regImage(0); Vizual_regImage(1); Vizual_regImage(2); Vizual_regImage(3);
        }


        private void из1234НовыйМассивToolStripMenuItem_Click(object sender, EventArgs e)               // ----- Из  zArrayDescriptor[0] 1,2,3 в основное окно файл в 2 раза больший 
        {
            int w2 = zArrayDescriptor[0].width;
            int h2 = zArrayDescriptor[0].height;
            int w1 = w2 * 2;
            int h1 = h2 * 2;
            //ZArrayDescriptor tmp = new ZArrayDescriptor(w2, h2);
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);
            /*
            tmp = zArrayDescriptor[0];  for (int i = 0; i < w2; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2, j * 2] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[1];  for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2 + 1, j * 2] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[2];  for (int i = 0; i < w2; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2, j * 2 + 1] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[3];  for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2 + 1, j * 2 + 1] = tmp.array[i, j]; }
              */


            for (int i = 0; i < w2; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2, j * 2] = zArrayDescriptor[0].array[i, j]; }
            for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2 + 1, j * 2] = zArrayDescriptor[1].array[i, j]; }
            for (int i = 0; i < w2; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2, j * 2 + 1] = zArrayDescriptor[2].array[i, j]; }
            for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2 + 1, j * 2 + 1] = zArrayDescriptor[3].array[i, j]; }

            zArrayPicture = res_array;

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        private void из12НовыйМассивToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int w2 = zArrayDescriptor[0].width;
            int h2 = zArrayDescriptor[0].height;
            int w1 = w2 * 2;
            int h1 = h2;
            //ZArrayDescriptor tmp = new ZArrayDescriptor(w2, h2);
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);
            /*
            tmp = zArrayDescriptor[0];  for (int i = 0; i < w2; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2, j * 2] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[1];  for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2 + 1, j * 2] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[2];  for (int i = 0; i < w2; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2, j * 2 + 1] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[3];  for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2 + 1, j * 2 + 1] = tmp.array[i, j]; }
              */


            for (int i = 0; i < w2; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2, j] = zArrayDescriptor[0].array[i, j]; }
            for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2 + 1, j] = zArrayDescriptor[1].array[i, j]; }


            zArrayPicture = res_array;

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        private void генерацияМассиваСоСдвигомНаПловинуПикселяToolStripMenuItem_Click(object sender, EventArgs e) // -----  Генерация файла со сдвигом на половину пикселя
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.ADD_razresh_2х2(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        // -------------------------------  Увеличение массива в два раза простым повторением
 //       private void увеличениеМассиваВ2РазаПростымПовторениемToolStripMenuItem_Click(object sender, EventArgs e)
 //       {
 //           ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            //zArrayPicture = FiltrClass.Filt_2_1(amp);
 //           zArrayPicture = FiltrClass.Filt_2_1_s(amp);
//            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
//        }
        // -------------------------------  Увеличение массива в два раза со сдвигом на пловину пикселя
        private void увеличениеМассиваВ2РазаПростымПовторениемToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);

            zArrayPicture = FiltrClass.Filt_2_1_2(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        //-------------------------------------------------------------------------------
        public void FiltDialog(EventHandler functionPointer)
        {
            int max_x = 120, max_y = 100;

            f_filt = new Form();
            f_filt.Size = new Size(max_x, max_y + 36);
            f_filt.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(40, 165);
            f_filt.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(8, 10);
            label1.Text = "k = 1,2,3 ... :";

            tb1_filt = new TextBox();
            tb1_filt.Location = new System.Drawing.Point(8, 30);
            tb1_filt.Size = new System.Drawing.Size(60, 20);
            tb1_filt.Text = k_filt.ToString();

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 60);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(100, 30);
            b1.Click += new System.EventHandler(functionPointer);

            f_filt.Controls.Add(label1);
            f_filt.Controls.Add(tb1_filt);
            f_filt.Controls.Add(b1);

            f_filt.Show();

        }

        //                              Инверсия цветов

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zArrayPicture = SumClass.Inversia(zArrayPicture);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     ->    Complex ->  Re, Im. Am, Ph
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void button1_Click(object sender, EventArgs e)
        {
            Complex_pictureBox(regComplex);
        }



        /// <summary>
        ///                       Отображение комплексных чисел Complex(int regComplex)
        /// </summary>
        /// <param name="regComplex"></param>
        private void Complex_pictureBox(int regComplex)
        {
            if (zComplex[regComplex] == null) { MessageBox.Show("Complex_pictureBox:  zComplex[regComplex] == NULL"); return; }

            PictureBox pictureB00, pictureB01, pictureB02, pictureB03;


            int regCmplx = 0;
            switch (regComplex)
            {
                case 0: regCmplx = 0; pictureB00 = pictureBox1; pictureB01 = pictureBox2; pictureB02 = pictureBox3; pictureB03 = pictureBox4; break;
                case 1: regCmplx = 4; pictureB00 = pictureBox5; pictureB01 = pictureBox6; pictureB02 = pictureBox7; pictureB03 = pictureBox8; break;
                case 2: regCmplx = 8; pictureB00 = pictureBox9; pictureB01 = pictureBox10; pictureB02 = pictureBox11; pictureB03 = pictureBox12; break;
                default: regCmplx = 0; pictureB00 = pictureBox1; pictureB01 = pictureBox2; pictureB02 = pictureBox3; pictureB03 = pictureBox4; break;
            }

            int width = zComplex[regComplex].width;
            int height = zComplex[regComplex].height;
            double[,] Image_double = new double[width, height];

            //MessageBox.Show("regComplex " + Convert.ToString(regComplex) + "width " + Convert.ToString(width) + "height " + Convert.ToString(height));

            Image_double = Furie.Re(zComplex[regComplex].array);
            zArrayDescriptor[regCmplx] = new ZArrayDescriptor(Image_double);
            //zArrayDescriptor[regCmplx].Double_Picture(pictureB00);   // Double_Picture  в ZArrayDescriptor
            Vizual.Vizual_Picture(zArrayDescriptor[regCmplx], pictureB00);

            Image_double = Furie.Im(zComplex[regComplex].array);
            zArrayDescriptor[regCmplx + 1] = new ZArrayDescriptor(Image_double);
            //zArrayDescriptor[regCmplx + 1].Double_Picture(pictureB01);
            Vizual.Vizual_Picture(zArrayDescriptor[regCmplx + 1], pictureB01);

            Image_double = Furie.Amplituda(zComplex[regComplex].array);
            zArrayDescriptor[regCmplx + 2] = new ZArrayDescriptor(Image_double);
            //zArrayDescriptor[regCmplx + 2].Double_Picture(pictureB02);
            Vizual.Vizual_Picture(zArrayDescriptor[regCmplx + 2], pictureB02);

            Image_double = Furie.Faza(zComplex[regComplex].array);
            zArrayDescriptor[regCmplx + 3] = new ZArrayDescriptor(Image_double);
            //zArrayDescriptor[regCmplx + 3].Double_Picture(pictureB03);
            Vizual.Vizual_Picture(zArrayDescriptor[regCmplx + 3], pictureB03);

        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //              Заполнение массива комплексных чисел  ( файл - Complex_form.cs)
        //   
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button2_Click(object sender, EventArgs e)  // Complex_form.cs
        {
            Complex_form BoxForm = new Complex_form();
            BoxForm.OnComplex += FormComplex;                           // фазу по центру амплитуда прежняя
            BoxForm.OnComplex0 += FormComplex0;
            BoxForm.OnComplexPhase += FormComplexPhase;
            BoxForm.OnComplexPhase_Random += FormComplexPhase_Random;  // Добавление к фазе случайных значений
            BoxForm.OnNull += FormNull;
            BoxForm.OnRe += FormRe;
            BoxForm.OnIm += FormIm;
            BoxForm.OnReIm0 += FormReIm0;
            BoxForm.OnCicle += FormCicle;
            BoxForm.OnAmpl += FormAmpl;                                // Меняем амплитуду (фаза прежняя)
            BoxForm.OnComplexNew += FormComplexNew;
            BoxForm.Show();
        }


        private void FormComplexNew()   // Создание нового Complex[regComplex]   Амплитуду из zArrayPicture (фаза нулевая) 
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL  (FormComplexNew)"); return; }

            int Nx = zArrayPicture.width;
            int Ny = zArrayPicture.height;

            zComplex[regComplex] = new ZComplexDescriptor(Nx, Ny);

            zComplex[regComplex] = new ZComplexDescriptor(zComplex[regComplex], zArrayPicture); // амплитуда = zComplex[regComplex], фаза - (прежняя) нулевая
            Complex_pictureBox(regComplex);
        }

        private void FormAmpl(int Nx, int Ny)   // В Complex[regComplex]   Амплитуду (фаза прежняя) ---------------zComplex------------------
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }

            Nx = zComplex[regComplex].width;
            Ny = zComplex[regComplex].height;

            ZArrayDescriptor ampl = new ZArrayDescriptor(Nx, Ny, zArrayPicture);                      //  в центре по краям 127, eсли не хватает    
            zComplex[regComplex] = new ZComplexDescriptor(zComplex[regComplex], ampl);
            Complex_pictureBox(regComplex);
        }



        private void FormComplex(double am, int Nx, int Ny)   // В Complex[regComplex]   фазу  по центру амплитуда прежняя
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL (Main фазу по центру амплитуда прежняя)"); return; }
            if (zComplex[regComplex] == null) { MessageBox.Show("zArrayPicture == NULL (Main фазу по центру амплитуда прежняя)"); return; }

            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);
            Nx = zComplex[regComplex].width;
            Ny = zComplex[regComplex].height;

            ZArrayDescriptor fz = new ZArrayDescriptor(Nx, Ny, zArrayPicture);                      // Фаза в центре по краям 127
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[regComplex * 4 + 2]);      // Амплитуда


            double max = SumClass.getMax(fz);
            double min = SumClass.getMin(fz);

            for (int i=0; i< Nx; i++)
                for (int j = 0; j < Ny; j++)
                {
                    fz.array[i, j] = (fz.array[i, j]-min) *2* Math.PI /(max-min) - Math.PI; 
                }




                    //MessageBox.Show("regComplex " + Convert.ToString(regComplex));
                    zComplex[regComplex] = new ZComplexDescriptor(fz, amp);                                // Конструктор

            Complex_pictureBox(regComplex);
        }
        //
        private void FormComplexPhase_Random()    //  В Complex[regComplex]  к существующей фазе добавляются случайные значения 
                                                  //  Амплитуда прежняя.     Шаблон в zArrayPicture.
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL  FormComplexPhase_Random"); return; }

            int Nx = zArrayDescriptor[regComplex * 4 + 2].width;
            int Ny = zArrayDescriptor[regComplex * 4 + 2].height;
            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture, Nx, Ny);                     // Фаза в центре (шаблон)
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[regComplex * 4 + 2]);      // Амплитуда прежняя

            zComplex[regComplex] = new ZComplexDescriptor(fz, amp, 1);

            Complex_pictureBox(regComplex);
        }


        private void FormComplex0(double am, int Nx, int Ny)   // В Complex[regComplex]   фазу  в левый угол
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }

            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);

            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture, Nx, Ny, 1);
            //MessageBox.Show("regComplex " + Convert.ToString(regComplex));

            zComplex[regComplex] = new ZComplexDescriptor(fz, am);
            //Vizual_regImage(regComplex);
            Complex_pictureBox(regComplex);
        }

        private void FormComplexPhase()   // В Complex[regComplex]   фазу  в левый угол амплитуда прежняя
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            if (zComplex[regComplex] == null) { MessageBox.Show("Complex[regComplex] == NULL"); return; }

            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);

            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[regComplex * 4 + 2]);      // Амплитуда

            zComplex[regComplex] = new ZComplexDescriptor(fz, amp);
            Complex_pictureBox(regComplex);
            MessageBox.Show("regComplex " + Convert.ToString(regComplex));
        }


        private void FormNull(int Nx, int Ny)                             // В Complex[regComplex] заносим нулевые значения
        {
            zComplex[regComplex] = new ZComplexDescriptor(Nx, Ny);
            Complex_pictureBox(regComplex);
        }
        /// <summary>
        ///  Меняем реальную часть комплексного массива zComplex[regComplex] значениями из центрального окнаz ArrayPicture
        /// </summary>
        /// <param name="Nx"></param>
        /// <param name="Ny"></param>
        private void FormRe(int Nx, int Ny)                               // В Complex[regComplex] заносим Re
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            if (zComplex[regComplex] == null) { MessageBox.Show("zComplex[" + regComplex + "] по центру == NULL"); return; }

            zComplex[regComplex] = new ZComplexDescriptor(zArrayPicture, zComplex[regComplex], 0);

            Complex_pictureBox(regComplex);
        }

        private void FormIm(int Nx, int Ny)                               // В Complex[regComplex] заносим Im
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);
            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture, Nx, Ny);
            zComplex[regComplex] = new ZComplexDescriptor(fz, zComplex[regComplex], 1);
            Complex_pictureBox(regComplex);
        }

        private void FormReIm0(int Nx, int Ny)                 // В Complex[regComplex] заносим Re Im=0 (В левый угол)
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);
            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture, Nx, Ny, 1);
            zComplex[regComplex] = new ZComplexDescriptor(fz);
            Complex_pictureBox(regComplex);
        }

        //  Циклический сдвиг ---------------------------------------------------------------------------

        private void FormCicle()   // В Complex[regComplex]   циклический сдвиг
        {

            if (zComplex[regComplex] == null) { MessageBox.Show("Complex[regComplex] == NULL"); return; }

            zComplex[regComplex] = Furie.Invers(zComplex[regComplex]);

            Complex_pictureBox(regComplex);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //              Арифметические операции над массивами  ADD_Cmplx.cs
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ADD_Cmplx ADDPLUS = new ADD_Cmplx();


            ADDPLUS.On_ADD += ADD_C;        // Сложить += два комплекных массива
            ADDPLUS.On_Send += Send_C;      // Переслать 
            ADDPLUS.On_Sub += Sub_C;        // Вычесть
            ADDPLUS.On_Add += Add_C;        // Сложить
            ADDPLUS.On_Div += Div_C;        // Разделить
            ADDPLUS.On_Mul += Mul_C;        // Умножить



            ADDPLUS.On_ROR_CMPLX += ROR_C;      // Cдвиг вправо  (циклический)
            ADDPLUS.On_ROL_CMPLX += ROL_C;      // Cдвиг влево  (циклический)


            ADDPLUS.On_ADD_Double += ADD_D;     // Сложить два вещественных массива
            ADDPLUS.On_Sub_Double += Sub_D;     // Вычесть
            ADDPLUS.On_Div_Double += Div_D;     // Разделить
            ADDPLUS.On_MulD   += Mul_D;         // Умножение вещественных массивов
            ADDPLUS.On_ROR    += ROR_D;         // Cдвиг вправо (циклический)
            ADDPLUS.On_ROL    += ROL_D;         // Cдвиг влево  (циклический)
            ADDPLUS.On_Conv   += Conv_D;        // Свертка двух массивов
            ADDPLUS.On_TRNS   += TRNS_D;        // Транспонирование вещественных массивов
            ADDPLUS.On_ROT180 += ROT180_D;      // Транспонирование вещественных массивов

            ADDPLUS.On_Pirs += ADD_Math.Pirs_D;          // Линейный коэффициент корреляции r-Пирсона
            ADDPLUS.On_ABS  += ABS_D;           // Абсолютное значение

            ADDPLUS.Show();
        }
        // Сами программы формы ADD_Cmplx находятся в  ADD_Math.cs - Арифметические операции над массивами

        private void ABS_D() { ADD_Math.ABS_D(regImage); Vizual_regImage(regImage); }                   // Абсолютное значение
        private void ROR_D(int k1) { ADD_Math.ROR_D(k1); Vizual_regImage(regImage); }                   // Циклический сдвиг вправо zArrayDescriptor[regImage]
        private void ROL_D(int k1) { ADD_Math.ROL_D(k1); Vizual_regImage(regImage); }                   // Циклический сдвиг влево zArrayDescriptor[regImage]
        private void ROL_C(int k1) { ADD_Math.ROL_C(k1); Complex_pictureBox(regComplex); }              // Циклический сдвиг влево комплексных чисел
        private void ROR_C(int k1) { ADD_Math.ROR_C(k1); Complex_pictureBox(regComplex); }              // Циклический сдвиг вправо  комплексных чисел
        private void TRNS_D() { ADD_Math.TRNS_D(); Vizual_regImage(regImage); }                         // Транспонирование zArrayDescriptor[regImage]
        private void ROT180_D() { ADD_Math.ROT180_D(); Vizual_regImage(regImage); }                     // Поворот zArrayDescriptor[regImage] на 180 градусов
        private void ADD_D(int k1, int k2, int k3) { ADD_Math.ADD_D(k1, k2, k3); Vizual_regImage(k3-1); } // Сложить два вещественных массива
        private void Sub_D(int k1, int k2, int k3) { ADD_Math.Sub_D1(k1, k2, k3); Vizual_regImage(k3-1); } // Вычесть два вещественных массива (3 аргумента)
        private void ADD_C(int k1, int k2) { ADD_Math.ADD_C(k1, k2); Complex_pictureBox(k2-1); }          // Накопление += комплексных массивов
        private void Send_C(int k1, int k2) { ADD_Math.Send_C(k1, k2); Complex_pictureBox(k2-1); }        // Пересылка комплексных массивов
        private void Add_C(int k3, int k4, int k5) { ADD_Math.Add_C(k3, k4, k5); Complex_pictureBox(k5-1); }   // Сложить два комплексных массива
        private void Sub_C(int k3, int k4, int k5) { ADD_Math.Sub_C(k3, k4, k5); Complex_pictureBox(k5-1); }   // Вычесть два комплексных массива

        private void Mul_C(int k3, int k4, int k5) { ADD_Math.Mul_C(k3, k4, k5, progressBar1); Complex_pictureBox(k5 - 1); } // Умножить два комплексных массива
        private void Mul_D(int k3, int k4, int k5) { ADD_Math.Mul_D(k3, k4, k5, progressBar1); Vizual_regImage(k5 - 1); } // Умножить два вещественных массивов
        private void Conv_D(int k3, int k4, int k5) { ADD_Math.Conv_D(k3, k4, k5, progressBar1); Vizual_regImage(k5 - 1); } // Корреляция двух вещественных массивов

        private void Div_C(int k3, int k4, int k5) { ADD_Math.Div_C(k3, k4, k5);  Complex_pictureBox(k5 - 1); } // Поэлементное деление комплексных чисел
        private void Div_D(int k3, int k4, int k5) { ADD_Math.Div_D(k3, k4, k5); Vizual_regImage(k5 - 1); }     // Поэлементное деление вещественных массивов


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //        Двухмерное преобразование Фурье и Френеля
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void фурьеToolStripMenuItem1_Click(object sender, EventArgs e)
        {   
            FrenelForm FrForm = new FrenelForm();

            FrForm.OnFrenel         += FrComplex;
            FrForm.OnFrenelN        += FrComplexN;              // Френель с четным количеством точек
            FrForm.OnFrenelN_CUDA   += FrComplexN_CUDA;         // Френель CUDA

            FrForm.OnFurie               += FurComplex;
            FrForm.OnFurieM              += FurComplexM;
            FrForm.OnFurie_N             += FurComplex_N;
            FrForm.OnFurie_2Line         += FurComplex_2Line;
            FrForm.OnFurie_CUDA          += FurComplex_CUDA;
            FrForm.OnFurie_CUDA_CMPLX    += FurComplex_CUDA1;   // Фурье CUDA из к1 в k2
            FrForm.OnFurie_NXY           += FurComplex_NXY;     // Фурье   с задаваемым количеством точек из k1 => k2
            FrForm.OnFrenel_NXY          += FrenelComplex_NXY;  // Френель с задаваемым количеством точек из k1 => k2
            FrForm.OnInterf_XY           += Frenel_XY;          // Интерферометрия двух сдвинутых фронтов

            FrForm.OnPSI_fast += FrPSI;                         // PSI + Фурье => Complex(k2) ------------------------
            FrForm.OnADD_PHASE += FrADD_PHASE;  // Фаза по пиле


            //FrForm.InversComplex += InversComplexM;
            FrForm.Show();
        }


        private void FrADD_PHASE(double step)   //  Фаза - пила
        {

            if (zComplex[regComplex] == null) { MessageBox.Show("zComplex[regComplex] == NULL  FrADD_PHASE"); return; }
           
          
          
            zComplex[regComplex] = Model_interf.ADD_PHASE(zComplex[regComplex], step);
          
            Complex_pictureBox(regComplex);
        }

        private void FrPSI(double[] fz, double xmax, double lambda, double d, int k2)    // быстрое PSI 8,9,10,11 -> zComplex[0]
        {
             if ( zArrayDescriptor[8]  == null)  { MessageBox.Show("FrPSI zComplex[8] == NULL");  return; }
             if ( zArrayDescriptor[9]  == null)  { MessageBox.Show("FrPSI zComplex[9] == NULL");  return; }
             if ( zArrayDescriptor[10] == null) { MessageBox.Show("FrPSI zComplex[10] == NULL"); return; }
             if ( zArrayDescriptor[11] == null) { MessageBox.Show("FrPSI zComplex[11] == NULL"); return; }
            
            double amplit = 160000;
            //zArrayPicture = ATAN_PSI.ATAN_quick(zArrayDescriptor,  progressBar1, fz, xmax, lambda, d, amplit);
            //zComplex[1] = ATAN_PSI.ATAN_891011(zArrayDescriptor, progressBar1, fz, amplit);     // Параметр amplit  не используется
            //FrComplexN(xmax, lambda, d, 1, 0);
           
            
            zComplex[k2] = FurieN.FrenelTransformN(ATAN_PSI.ATAN_8_11(8, 9, 10, 11, zArrayDescriptor,  fz, amplit), lambda, d, xmax);
            
            //MessageBox.Show("Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);
            Complex_pictureBox(k2);
           
           
        }



        private void FrComplex(double xmax, double lambda, double d, int k1, int k2)
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[0] == NULL"); return; }
            int m = 1;
            int n = zComplex[k1].width;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > n) { n = nn / 2; m = i; break; } }

            MessageBox.Show("n = " + Convert.ToString(n) + " m = " + Convert.ToString(m));

            zComplex[k2] = new ZComplexDescriptor(n, n);
            //MessageBox.Show("regComplex " + Convert.ToString(regComplex));
            zComplex[k2] = Furie.FrenelTransform(zComplex[k1], m, lambda, d, xmax);
            MessageBox.Show(" m= " + m + " Lambda= " + lambda + " d= " + d + " dx= " + xmax);
            Complex_pictureBox(k2);
        }

        private void FrComplexN(double xmax, double lambda, double d, int k1, int k2)           //----------- Френель с четным количеством точек
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[0] == NULL"); return; }
            int nx = zComplex[k1].width;
            int ny = zComplex[k1].height;

            zComplex[k2] = new ZComplexDescriptor(nx, ny);

            //ZComplexDescriptor zComplex_Picture = new ZComplexDescriptor(zArrayPicture);        // Re=zArrayPicture Im=0
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
                  zComplex[k2] = FurieN.FrenelTransformN(zComplex[k1], lambda, d, xmax);
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);

            //MessageBox.Show(" Lambda= " + lambda + " d= " + d + " dx= " + xmax);
            Complex_pictureBox(k2);
        }


        private void FrComplexN_CUDA(double xmax, double lambda, double d, int k1, int k2)       //---------- Френель CUDA
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[0] == NULL"); return; }
            int nx = zComplex[k1].width;
            int ny = zComplex[k1].height;

            zComplex[k2] = new ZComplexDescriptor(nx, ny);

           // ZComplexDescriptor zComplex_Picture = new ZComplexDescriptor(zArrayPicture);        // Re=zArrayPicture Im=0
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
            zComplex[k2] = CUDA_FFT.Fr_CUDA(zComplex[k1], lambda, d, xmax); 
            
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("Время Френель CUDA  Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);

            //MessageBox.Show(" m= " + m + " Lambda= " + lambda + " d= " + d + " dx= " + xmax);
            Complex_pictureBox(k2);
        }


        private void FurComplex_NXY(int k1, int k2, int X, int Y, int N)     // Прямое BPF с задаваемым количеством точек
        {
            if (zComplex[k1] == null) { MessageBox.Show("zComplex[" + k1 + "] == NULL (FurComplex_NXY)"); return; }
            if (zComplex[k1].width < N)  { MessageBox.Show(" zComplex.width[" + k1 + "] < " + N + "] (FurComplex_NXY)"); return; }
            if (zComplex[k1].height < N) { MessageBox.Show(" zComplex.height[" + k1 + "] < " + N + "] (FurComplex_NXY)"); return; }

            zComplex[k2] = new ZComplexDescriptor(N, N);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    zComplex[k2].array[i, j] = zComplex[k1].array[i+X, j+Y];
           
            zComplex[k2] = FurieN.BPF2(zComplex[k2]);                    // Фурье преобразование для произвольного количества точек
           
            Complex_pictureBox(k2);
        }

        private void FrenelComplex_NXY(int k1, int k2, int X, int Y, int N, double xmax, double lambda, double d)     // Френель с задаваемым количеством точек
        {
            if (zComplex[k1] == null)    { MessageBox.Show("zComplex[" + k1 + "] == NULL (FrenelComplex_NXY)"); return; }
            if (zComplex[k1].width < N)  { MessageBox.Show(" zComplex.width[" + k1 + "] < " + N + "] (FrenelComplex_NXY)"); return; }
            if (zComplex[k1].height < N) { MessageBox.Show(" zComplex.height[" + k1 + "] < " + N + "] (FrenelComplex_NXY)"); return; }

            ZComplexDescriptor z = new ZComplexDescriptor(N, N);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    z.array[i, j] = zComplex[k1].array[i + X, j + Y];

            //zComplex[k2] = FurieN.BPF2(zComplex[k2]);                    // Фурье преобразование для произвольного количества точек
            zComplex[k2] = FurieN.FrenelTransformN(z, lambda, d, xmax);

            Complex_pictureBox(k2);
        }



        private void Frenel_XY(double[] fz, double xmax, double lambda, double d, int X, int Y, int X1, int Y1, int N)     // Интерферометрия сдвинутых фронтов
        {
            Model_object.Interf_XY(zComplex, zArrayDescriptor, fz,  xmax,  lambda,  d, X,  Y,  X1,  Y1, N);
            Complex_pictureBox(0); 
            //Complex_pictureBox(1); Complex_pictureBox(2);
            //Vizual_regImage(4); Vizual_regImage(5); Vizual_regImage(6); Vizual_regImage(7);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
        }



        private void FurComplex_2Line(int k1, int k2)                  // Прямое Фурье преобразование для произвольного количества точек по строкам
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[" + k1 + "] == NULL (FurComplex)"); return; }
      
            zComplex[k2] = FurieN.BPF2_Line(zComplex[k1]);                 // Фурье преобразование для произвольного количества точек         
          
            Complex_pictureBox(k2);
        }

        private void FurComplex(int k1, int k2)                                           // Прямое Фурье преобразование для произвольного количества точек
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex["+ k1 + "] == NULL (FurComplex)"); return; }
            //int m = 1;
            //int n = zComplex[k1].width;
            //int nn = 2;
            //for (int i = 1; ; i++) { nn = nn * 2; if (nn > n) { n = nn / 2; m = i; break; } }
            //MessageBox.Show("n = " + Convert.ToString(n) + " m = " + Convert.ToString(m));

            //zComplex[k2] = new ZComplexDescriptor(n, n);

            //zComplex[k2] = Furie.FourierTransform(zComplex[k1], m);                        // Старое Фурье преобразование
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
                        zComplex[k2] = FurieN.BPF2(zComplex[k1]);                            // Фурье преобразование для произвольного количества точек
            sw.Stop();              
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);
            Complex_pictureBox(k2);
        }

        private void FurComplexM(int k1, int k2)  // Обратное преобразование
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[0] == NULL"); return; }
            int m = 1;
            int n = zComplex[k1].width;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > n) { n = nn / 2; m = i; break; } }

            MessageBox.Show("n = " + Convert.ToString(n) + " m = " + Convert.ToString(m));

            zComplex[k2] = new ZComplexDescriptor(n, n);

            zComplex[k2] = Furie.InverseFourierTransform(zComplex[k1], m);

            Complex_pictureBox(k2);
        }

        private void FurComplex_N()                                            // Прямое преобразование Фурье с четным числом точек
        {
            //MessageBox.Show("Прямое преобразование Фурье с произвольным числом точек");
            if (zArrayPicture == null) { MessageBox.Show("Главное окно пустое == NULL"); return; }

            int nx = zArrayPicture.width;
            int ny = zArrayPicture.height;
       
            ZComplexDescriptor zComplex_Picture = new ZComplexDescriptor(zArrayPicture);  // Re=zArrayPicture Im=0
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
                 zComplex_Picture = FurieN.BPF2(zComplex_Picture);              
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);
          
           
            zArrayPicture = Furie.zAmplituda(zComplex_Picture);        // Выводится только амплитуда
            zArrayPicture = Furie.Invers_Double(zArrayPicture);        // Инверсия
           
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


        private void FurComplex_CUDA()                                    // --------------------------------------------------Прямое преобразование Фурье (CUDA) из главного окна
        {
            //MessageBox.Show("Прямое преобразование Фурье с произвольным числом точек");
            if (zArrayPicture == null) { MessageBox.Show("Главное окно пустое == NULL"); return; }

            CUDA_FFT.Fur_CUDA(zArrayPicture);                             // FFT из главного окна

            zArrayPicture = Furie.Invers_Double(zArrayPicture);           // Инверсия
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);           // Визуализация

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }


        private void FurComplex_CUDA1(int k1, int k2)                                            // Прямое преобразование Фурье (CUDA) из zComplex[k1] -> zComplex[k2]
        {
            //MessageBox.Show("Прямое преобразование Фурье с произвольным числом точек");
            if (zComplex[k1] == null) { MessageBox.Show("zComplex[k1] == NULL"); return; }

            zComplex[k2] = CUDA_FFT.Fur_CUDA1(zComplex[k1]); 

            Complex_pictureBox(k2);
           
        }


        //   ---------------------------------------------------------------------------------------------------------------------
        /*      private void InversComplexM(int k1, int k2)      // Инверсия в центр zComplex[regComplex]
                {

                    if (zComplex[regComplex] == null) { MessageBox.Show("zComplex[regComplex] == NULL"); return; }

                    zComplex[regComplex] =  Furie.Invers(zComplex[regComplex]);

                    Complex_pictureBox(regComplex);
                }
         * */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                   ->    Инверсия в pictureBox01
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button6_Click(object sender, EventArgs e)
        {
            if (zArrayDescriptor[regImage] == null) { MessageBox.Show("zArrayDescriptor[" +regImage+"] == NULL"); return; }
            zArrayPicture = Furie.Invers_Double(zArrayDescriptor[regImage]);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            //zArrayPicture.Double_Picture(pictureBox01);                     // В основное окно zArrayPicture
            //currentScaleRatio = 1;
            applyScaleModeToPicturebox();
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Интерференция плоских волн     (Сложение с опорной волной)     InterForm.cs
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////      
        private void интерференцияПлоскихВолнToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            InterForm BoxForm = new InterForm();
            BoxForm.OnBox       += FormOn;
            BoxForm.OnBoxADD    += FormOnADD;      // Сложить с плоской волной
            BoxForm.OnBoxSUB    += FormOnSUB;
           // BoxForm.OnBoxADD_Random += FormOnADD_Random;
            BoxForm.OnBoxMUL    += FormOnMUL;
            BoxForm.OnBoxPSI    += FormOnPSI;      // Сложение с фазовым сдвигом => 8,9,10,11
            BoxForm.OnBoxSUB1   += FormOnSUB1;
            BoxForm.OnBoxNoise  += FormOnNoise;
            BoxForm.Show();

        }
        // Сложить с плоской волной

        private void FormOnADD(double am, double AngleX, double AngleY, double Lambda, double dx, double noise, double fz) // Сложить с плоской волной + fz[0]
        {
            //int k1 = regComplex;
            if (zComplex[1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[k1] == NULL   FormOnADD"); return; }

            zComplex[2] = Model_interf.Model_pl_ADD_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz);
            Complex_pictureBox(2);

        }
        private void FormOnPSI(double am, double AngleX, double AngleY, double Lambda, double dx, double noise, double[] fz)
        {

            if (zComplex[1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[1] == NULL"); return; }
            zArrayDescriptor[8]  = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz[0]);
            zArrayDescriptor[9]  = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz[1]);
            zArrayDescriptor[10] = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz[2]);
            zArrayDescriptor[11] = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz[3]);

         
            Vizual_regImage(8); Vizual_regImage(9); Vizual_regImage(10); Vizual_regImage(11);


            // PSI
            //zComplex[1] = ATAN_PSI.ATAN_891011(zArrayDescriptor,  progressBar1, fz, am);


            //int i_Complex = 1;
            //ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[i_Complex * 4 + 2]);      // Амплитуда из regComplex = 1
            //zComplex[1] = Furie.ATAN_891011(zArrayDescriptor, fz);
            //zComplex[1] = Furie.ATAN_OLD4(zArrayDescriptor, fz);  // Формула из книги
            //Complex_pictureBox(1);


        }

        private void FormOnNoise(double noise)      // Добавление шума к фазе
        {

            if (zComplex[regComplex] == null) { MessageBox.Show("Ошибка добавления шума к фазе: zComplex[k1] == NULL"); return; }
            zComplex[regComplex] = Model_interf.Model_Noise(noise, zComplex[regComplex]);
            Complex_pictureBox(regComplex);

        }

        private void FormOn(double am, double AngleX, double AngleY, double Lambda, int NX, int NY, double dx, int k1)
        {

            zComplex[k1] = new ZComplexDescriptor(NX, NY);
            zComplex[k1] = Model_interf.Model_pl(am, AngleX, AngleY, Lambda, NX, NY, dx);
            Complex_pictureBox(k1);

        }

      
 /*       private void FormOnADD_Random(double am, double AngleX, double AngleY, double Lambda, double dx, int k1)
        {

            if (zComplex[k1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[k1] == NULL"); return; }
            zComplex[k1] = Model_interf.Model_pl_ADD_Random(am, zComplex[k1], AngleX, AngleY, Lambda, dx);
            Complex_pictureBox(k1);

        }
  * */
      
        private void FormOnMUL(double am, double AngleX, double AngleY, double Lambda, double dx, int k1)
        {

            if (zComplex[k1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[k1] == NULL"); return; }
            zComplex[k1] = Model_interf.Model_pl_MUL(am, zComplex[k1], AngleX, AngleY, Lambda, dx);
            Complex_pictureBox(k1);

        }
       
        //   Устранение наклона
        private void FormOnSUB1(double AngleX, double AngleY)
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL");  return; }
            if (zArrayDescriptor[regImage] == null) { MessageBox.Show("Вычитание наклона zArrayDescriptor[regImage] == NULL"); return; }

            zArrayPicture = Model_interf.Model_pl_SUB1(zArrayPicture, AngleX, AngleY);
            //Complex_pictureBox(k1);
            zArrayPicture.Double_Picture(pictureBox01);
        }
        // Вычитание фазы
        private void FormOnSUB(double am, double AngleX, double AngleY, double Lambda, double dx, int k1)
        {

            if (zComplex[k1] == null) { MessageBox.Show("Вычитание плоской волны zComplex[k1] == NULL"); return; }
            zComplex[k1] = Model_interf.Model_pl_SUB(am, zComplex[k1], AngleX, AngleY, Lambda, dx);
            Complex_pictureBox(k1);

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Интерференция сферических волн  
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void интерференцияСферическихВолнToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InterFormSph SpForm = new InterFormSph();
            SpForm.SpBox += Sphera;
            SpForm.SpBox1 += Sphera1;
            SpForm.SpBox2 += Sphera_SUB;
            //BoxForm.OnBoxADD += FormOnADD;
            //BoxForm.OnBoxMUL += FormOnMUL;
            SpForm.Show();
        }
        private void Sphera(double am, double Lambda, int NX, int NY, double dx, double dl, int k1)
        {
            zComplex[k1] = new ZComplexDescriptor(NX, NY);
            zComplex[k1] = Model_interf.Model_Sp(am, Lambda, NX, NY, dx, dl);
            Complex_pictureBox(k1);

        }
        private void Sphera1(double am, double Lambda, int NX, int NY, double dx, double dl, int k1)
        {
            zComplex[k1] = new ZComplexDescriptor(NX, NY);
            zComplex[k1] = Model_interf.Model_Sp1(am, Lambda, NX, NY, dx, dl);
            Complex_pictureBox(k1);

        }
        private void Sphera_SUB(double am, double Lambda, int NX, int NY, double dx, double dl, int k1)
        {
            zComplex[k1] = new ZComplexDescriptor(NX, NY);
            zComplex[k1] = Model_interf.Model_SpSUB(am, Lambda, NX, NY, dx, dl);
            Complex_pictureBox(k1);

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //           
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)  // -------------------------   Нижний движок
        {
            //vScrollBar2.Maximum = 255;
            //vScrollBar2.Value=50;
            //label33.Text = e.NewValue.ToString();
            //vScrollBar2.Update();
            //vScrollBar2.Show();

           // double max = Convert.ToDouble(textBox1.Text);    // Отображение pictureBox01
           // double min = Convert.ToDouble(textBox2.Text);
           // vScrollBar1.Maximum = Convert.ToInt32(max);
           // vScrollBar1.Minimum = Convert.ToInt32(min);

            int k = vScrollBar2.Maximum - e.NewValue;
           
            int k1 = Convert.ToInt32(textBox1.Text);   // Max
            if (k < k1) textBox2.Text = k.ToString();

           // double max = Convert.ToDouble(textBox1.Text);    // Отображение pictureBox01
           // double min = Convert.ToDouble(textBox2.Text);
           // SumClass.Range_Picture(pictureBox01, min, max);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)  // ----------------------- Верхний движок
        {
            //double max = Convert.ToDouble(textBox1.Text);    // Отображение pictureBox01
            //double min = Convert.ToDouble(textBox2.Text);
            //vScrollBar1.Maximum = Convert.ToInt32(max);
            //vScrollBar1.Minimum = Convert.ToInt32(min);
            

            int k = vScrollBar1.Maximum - e.NewValue;
           
            int k1 = Convert.ToInt32(textBox2.Text);  // Min
            if (k > k1) textBox1.Text = k.ToString();
           // textBox2.Text = e.NewValue.ToString();
            
            //SumClass.Range_Picture(pictureBox01, min, max);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //        Определение  Max Min zArrayPicture (основное окно)
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button5_Click(object sender, EventArgs e)
        {
            if (zArrayPicture == null)     { MessageBox.Show("zArrayPicture == NULL"); return; }
            if (zArrayPicture.width  == 0) { MessageBox.Show("zArrayPicture.width  = 0"); return; }
            if (zArrayPicture.height == 0) { MessageBox.Show("zArrayPicture.height = 0"); return; }
            double max, min;
            max = SumClass.getMax(zArrayPicture); textBox1.Text = Convert.ToString(max);
            min = SumClass.getMin(zArrayPicture); textBox2.Text = Convert.ToString(min);
            SumClass.Range_Picture(pictureBox01, zArrayPicture, min, max);
            //MessageBox.Show("min = " + Convert.ToString(min) + "max = " + Convert.ToString(max));

            //int BarMax = Convert.ToInt32(max - min);

            vScrollBar1.Maximum = Convert.ToInt32(max);
            vScrollBar1.Minimum = Convert.ToInt32(min);
            vScrollBar1.Value = Convert.ToInt32(min);
            vScrollBar1.Update();
            vScrollBar1.Show();

            vScrollBar2.Maximum = Convert.ToInt32(max);
            vScrollBar2.Minimum = Convert.ToInt32(min);
            vScrollBar2.Value = Convert.ToInt32(max);
            vScrollBar2.Update();
            vScrollBar2.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Range  (основное окно)  Приведение изображения к диапазону
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button4_Click(object sender, EventArgs e)
        {
            double max = Convert.ToDouble(textBox1.Text);
            double min = Convert.ToDouble(textBox2.Text);
            SumClass.Range_Picture(pictureBox01, zArrayPicture, min, max);
            //SumClass.Range_Picture(pictureBox01,  min, max);
            
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Shift  (основное окно)  Сдвиг по интенсивности
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button7_Click(object sender, EventArgs e)
        {
            double s_ogr = Convert.ToDouble(textBox1.Text);
            double s_min = Convert.ToDouble(textBox2.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = SumClass.Shift_Picture(amp, s_ogr, s_min);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Save  (основное окно)  Сдвиг по интенсивности
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       
        private void button8_Click(object sender, EventArgs e)
        {
            double max = Convert.ToDouble(textBox1.Text);
            double min = Convert.ToDouble(textBox2.Text);
            zArrayPicture = SumClass.Range_Array(zArrayPicture, min, max);
            SumClass.Range_Picture(pictureBox01, zArrayPicture, min, max);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        


//----------------------------------------------------------------------------------------------------------------------------------
        private void Фильтр_Click(object sender, EventArgs e)
        {

        }
        // -------------------------------------------------------------------------------------------------------------------------
        //                     Модель объекта
        // 
        // -------------------------------------------------------------------------------------------------------------------------
        private void модельОбъектаToolStripMenuItem_Click(object sender, EventArgs e)  // Модель объекта
        {
            Model ModelForm = new Model();                   // Model.cs

            ModelForm.OnModel += FormModel;                  // Записать в zComplex[regComplex] модель после деформации

            ModelForm.OnInterf_Fr += FormModel_fr;           // Записать в zComplex[1] модель в области Френеля до    деформации,
                                                             // записать в zComplex[2] модель в области Френеля после деформации,
                                                             // сложить с плоским фронтом под углом ax,ay
           // ModelForm.OnInterf        += FormInterf;
            ModelForm.OnInterf2       += FormInterf2;        // Двойная экспозиция
           // ModelForm.OnInterf3       += FormInterf3;
            ModelForm.OnInterfPSI_Fr  += FormInterfPSI_Fr;   // 4 интерферограммы в 8,9,10,11
            ModelForm.OnInterf8PSI_Fr += FormInterf8PSI_Fr;

            ModelForm.Show();
        }

        private void FormModel_fr(double sdvg0, double sdvg, double noise, double lambda, double lm, double dx, double ax, double ay)
        {
            
           
            zComplex[1] = Model_object.Model_2(sdvg0, noise, lambda);           // До деформации
            zComplex[1] = FurieN.FrenelTransformN(zComplex[1], lambda, lm, dx);
            zComplex[2] = Model_object.Model_2(sdvg, noise, lambda);            // После деформации
            zComplex[2] = FurieN.FrenelTransformN(zComplex[2], lambda, lm, dx);
            Complex_pictureBox(1); Complex_pictureBox(2);

        }
 /*       private void FormInterf(double sdvg, double noise, double Lambda)  // Модель объекта (пластинки) Сложение волновых фронтов
        {
            //if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            //if (zComplex[regComplex] == null) { MessageBox.Show("Моделирование объекта: zComplex[regComplex] == NULL"); return; }

            Model_object.Glgr_Interf(zComplex, zArrayDescriptor, sdvg, noise, Lambda);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
            Complex_pictureBox(2);
            //zArrayPicture.Double_Picture(pictureBox01);
        }
*/       
        // Двойная экспозиция
        private void FormInterf2(double sdvg0 ,double sdvg, double noise, double Lambda, double lm, double dx, double AngleX, double AngleY)  
        {
 
            Model_object.Glgr_Interf2(zComplex, zArrayDescriptor, sdvg0, sdvg, noise, Lambda, lm, dx, AngleX, AngleY);

            //Complex_pictureBox(0);
            Complex_pictureBox(1);
            Complex_pictureBox(2);

            Vizual_regImage(0);
            Vizual_regImage(1);
            Vizual_regImage(2);

        }
/*        private void FormInterf3(double sdvg, double noise, double Lambda)  // Модель объекта (пластинки) Две голограммы
        {

            Model_object.Glgr_Interf3(zComplex, zArrayDescriptor, sdvg, noise, Lambda);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
            zArrayDescriptor[8].Double_Picture(pictureBox9);
            zArrayDescriptor[9].Double_Picture(pictureBox10);
            //Complex_pictureBox(2);

        }
*/
        private void FormModel(double sdvg0, double sdvg, double noise, double Lambda)  // Модель объекта (пластинки)
        {
            MessageBox.Show("noise = " + noise);
            zComplex[0] = Model_object.Model_2(sdvg0, noise, Lambda);
            zComplex[1] = Model_object.Model_2(sdvg, noise, Lambda);
            Complex_pictureBox(0); Complex_pictureBox(1);            
        }
        /*
                private void FormInterfPSI(double sdvg0, double sdvg1, double noise, double Lambda, double dx, double[] fz)  // Модель объекта (пластинки) Двойная экспозиция Расшифровка PSI
                {

                    zArrayPicture = Model_object.Glgr_Interf_PSI(zComplex, zArrayDescriptor, sdvg0, sdvg1, noise, Lambda, dx, fz);
                    Complex_pictureBox(0);
                    Complex_pictureBox(1);
                    //Complex_pictureBox(2);
                    zArrayDescriptor[8].Double_Picture(pictureBox9);
                    zArrayDescriptor[9].Double_Picture(pictureBox10);
                    zArrayDescriptor[10].Double_Picture(pictureBox11);
                    zArrayDescriptor[11].Double_Picture(pictureBox12);

                    //Complex_pictureBox(regComplex);
                    zArrayPicture.Double_Picture(pictureBox01);
                }
         */
        // Модель объекта (пластинки) Двойная экспозиция Расшифровка PSI
        private void FormInterfPSI_Fr(double sdvg0, double sdvg1, double noise, double Lambda, double dx, double d, 
                                      double[] fz, double Ax, double Ay)  
        {

            Model_object.Glgr_Interf_PSI_Fr(zComplex, zArrayDescriptor, progressBar1, sdvg0, sdvg1, noise, Lambda, dx, d, fz, Ax, Ay);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
            //Complex_pictureBox(2);

           
            zArrayDescriptor[8].Double_Picture(pictureBox9);
            zArrayDescriptor[9].Double_Picture(pictureBox10);
            zArrayDescriptor[10].Double_Picture(pictureBox11);
            zArrayDescriptor[11].Double_Picture(pictureBox12);


            //Complex_pictureBox(regComplex);
            //zArrayPicture.Double_Picture(pictureBox01);
           // Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        // Расшифровка PSI по новому алгоритму

        private void FormInterf8PSI_Fr(double sdvg0, double sdvg1, double noise, double Lambda, double dx, double d, double[] fz, double Ax, double Ay) 
        {

            Model_object.Glgr_Interf8_PSI_Fr(zComplex, zArrayDescriptor, sdvg0, sdvg1, noise, Lambda, dx, d, fz, Ax, Ay);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
            Complex_pictureBox(2);
        }
        // -------------------------------------------------------------------------------------------------------------------------
 /*       private void aDDToolStripMenuItem_Click(object sender, EventArgs e)  // zArrayPicture + zComplex[1] => zComplex[1]
        {

        }

        private void zArrayPictureZComplex1ToolStripMenuItem_Click(object sender, EventArgs e)  // zArrayPicture + zArrayDescriptor[regImage] => zArrayPicture
        {

        }
*/
      // private void aTANToolStripMenuItem_Click(object sender, EventArgs e)
      // {
           //
      // }

        private void PRMTRToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        // -------------------------------------------------------------------------------------------------------------------------
        //  Моделирование интерференционных полос  и ATAN2
        // -------------------------------------------------------------------------------------------------------------------------
        private void моделированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Model_Sin ModelForm = new Model_Sin();
            ModelForm.OnModelSin          += FormModel_Sin;             // Задается число полос
            ModelForm.OnModelSin1         += FormModel_Sin1;            // Задается число точек в периоде
            ModelForm.OnModelWB           += FormModel_WB;
            ModelForm.OnModel_Dithering   += FormModel_Dithering;
            ModelForm.OnModel_DitheringVZ += FormModel_DitheringVZ;
            ModelForm.OnModelAtan2        += FormModel_Atan2;
            ModelForm.OnModelAtan2_L      += FormModel_Atan2_L;
            ModelForm.OnModelExp          += FormModel_Exp;
            ModelForm.Show();

        }

        private void FormModel_Exp(double g, int N)       // Модель exponent
        {
            zComplex[regComplex] = Model_Sinus.Exponenta(g,N);
            Complex_pictureBox(regComplex);
        }
        private void FormModel_Sin(double[] fz, double amp, double gamma, double n_pol, int kr, int N, double noise)       // Модель sin c фазовым сдвигом   kr - разреживание нулями
        {
            for (int i = 0; i < 4; i++)
            {
                zArrayDescriptor[i] = Model_Sinus.Sinus(fz[i], amp, n_pol, gamma, kr, N, noise);               
                Vizual_regImage(i);
            }         
        }

        private void FormModel_Sin1(double[] fz, double amp, double gamma, double n_pol, int kr, int N, double noise)       // Модель sin c фазовым сдвигом
        {
            for (int i = 0; i < 4; i++)
            {
                zArrayDescriptor[i] = Model_Sinus.Sinus1(fz[i], amp, n_pol, gamma, kr, N, noise);
                Vizual_regImage(i);
            }
        }

        private void FormModel_Dithering(double[] fz, double n_pol, int n_kvant, int n_urovn)       // Dithering
        {

            zArrayDescriptor[0] = Model_Sinus.Dithering(fz[0], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[0].Double_Picture(pictureBox1);


            zArrayDescriptor[1] = Model_Sinus.Dithering(fz[1], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[1].Double_Picture(pictureBox2);

            zArrayDescriptor[2] = Model_Sinus.Dithering(fz[2], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[2].Double_Picture(pictureBox3);

            zArrayDescriptor[3] = Model_Sinus.Dithering(fz[3], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[3].Double_Picture(pictureBox4);
        }

        private void FormModel_DitheringVZ(double[] fz, double n_pol, int n_kvant, int n_urovn)       // Dithering (Матрица возбуждения)
        {

            zArrayDescriptor[0] = Model_Sinus.DitheringVZ(fz[0], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[0].Double_Picture(pictureBox1);


            zArrayDescriptor[1] = Model_Sinus.DitheringVZ(fz[1], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[1].Double_Picture(pictureBox2);

            zArrayDescriptor[2] = Model_Sinus.DitheringVZ(fz[2], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[2].Double_Picture(pictureBox3);

            zArrayDescriptor[3] = Model_Sinus.DitheringVZ(fz[3], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[3].Double_Picture(pictureBox4);
        }



        private void FormModel_WB(double[] fz, double gamma, double n_pol)       // Модель WB полос
        {

            zArrayDescriptor[0] = Model_Sinus.WB(fz[0], n_pol);
            zArrayDescriptor[0].Double_Picture(pictureBox1);


            zArrayDescriptor[1] = Model_Sinus.WB(fz[1], n_pol);
            zArrayDescriptor[1].Double_Picture(pictureBox2);

            zArrayDescriptor[2] = Model_Sinus.WB(fz[2], n_pol);
            zArrayDescriptor[2].Double_Picture(pictureBox3);

            zArrayDescriptor[3] = Model_Sinus.WB(fz[3], n_pol);
            zArrayDescriptor[3].Double_Picture(pictureBox4);
        }

        private void FormModel_Atan2(double[] fz)  // Atan2 bp 1,2,3,4 => zArrayPicture
        {
            zArrayPicture = FazaClass.ATAN_N(zArrayDescriptor, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            //zArrayPicture.Double_Picture(pictureBox01);
        }
        private void FormModel_Atan2_L(double[] fz)  // Atan2 bp 1,2,3,4 => zArrayPicture
        {
            zArrayPicture = FazaClass.ATAN_Gr(zArrayDescriptor, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            //zArrayPicture.Double_Picture(pictureBox01);
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //           Моделирование фазы
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            

        private void моделированиеФазыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Model_Faza BoxFZ = new Model_Faza();
            BoxFZ.OnModelFz += ModelFaza;
            BoxFZ.OnModel_Sin_Fz += Model_Sin_Fz;   // c n1 и n2
            BoxFZ.OnModel_Sin_Fz3 += Model_Sin_Fz3; // c n1, n2 и n3
            BoxFZ.OnModelFzSub += ModelFazaSub;
            BoxFZ.OnModelFzSubA += ModelFazaSubA;
            BoxFZ.Show();
        }


        private void Model_Sin_Fz3(double n1, double n2, double n3, double n12, double n23, double noise)      // 4 sin c фазовым сдвигом => atan
        {
            int N = 1024;
            double[] fz = new double[4];
            fz[0] = Math.PI * 0 / 180.0; fz[1] = Math.PI * 90 / 180.0; fz[2] = Math.PI * 180 / 180.0; fz[3] = Math.PI * 270 / 180.0;  // Фазовый сдвиг в радианах  
                   
            zArrayDescriptor[8] = Model_Sinus.Sinus1(fz[0], 255, n1, 1, 0, N, noise);   // 4 синусоиды с периодом n1
            zArrayDescriptor[9] = Model_Sinus.Sinus1(fz[1], 255, n1, 1, 0, N, noise);
            zArrayDescriptor[10] = Model_Sinus.Sinus1(fz[2], 255, n1, 1, 0, N, noise);
            zArrayDescriptor[11] = Model_Sinus.Sinus1(fz[3], 255, n1, 1, 0, N, noise);
            zArrayDescriptor[0] = ATAN_PSI.ATAN(zArrayDescriptor, 8, 9, 10, 11, fz);   // ATAN2 с периодом n1  => zArrayDescriptor[0]

            zArrayDescriptor[8] = Model_Sinus.Sinus1(fz[0],  255, n2, 1, 0, N, noise);   // 4 синусоиды с периодом n2
            zArrayDescriptor[9] = Model_Sinus.Sinus1(fz[1],  255, n2, 1, 0, N, noise);
            zArrayDescriptor[10] = Model_Sinus.Sinus1(fz[2], 255, n2, 1, 0, N, noise);
            zArrayDescriptor[11] = Model_Sinus.Sinus1(fz[3], 255, n2, 1, 0, N, noise);
            zArrayDescriptor[1] = ATAN_PSI.ATAN(zArrayDescriptor, 8, 9, 10, 11, fz);   // ATAN2 с периодом n2  => zArrayDescriptor[1]

            zArrayDescriptor[8]  = Model_Sinus.Sinus1(fz[0], 255, n3, 1, 0, N, noise);   // 4 синусоиды с периодом n2
            zArrayDescriptor[9]  = Model_Sinus.Sinus1(fz[1], 255, n3, 1, 0, N, noise);
            zArrayDescriptor[10] = Model_Sinus.Sinus1(fz[2], 255, n3, 1, 0, N, noise);
            zArrayDescriptor[11] = Model_Sinus.Sinus1(fz[3], 255, n3, 1, 0, N, noise);
            zArrayDescriptor[2]  = ATAN_PSI.ATAN(zArrayDescriptor, 8, 9, 10, 11, fz);   // ATAN2 с периодом n2  => zArrayDescriptor[2]

            zArrayDescriptor[3] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[0], zArrayDescriptor[1], noise);   // n12
            zArrayDescriptor[4] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[1], zArrayDescriptor[2], noise);   // n23

            zArrayDescriptor[5] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[4], zArrayDescriptor[3], noise);   // n123
            double n123 = n12 * n23 / (Math.Abs(n12 - n23));
            zArrayDescriptor[6] = Model_Sinus.Model_FAZA_T(zArrayDescriptor[5], zArrayDescriptor[4], n123, n23);  // Ступеньки
            //zArrayDescriptor[6] = Model_Sinus.Model_FAZA_T(zArrayDescriptor[5], zArrayDescriptor[0], n123, n1);

            zArrayDescriptor[7] = Model_Sinus.Model_FAZA_SUM(zArrayDescriptor[6], zArrayDescriptor[4], n23);      // Восстановленный профиль
            //zArrayDescriptor[7] = Model_Sinus.Model_FAZA_SUM(zArrayDescriptor[6], zArrayDescriptor[0], n1);

            zArrayDescriptor[8] = Model_Sinus.Model_FAZA_T(zArrayDescriptor[7], zArrayDescriptor[0], 1, n1);  // Ступеньки
            zArrayDescriptor[9] = Model_Sinus.Model_FAZA_SUM(zArrayDescriptor[8], zArrayDescriptor[0], n1);


            for (int i = 0; i < 12; i++) Vizual_regImage(i);
            

        }



        private void Model_Sin_Fz(double n1, double n2, double noise)      // 4 sin c фазовым сдвигом => atan
        {
            int N = 1024;
            double[] fz = new double[4];
            fz[0] = Math.PI *   0 / 180.0;                                // Фазовый сдвиг в радианах  
            fz[1] = Math.PI *  90 / 180.0;
            fz[2] = Math.PI * 180 / 180.0;
            fz[3] = Math.PI * 270 / 180.0;
            zArrayDescriptor[4]  = Model_Sinus.Sinus1(fz[0], 255, n1, 1, 0, N, noise);   // 4 синусоиды с периодом n1
            zArrayDescriptor[5]  = Model_Sinus.Sinus1(fz[1], 255, n1, 1, 0, N, noise);
            zArrayDescriptor[6]  = Model_Sinus.Sinus1(fz[2], 255, n1, 1, 0, N, noise);
            zArrayDescriptor[7]  = Model_Sinus.Sinus1(fz[3], 255, n1, 1, 0, N, noise);

           // for (int i = 0; i < 4; i++) fz[i] = fz[i] + Math.PI * 20 / 180.0;  // Сдвиг

            zArrayDescriptor[8]  = Model_Sinus.Sinus1(fz[0], 255, n2, 1, 0, N, noise);   // 4 синусоиды с периодом n2
            zArrayDescriptor[9]  = Model_Sinus.Sinus1(fz[1], 255, n2, 1, 0, N, noise);
            zArrayDescriptor[10] = Model_Sinus.Sinus1(fz[2], 255, n2, 1, 0, N, noise);
            zArrayDescriptor[11] = Model_Sinus.Sinus1(fz[3], 255, n2, 1, 0, N, noise);
    

            zArrayDescriptor[0] = ATAN_PSI.ATAN(zArrayDescriptor, 4, 5, 6, 7, fz);
            zArrayDescriptor[1] = ATAN_PSI.ATAN(zArrayDescriptor, 8, 9, 10, 11, fz);

            zArrayDescriptor[2] = Model_Sinus.Model_FAZA_SUB(zArrayDescriptor[0], zArrayDescriptor[1]);
            zArrayDescriptor[3] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[0], zArrayDescriptor[1], noise);
            for (int i = 0; i < 1024; i++) for (int j = 0; j < 100; j++)   zArrayDescriptor[3].array[i, j] = zArrayDescriptor[0].array[i, j];
            for (int i = 0; i < 1024; i++) for (int j = 100; j < 200; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[1].array[i, j];
            for (int i = 0; i < 1024; i++) for (int j = 200; j < 300; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[2].array[i, j];

             zArrayDescriptor[4]= Model_Sinus.Model_FAZA_T1(zArrayDescriptor[3], n1, n2);
            
            for (int i = 0; i < 12; i++) Vizual_regImage(i);
           
        }

        private void ModelFaza(double n1, double n2, double noise)      // Фаза пила 1024/n
        {
            zArrayDescriptor[0] = Model_Sinus.Model_FAZA(n1, noise);
            zArrayDescriptor[1] = Model_Sinus.Model_FAZA(n2, noise);
            zArrayDescriptor[2] = Model_Sinus.Model_FAZA_SUB(zArrayDescriptor[0], zArrayDescriptor[1]);
            zArrayDescriptor[3] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[0], zArrayDescriptor[1], noise);
            for (int i = 0; i < 1024; i++) for (int j =   0; j < 128; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[0].array[i, j];
            for (int i = 0; i < 1024; i++) for (int j = 128; j < 256; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[1].array[i, j];
            for (int i = 0; i < 1024; i++) for (int j = 256; j < 380; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[2].array[i, j];

            Vizual_regImage(0);
            Vizual_regImage(1);
            Vizual_regImage(2);
            Vizual_regImage(3);
        }



        private void ModelFazaSub(int k1,int k2, int k3)      // Вычитание фаз
        {
            k1--; k2--; k3--;
            
            //MessageBox.Show("Эквивалентная длина волны " + k1 + " " + k2 + " " + k3);
            zArrayDescriptor[k1] = Model_Sinus.Model_FAZA_SUB(zArrayDescriptor[k2],zArrayDescriptor[k3]);
            Vizual_regImage( k1);
        }
        private void ModelFazaSubA(int k1, int k2, int k3)      // Вычитание фаз абсолютная
        {
            k1--; k2--; k3--;
            MessageBox.Show("Вычитание " + k1 + " " + k2 + " " + k3);

            zArrayDescriptor[k1] = Model_Sinus.Model_FAZA_SUBA(zArrayDescriptor[k2], zArrayDescriptor[k3]);
            Vizual_regImage( k1);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                       Фазовая развертка
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        private void разверткаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // MessageBox.Show("Фазовая развертка" );
        }

        private void разверткаToolStripMenuItem_Click(object sender, EventArgs e)     // Построение таблицы
        {
            UnrupForm UnrupForm = new UnrupForm();
            UnrupForm.OnUnrup              += FormUnrup;                     // Таблица со сдвигом
            UnrupForm.OnUnrup_Diag         += FormUnrup_Diag;
            UnrupForm.OnUnrup_Diag_Tab     += FormUnrup_Diag_Tab;
            UnrupForm.OnUnrup_Diag_Tab256  += FormUnrup_Diag_Tab256;
            UnrupForm.OnUnrup_Tab256       += FormUnrup_Tab256;
            UnrupForm.OnUnrup_2pi          += FormUnrup_2pi;
            UnrupForm.OnUnrup_Line         += FormUnrup_Line;
            UnrupForm.Show();
        }

        private void FormUnrup(PictureBox pictureBox1, int k1, int k2, int N1_sin, int N2_sin, int scale, int d, int sdvig)      
        {
            Unrup.Tabl_int(zArrayDescriptor, pictureBox1, k1, k2, N1_sin, N2_sin, scale, d, sdvig);
        }

        private void FormUnrup_Diag(PictureBox pictureBox1, int N1_sin, int N2_sin, int scale, int d) // Диагонали
        {
            Unrup.Tabl_Diag(zArrayDescriptor, pictureBox1,  N1_sin, N2_sin, scale, d);
        }

        private void FormUnrup_Diag_Tab(PictureBox pictureBox1, int N1_sin, int N2_sin, int scale, int d) // Данные по диагоналям
        {
            Unrup.Tabl_Diag_Tab(zArrayDescriptor, pictureBox1, N1_sin, N2_sin, scale, d);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------
        private void FormUnrup_Diag_Tab256(int N1_sin, int N2_sin,  int d)      // Данные (от 0 до 255) по диагоналям
        {
            zArrayPicture = Unrup.Tabl_Diag_Tab256(N1_sin, N2_sin, d);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void FormUnrup_Tab256(int b)
        {
            zArrayPicture = Unrup.Tabl256(zArrayDescriptor[2], zArrayDescriptor[0], zArrayDescriptor[1], b);                // Из 1 и 2 =>  zArrayPicture
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        private void FormUnrup_2pi(int b)  // Ручное устранение фазовой неоднозначности
        {
            zArrayDescriptor[regImage] = Unrup.Unrup_2pi(zArrayDescriptor[regImage], b);                // Из zArrayDescriptor[regImage]  =>  zArrayDescriptor[regImage]
            Vizual_regImage(regImage);
            //Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }
      
        private void FormUnrup_Line()  // Вычитание тренда по двум точкам
        {

            int X1 = Convert.ToInt32(textBox3.Text);
            int Y1 = Convert.ToInt32(textBox4.Text);

            int X2 = Convert.ToInt32(textBox5.Text);
            int Y2 = Convert.ToInt32(textBox6.Text);

         
            zArrayDescriptor[regImage] = Unrup.Unrup_Line(zArrayDescriptor[regImage], X1, Y1, X2, Y2);    // Из zArrayDescriptor[regImage]  =>  zArrayDescriptor[regImage]
            Vizual_regImage(regImage);
            //Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }
        //-----------------------------------------------------------------------------------------------------------------
        private void разверткаToolStripMenuItem1_Click(object sender, EventArgs e)  // Развертка
        {
            UnrupForm1 UnrupForm1 = new UnrupForm1();
            UnrupForm1.OnUnrup1 += FormUnrup1;

            UnrupForm1.Show();

        }
        private void FormUnrup1(int k1, int k2, int k3, int N1_sin, int N2_sin, int N_diag, int sdvig, int Mngtl)
        {
            zArrayDescriptor[k3] = Unrup.Unrup_array(zArrayDescriptor, k1, k2, N1_sin, N2_sin, N_diag, sdvig, Mngtl);
            Vizual_regImage(k3);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //           PSI  из 8,9,10,11 в zArrayPicture fz[4]         Амплитуда Фаза Квантование
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pSI4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PSI PSIForm = new PSI();
            PSIForm.OnPSI   += FormPSI;              // PSI амплитуда и фаза =>  (8,9,10,11)
            PSIForm.OnPSI1  += FormPSI1;             // PSI  фазы (1,2,3,4) -> 5
            PSIForm.OnIMAX  += FormIMAX;             // Квантование (8,9,10,11)
            PSIForm.OnIMAX1 += FormIMAX1;            // Квантование одного кадра
            PSIForm.OnMaska += FormMaska;            // Наложение маски
            PSIForm.OnLis   += FormLis;              // Фигуры Лиссажу

            PSIForm.Show();
        }

        private void FormLis(double[] fz)
        {
            zArrayPicture = FazaClass.ATAN_Gr9101112(zArrayDescriptor, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }
        private void FormPSI(double[] fz, double am)
        {
            zComplex[1] = ATAN_PSI.ATAN_891011(zArrayDescriptor, progressBar1, fz, am);
            Complex_pictureBox(1);

        }
        private void FormPSI1(int k1, int k2, int k3, int k4, int k5, double[] fz)
        {
       
            zArrayDescriptor[k5] = ATAN_PSI.ATAN(zArrayDescriptor, k1, k2, k3, k4, fz);
            Vizual_regImage(k5);
        }

        private void FormIMAX(int imax)
        {

            ATAN_PSI.Diapazon(zArrayDescriptor, imax);

            zArrayDescriptor[8].Double_Picture(pictureBox9);
            zArrayDescriptor[9].Double_Picture(pictureBox10);
            zArrayDescriptor[10].Double_Picture(pictureBox11);
            zArrayDescriptor[11].Double_Picture(pictureBox12);

        }

        private void FormIMAX1(int imax)
        {
            ATAN_PSI.Diapazon1(zArrayDescriptor, regImage, imax);
            Vizual_regImage(regImage);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

       
        private void интерферометрияToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

		private void FormMaska(int k1, int k2, int k3)
        {
            zArrayDescriptor[k3] = ATAN_PSI.Maska(zArrayDescriptor, k1, k2);
            Vizual_regImage(k3);
        }
//
//                  Устранение фона по трем точкам
//
        private void устраненияФонаПо3ТочкамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int X1 = Convert.ToInt32(textBox3.Text);
            int Y1 = Convert.ToInt32(textBox4.Text);

            int X2 = Convert.ToInt32(textBox5.Text);
            int Y2 = Convert.ToInt32(textBox6.Text);

            int X3 = Convert.ToInt32(textBox7.Text);
            int Y3 = Convert.ToInt32(textBox8.Text);

            int X4 = Convert.ToInt32(textBox9.Text);
            int Y4 = Convert.ToInt32(textBox10.Text);


            zArrayPicture = Rot.Sub_Plane(zArrayPicture, X1, Y1,  X2,  Y2, X3,  Y3);     
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        private void pictureBox01_Click(object sender, EventArgs e)
        {

        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageCamera();
        }


        private void ManageCamera()
        {
            CameraForm cameraForm = new CameraForm();
            cameraForm.PictureTaken += new PictureTakenHandler(HandleCameraPicture);
            cameraForm.LiveViewUpdated += new LiveViewUpdatedHandler(HandleLiveViewUpdate);
            cameraForm.Show();
        }

        private void HandleCameraPicture(PictureTakenEventArgs eventArgs)
        {
            if (eventArgs == null) return;

            switch (eventArgs.PhaseShiftNumber)
            {
                
                case 0:
                    zArrayPicture = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                    pictureBox01.Image = eventArgs.Image;
                    break;

                case 1:
                    {
                        zArrayDescriptor[8] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        //zArrayDescriptor[8] = Util_array.getArrayFromBitmap(eventArgs.Image);
                        pictureBox9.Image = eventArgs.Image;
                        break;
                    }
                case 2:
                    {
                        zArrayDescriptor[9] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        //zArrayDescriptor[9] = Util_array.getArrayFromBitmap(eventArgs.Image);
                        pictureBox10.Image = eventArgs.Image;
                        break;
                    }
                case 3:
                    {
                        zArrayDescriptor[10] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        //zArrayDescriptor[10] = Util_array.getArrayFromBitmap(eventArgs.Image);
                        pictureBox11.Image = eventArgs.Image;
                        break;
                    }
                case 4:
                    {
                        zArrayDescriptor[11] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        //zArrayDescriptor[11] = Util_array.getArrayFromBitmap(eventArgs.Image);
                        pictureBox12.Image = eventArgs.Image;
                        break;
                    }

            }
        }

        private void HandleLiveViewUpdate(LiveViewUpdatedEventArgs eventArgs)
        {
            switch (eventArgs.PhaseShiftNumber)
            {
                case 1:
                    {
                        zArrayDescriptor[8] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        //zArrayDescriptor[8] = Util_array.getArrayFromBitmap(eventArgs.Image);
                        pictureBox9.Image = eventArgs.Image;
                        break;
                    }
                case 2:
                    {
                        zArrayDescriptor[9] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        //zArrayDescriptor[9] = Util_array.getArrayFromBitmap(eventArgs.Image);
                        pictureBox10.Image = eventArgs.Image;
                        break;
                    }
                case 3:
                    {
                        zArrayDescriptor[10] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        //zArrayDescriptor[10] = Util_array.getArrayFromBitmap(eventArgs.Image);
                        pictureBox11.Image = eventArgs.Image;
                        break;
                    }
                case 4:
                    {
                        zArrayDescriptor[11] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        //zArrayDescriptor[11] = Util_array.getArrayFromBitmap(eventArgs.Image);
                        pictureBox12.Image = eventArgs.Image;
                        break;
                    }

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void фурьеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }





























        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}

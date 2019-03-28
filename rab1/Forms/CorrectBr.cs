using System;
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
    public delegate void VisualRegImageDelegate(int k);  // Для VisualRegImage 
    public delegate void PhotoDelegate();                // TakePhoto12



    public delegate void CorrectBr1(int k1, int k2, int n);
    public delegate void CorrectBr2(int k1, int k2);
   // public delegate void CorrectBr3(int k1, int k2, int N, int nx, int ny);
    public partial class CorrectBr : Form
    {
        public static VisualRegImageDelegate VisualRegImage = null;  // Визуализация одного кадра от 0 до 11 из main
        public static PhotoDelegate TakePhoto12             = null;  // Ввести кадр из 1 в 2 из main

        public event CorrectBr1 On_CorrectX;
        public event CorrectBr1 On_CorrectG;
       // public event CorrectBr1 On_CorrectClin;
        public event CorrectBr2 On_CorrectSumm;
       // public event CorrectBr3 On_CorrectGxy;

        private static int n = 4096;                  // Размер массива
        private static int k1 = 1;
        private static int k2 = 2;

        private static int k11 = 1;
        private static int k21 = 2;
        private static int k31 = 5;

        private static int k12 = 1;
        //private static int I0 = 0;
        private static double gamma = 1;
       
        private static int nx = 4096;                  // Размер массива
        private static int ny = 2160;                  // Размер массива

        private static int kv = 16;

        private static int X0   = 100;
        private static int STEP = 128;
        //private static int k18 = 1;

        private static int dx = 100;

        private static int porog = 250;

        //double[] cl = { 1,  2,  3,  4,  5,   6,   7,   8,   9,  10,  11,  12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,  23,  24,  25,  26,  27,  28, 29,   30,  31, 32 };
        //double[] cl = { 0,  1,  2,  3,  4,   5,   6,   7,   8,   9,  10,  11,  12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,  23,  24,  25,  26,  27, 28,   29,  30, 31 };
        //double[] cl = { 7, 15, 23, 31, 39,  47,  55,  63,  71,  79,  87,  95, 103, 111, 119, 127, 135, 143, 151, 159, 167, 175, 183, 191, 199, 207, 215, 223, 231, 239, 247, 255 };
        // double[] cl = { 0, 15, 47, 63, 79, 95, 111, 127, 143, 159,  175, 191, 207, 223, 239, 255 };


        // double[] cl = { 30, 45, 60, 75, 90, 105, 120, 135, 150, 165,  180, 195, 210, 225, 240, 255 };    Правильные значения
        // double[] cl = { 16, 32, 48, 64, 80, 96, 112, 128, 144, 160, 174, 190, 206, 222, 238, 255   };

        // double[] cl = { 0, 32, 40, 48, 57, 65,  75,  85,  95, 110,  120, 135, 155, 170, 200, 255 };
        double[] cl = { 0, 37, 48, 54, 63, 68, 77, 85, 93, 101, 111, 124, 139, 162, 187, 255 };



        public CorrectBr()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(k1);     // Номер кадра
            textBox3.Text = Convert.ToString(k2);     // Номер кадра
            textBox2.Text = Convert.ToString(n);      // Размер массива

            textBox4.Text = Convert.ToString(k11);
            textBox5.Text = Convert.ToString(k21);
            textBox6.Text = Convert.ToString(k31);

            textBox7.Text = Convert.ToString(gamma);
            textBox8.Text = Convert.ToString(k12);

            textBox9.Text = Convert.ToString(porog);

            textBox14.Text = Convert.ToString(nx);
            textBox15.Text = Convert.ToString(ny);

            textBox16.Text = Convert.ToString(kv);  // Число градаций 16, 32, 64, 128, 256

            textBox17.Text = Convert.ToString(X0);    // Начало для вертикальных линий
            textBox19.Text = Convert.ToString(STEP);  // Шаг для вертикальных линий
           // textBox18.Text = Convert.ToString(k18 );  // В какой массив
          
            textBox20.Text = Convert.ToString(dx);
        }


        /// <summary>
        /// Массив меняет длину от X1 до X2 -> 0 до 4096      
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)    // Button1 "Выравнивание по длине массива"
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            n = Convert.ToInt32(textBox2.Text);

            On_CorrectX(k1-1, k2-1, n);
            Close();
        }
        /// <summary>
        ///  Определения клина с обратными гамма-искажениями
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            k11 = Convert.ToInt32(textBox4.Text);
            k21 = Convert.ToInt32(textBox5.Text);
            k31 = Convert.ToInt32(textBox6.Text);
            On_CorrectG(k11 - 1, k21 - 1, k31-1);
            Close();
        }
        //------------------------------------------------------------------------------------------- Клин
        /// <summary>
        /// Клин от I0 до 255 размер текущий размер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            k12 = Convert.ToInt32(textBox8.Text);
            gamma = Convert.ToDouble(textBox7.Text);
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);

            int nu = 255;                                               // Число уровней
            Form1.zArrayDescriptor[k12-1] = Model_Sinus.Intensity1(nu,  nx, ny, gamma);
            VisualRegImage(k12-1);

           // Close();

        }
        private void button7_Click(object sender, EventArgs e)          // ---------------------------------   Обратный клин
        {
            k12 = Convert.ToInt32(textBox8.Text);
            gamma = Convert.ToDouble(textBox7.Text);
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);

            int nu = 255;                                               // Число уровней
            Form1.zArrayDescriptor[k12 - 1] = Model_Sinus.Intensity2(nu, nx, ny, gamma);
            VisualRegImage(k12 - 1);

            //Close();
        }
  /// <summary>
  ///  Вертикальные линии для определения геометрических искажений
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            int k = Convert.ToInt32(textBox8.Text) - 1;
            if (Form1.zArrayDescriptor[k] == null) { MessageBox.Show("CorrectBr zArrayDescriptor == NULL"); return; }
            int Nx = Form1.zArrayDescriptor[k].width;
            int Ny = Form1.zArrayDescriptor[k].height;

            X0 = Convert.ToInt32(textBox17.Text);    // Полосы рисуются начиная с X0
            STEP = Convert.ToInt32(textBox19.Text);  //Шаг полос

            for (int i = X0; i < Nx; i += STEP)
            {
                for (int j = 0; j < Ny; j++)
                {
                    double z = Form1.zArrayDescriptor[k].array[i, j];
                    if (z < 128) Form1.zArrayDescriptor[k].array[i, j] = 255; else Form1.zArrayDescriptor[k].array[i, j] = 0;
                }
            }
            VisualRegImage(k);
        }


        /// <summary>
        /// Ч/Б полосы с заданным чмслом градаций
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            k12 = Convert.ToInt32(textBox8.Text);
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);
            kv = Convert.ToInt32(textBox16.Text);                       // Число градация
            int dx = 100;
            int nx1 = nx + dx * 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx1, ny);

            int step = nx / kv;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                   {
                      if ( (i / step) % 2 == 0 ) cmpl.array[i + dx, j] = 0; else cmpl.array[i + dx, j] = 255;
                   }

            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);     // Белая и черная полоса по краям

            Form1.zArrayDescriptor[k12 - 1] = cmpl;
            VisualRegImage(k12 - 1);


        }

        /// <summary>
        /// Клин с 16 значениями интенсивности из массива
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
   
            k12 = Convert.ToInt32(textBox8.Text);
          
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);          

            double[] am = new double[nx];
            //for (int i = 0; i < nx; i++) { am[i] = cl[i / 256]; }      // строка клина 128 -  0-31  256 -  0-15

            
            kv = Convert.ToInt32(textBox16.Text);                      //  Число градаций
            am = Clin(kv, nx);                                     // Формирование клина

            if (am == null ) { MessageBox.Show("kv != 16, 32, 64, 128  kv= " + kv); return; }

            int dx = 100;
            int Nx1 = nx + dx * 2; 
            
            ZArrayDescriptor cmpl = new ZArrayDescriptor(Nx1, ny);
            // ----------------------------------------------------------------------------------- Клин
            for (int i = 0; i < nx; i++)
               for (int j = 0; j < ny; j++)
                   {  cmpl.array[i + dx, j] = am[i];  }

            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);     // Белая и черная полоса по краям
            Form1.zArrayDescriptor[k12 - 1] = cmpl;
            VisualRegImage(k12 - 1);
            // ----------------------------------------------------------------------------------- Обратный клин
            ZArrayDescriptor cmpl1 = new ZArrayDescriptor(Nx1, ny);
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                { cmpl1.array[i + dx, j] = am[nx-1-i]; }

            cmpl1 = Model_Sinus.Intens(0, 256, dx, cmpl1);     // Белая и черная полоса по краям
            Form1.zArrayDescriptor[k12 - 1 + 1] = cmpl1;
            VisualRegImage(k12 - 1 + 1);

        }


        private double[] Clin( int kv, int nx)
        {
            double[] am = new double[nx];
            int nsd = 0;
            switch (kv)
            {
                case 16: for (int i = 0; i < nx; i++) { am[i] = cl[i / 256]; } break;    // 4096
                case 32:
                    cl = MasX2(cl);
                    nsd = nx / 32;
                    for (int i = 0; i < nx; i++) { am[i] = cl[i / 128]; }
                    am = Strerch(am, nsd);
                    break;  // 4096-128
                case 64:
                    cl = MasX2(cl); cl = MasX2(cl);
                    nsd = nx / 32 + nx / 64;
                    for (int i = 0; i < nx; i++) { am[i] = cl[i / 64]; }
                    am = Strerch(am, nsd);
                    break;
                case 128:
                    cl = MasX2(cl); cl = MasX2(cl); cl = MasX2(cl);
                    nsd = nx / 32 + nx / 64 + nx / 128;
                    for (int i = 0; i < nx; i++) { am[i] = cl[i / 32]; }
                    am = Strerch(am, nsd);
                    break;
                case 256:
                    cl = MasX2(cl); cl = MasX2(cl); cl = MasX2(cl); cl = MasX2(cl);
                    nsd = nx / 32 + nx / 64 + nx / 128 + nx / 256;
                    for (int i = 0; i < nx; i++) { am[i] = cl[i / 16]; }
                    am = Strerch(am, nsd);
                    break;
                default:  return null;
            }

            return am;

        }

        /// <summary>
        /// Растяжение массива 0 - n-nx -> 0 - n
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        private double[] Strerch(double[] am, int nx)
        {
           int n = am.Length;        double[] am2 = new double[n];
           int n1 = n - nx;

            for (int i=0; i<n; i++)   
                { 
                    int i1 = (n1-1) * i / n ;
                    am2[i] = am[i1];
                }
            return am2;
        }
        private double[] MasX2(double[] am)
        {
            int nx  = am.Length;
            int nx2 = nx * 2;
            double[] am2 = new double[nx2];

           for (int i = 0; i < nx2-1; i++)
              {
                 if (i % 2 == 0) am2[i] =  am[i/2];
                  if (i % 2 != 0) am2[i] = (am[i/2] + am[i / 2 + 1])/2;
              }
        

            return am2;
        }
        /// <summary>
        /// Сложение строк от Y1 до Y2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            On_CorrectSumm(k1-1, k2 - 1);
            Close();

        }
        /// <summary>
        ///  Растяжение массива для проекции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       // private void button5_Click(object sender, EventArgs e)
      //  {
           // textBox9.Text = Convert.ToString(k14);    // Сложение строк
          
      //      k1 = Convert.ToInt32(textBox1.Text);
     //       k2 = Convert.ToInt32(textBox3.Text);
      //      N_Line = Convert.ToInt32(textBox13.Text);
      //      nx= Convert.ToInt32(textBox14.Text);
       //     ny = Convert.ToInt32(textBox15.Text);
      //      On_CorrectGxy(k1 - 1, k2 - 1, N_Line, nx, ny);
      //      Close();

     //   }
        /// <summary>
        /// Выбрасывание белой и черной полосы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            dx    = Convert.ToInt32(textBox20.Text);

            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("CorrectBr zArrayDescriptor == NULL"); return; }
            int nx1 = Form1.zArrayDescriptor[k1-1].width;
            int ny = Form1.zArrayDescriptor[k1-1].height;

            nx = nx1 - dx * 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);      // Результирующий фронт

            for (int i = dx; i < nx1-dx; i++)
              for (int j = 0; j < ny; j++)
                {
                     cmpl.array[i-dx,j] = Form1.zArrayDescriptor[k1-1].array[i , j];
                }
            Form1.zArrayDescriptor[k2-1] = cmpl;
            VisualRegImage(k2-1);
            Close();
        }
        /// <summary>
        /// Выделение белых и черных полос
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            porog  = Convert.ToInt32(textBox9.Text);
           
            if (Form1.zArrayDescriptor[k1-1] == null) { MessageBox.Show("CorrectBr zArrayDescriptor == NULL"); return; }
            int nx = Form1.zArrayDescriptor[k1 - 1].width;
            int ny = Form1.zArrayDescriptor[k1 - 1].height;

          

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);      // Результирующий фронт

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double a = Form1.zArrayDescriptor[k1 - 1].array[i, j];
                    if (a > porog) cmpl.array[i, j] = 255; else cmpl.array[i, j] = 0;
                }
            Form1.zArrayDescriptor[k2 - 1] = cmpl;
            VisualRegImage(k2 - 1);
            Close();
        }
        /// <summary>
        /// Усреднение клина
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox3.Text);
            porog = Convert.ToInt32(textBox9.Text);

            if (Form1.zArrayDescriptor[k1 - 1] == null) { MessageBox.Show("CorrectBr zArrayDescriptor 1 == NULL"); return; }
            if (Form1.zArrayDescriptor[k2 - 1] == null) { MessageBox.Show("CorrectBr zArrayDescriptor 2 == NULL"); return; }
            int nx = Form1.zArrayDescriptor[k1 - 1].width;
            int ny = Form1.zArrayDescriptor[k1 - 1].height;

            ZArrayDescriptor cmpl_1 = new ZArrayDescriptor(nx, ny);      // Полосы
            ZArrayDescriptor cmpl_2 = new ZArrayDescriptor(nx, ny);      // Клин

            double[] am1 = new double[nx];
            double[] am2 = new double[nx];
            double[] am3 = new double[nx];

            int N_line = 128;

            for (int i = 0; i < nx; i++)
            {
               am1[i] = Form1.zArrayDescriptor[k1 - 1].array[i, N_line];
               am2[i] = Form1.zArrayDescriptor[k2 - 1].array[i, N_line];
            }

            int ii = 0, i2 = 0, i1 =0;
            double s = 0;

            do
            {
                
                i1 = ii;
                s = 0;                 while (am1[ii] == 0) { s = s + am2[ii]; ii++; if (ii >= nx) break; }
                i2 = ii--; s = s/(i2 - i1);
                for (int i = i1; i < i2; i++) am3[i] = s;

                ii++; i1 = ii;  if (ii >= nx) break;
                s = 0;                 while (am1[ii] != 0) { s = s + am2[ii]; ii++; if (ii >= nx) break; }
                i2 = ii--; s = s/(i2 - i1);
                for (int i = i1; i < i2; i++) am3[i] = s;
                ii++;
            }
            while (ii < nx);


            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    cmpl_1.array[i, j] = am3[i];
                }
            Form1.zArrayDescriptor[k2] = cmpl_1; //  => 3
            VisualRegImage(k2 );

            Close();
        }
        /// <summary>
        /// Ввод клина с камеры и обработка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            int nx = 4096;
            int dx = 100;
            int nx1 = nx + dx * 2;              // Размер для изображения с добавленными полосами
            double[] am = new double[nx];
            int kv = 16;                        //  Число градаций

            am = Clin(kv, nx);                  // Формирование клина
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx1, ny);
            
            for (int i = 0; i < nx; i++) for (int j = 0; j < ny; j++)  { cmpl.array[i + dx, j] = am[i]; } // ----------------- Клин
            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);                                                  // Белая и черная полоса по краям
            Form1.zArrayDescriptor[0] = cmpl;    // Клин в 1 массив
            VisualRegImage(0);
            TakePhoto12();
        }
    }
}

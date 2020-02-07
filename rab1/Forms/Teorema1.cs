using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;

namespace rab1.Forms
{
    public delegate void VisualComplexDelegate(int k);
    public delegate void VisualArrayDelegate();
    public partial class Teorema1 : Form
    {

        public static VisualComplexDelegate VisualComplex = null;
        public static VisualArrayDelegate   VisualArray = null;

        private static int Number_kadr     = 1;
        private static int Number_Pont_Rec = 1024;
        private static int Number_Pont     = 2048;
        //private static int Number_Sinc     = 2048;

        private static double Number_Period   = 8;
        private static int dx = 32;   // Шаг дискретизации
        private static int t  = 30;   // Размер прямоугольного импульса

        private static int k1 = 1;
        private static int k2 = 2;
        private static int k3 = 1;   // Для синусоиды
        private static int k4 = 1;   // Для синусоиды умноженной на прямоугольник
        private static int k5 = 1;   // Для дискретизации

        private static int k6 = 3;      // Для дискретизации
        private static int Step0 = 0;   // Смешение при дискретизации

     

        public Teorema1()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(Number_kadr);
            textBox2.Text = Convert.ToString(Number_Pont_Rec);
            textBox3.Text = Convert.ToString(Number_Pont);
            textBox4.Text = Convert.ToString(k1);
            textBox5.Text = Convert.ToString(k2);
            textBox6.Text = Convert.ToString(Number_Period);
            textBox7.Text = Convert.ToString(k3);
            textBox8.Text = Convert.ToString(k4);
            textBox9.Text = Convert.ToString(k5);
            textBox10.Text = Convert.ToString(dx);

            textBox11.Text = Convert.ToString(k6);
            textBox12.Text = Convert.ToString(Step0);        // Смешение при дискретизации
            textBox13.Text = Convert.ToString(t);        // Размер прямоугольного импульса
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
/// <summary>
/// Прямоугольник
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Number_kadr     = Convert.ToInt32(textBox1.Text);
            Number_Pont_Rec = Convert.ToInt32(textBox2.Text);  // Число точек в прямоугольнике
            Number_Pont     = Convert.ToInt32(textBox3.Text);  // Общее число точек
            int regComplex  = Number_kadr - 1;

            int nx = Number_Pont;
            int ny = 100;
            int x0 = nx / 2 - Number_Pont_Rec / 2;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);
            for (int i = x0; i < x0 + Number_Pont_Rec; i++)
                for (int j = 0; j < ny; j++) { cmpl.array[i, j] = 1; }

            // ZComplexDescriptor(ZArrayDescriptor descriptorToCopy)
            Form1.zComplex[regComplex] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[regComplex] = new ZComplexDescriptor(cmpl);  // амплитуда = cmpl, фаза - (прежняя) нулевая
            VisualComplex(regComplex);

            //Close();
        }
        /// <summary>
        /// Синусоида
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Number_Period = Convert.ToDouble(textBox6.Text);
            k3 = Convert.ToInt32(textBox7.Text);
            int regComplex = k3 - 1;
            Number_Pont = Convert.ToInt32(textBox3.Text);

            int nx = Number_Pont;
            int ny = 100;

            double a1 = 1;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                   { //cmpl.array[i, j] = a1 * (Math.Sin(2.0 * Math.PI * i / Number_Period - fz) + 1.0) / 2.0;     // Число точек
                     cmpl.array[i, j] = a1 * (Math.Sin(2.0 * Math.PI * i * Number_Period / nx ) + 1.0) / 2.0;  
                }
            Form1.zComplex[regComplex] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[regComplex] = new ZComplexDescriptor(cmpl);  // амплитуда = cmpl, фаза - (прежняя) нулевая
            VisualComplex(regComplex);

        }
/// <summary>
/// 3 синусоиды
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            double Period  = 8; // 7.2;
            double Period1 = 4; // 3.3;
            double Period2 = 1;
            //double Period =  7.2;
            //double Period1 = 3.3;
            //double Period2 = 1;
            k3 = Convert.ToInt32(textBox7.Text);
            int regComplex = k3 - 1;
            Number_Pont = Convert.ToInt32(textBox3.Text);

            int nx = Number_Pont;
            int ny = 100;
            

            double a1 = 1;
            a1 = a1 / 3;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                { 
                    cmpl.array[i, j] = a1 *( (Math.Sin(2.0 * Math.PI * i * Period / nx) + 1.0) / 2.0 +
                                             (Math.Sin(2.0 * Math.PI * i * Period1 / nx) + 1.0) / 2.0 +
                                             (Math.Sin(2.0 * Math.PI * i * Period2 /  nx) + 1.0) / 2.0
                                            ) ;
                }
            Form1.zComplex[regComplex] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[regComplex] = new ZComplexDescriptor(cmpl);  // амплитуда = cmpl, фаза - (прежняя) нулевая
            VisualComplex(regComplex);

        }


        private void button17_Click(object sender, EventArgs e)
        {
            //double Period = 8; // 7.2;
            //double Period1 = 4; // 3.3;
            //double Period2 = 1;
            double Period =  7.2;
            double Period1 = 3.3;
            double Period2 = 1;
            k3 = Convert.ToInt32(textBox7.Text);
            int regComplex = k3 - 1;
            Number_Pont = Convert.ToInt32(textBox3.Text);

            int nx = Number_Pont;
            int ny = 100;


            double a1 = 1;
            a1 = a1 / 3;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    cmpl.array[i, j] = a1 * ((Math.Sin(2.0 * Math.PI * i * Period / nx) + 1.0) / 2.0 +
                                             (Math.Sin(2.0 * Math.PI * i * Period1 / nx) + 1.0) / 2.0 +
                                             (Math.Sin(2.0 * Math.PI * i * Period2 / nx) + 1.0) / 2.0
                                            );
                }
            Form1.zComplex[regComplex] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[regComplex] = new ZComplexDescriptor(cmpl);  // амплитуда = cmpl, фаза - (прежняя) нулевая
            VisualComplex(regComplex);

        }


        /// <summary>
        /// Умножение на прямоугольник
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button5_Click(object sender, EventArgs e)
        {
            k4 = Convert.ToInt32(textBox8.Text);
            Number_Pont_Rec = Convert.ToInt32(textBox2.Text);
            Number_Pont = Convert.ToInt32(textBox3.Text);
            int regComplex = k4 - 1;

            int nx = Number_Pont;
            int ny = 100;
            int x0 = Number_Pont / 2 - Number_Pont_Rec / 2;


            double [] array = new double[nx];
            for (int i = x0; i < x0 + Number_Pont_Rec; i++)  { array[i] = 1; }
            

            ZComplexDescriptor cmpl = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i++)
               for (int j = 0; j < ny; j++)
                    { if (array[i] > 0 ) cmpl.array[i, j] = Form1.zComplex[regComplex].array[i, j];                 }

            Form1.zComplex[regComplex] = cmpl;

          
            VisualComplex(regComplex);
        }

        /// <summary>
        /// Оставить точки, ограниченные прямоугольником
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button10_Click(object sender, EventArgs e)
        {
            k4 = Convert.ToInt32(textBox8.Text);
            Number_Pont_Rec = Convert.ToInt32(textBox2.Text);
            Number_Pont = Convert.ToInt32(textBox3.Text);
           

            int nx = Number_Pont;
            int ny = 100;
            int x0 = Number_Pont / 2 - Number_Pont_Rec / 2;


            ZComplexDescriptor cmpl = new ZComplexDescriptor(Number_Pont_Rec, ny);
            for (int i = 0; i < Number_Pont_Rec; i++)
                for (int j = 0; j < ny; j++)
                {  cmpl.array[i, j] = Form1.zComplex[k4-1].array[x0 + i, j]; }

            Form1.zComplex[k4 - 1] = cmpl;
            Number_Pont = Number_Pont_Rec;

            VisualComplex(k4 - 1);
        }
        /// <summary>
        /// Оставить точки, вне прямоугольника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button19_Click(object sender, EventArgs e)
        {
            k4 = Convert.ToInt32(textBox8.Text);                    // Номер окна
            Number_Pont_Rec = Convert.ToInt32(textBox2.Text);       // Размер прямоугольника
            Number_Pont = Convert.ToInt32(textBox3.Text);           // Общее число точек

            int nx = Number_Pont;
            int ny = 100;
            int x0 = Number_Pont_Rec / 2;
            int x1 = nx  - Number_Pont_Rec / 2;

            ZComplexDescriptor cmpl = new ZComplexDescriptor(nx, ny);
            for (int i = x0+1; i < x1; i++)
                for (int j = 0; j < ny; j++)
                  { cmpl.array[i, j] = Form1.zComplex[k4 - 1].array[i, j]; }

           
            Form1.zComplex[k4 - 1] = cmpl;
            

            VisualComplex(k4 - 1);
        }

        /// <summary>
        /// Добавление боковых лепестков  из 1 и 2 => центральное окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button20_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox4.Text);       // Боковые лепестки (вещественный)
            k2 = Convert.ToInt32(textBox5.Text);       // Одиночный спектр (вещественный)
            k6 = Convert.ToInt32(textBox11.Text);       // Одиночный спектр (вещественный)

            Number_Pont_Rec = Convert.ToInt32(textBox2.Text);       // Размер прямоугольника
            Number_Pont = Convert.ToInt32(textBox3.Text);           // Общее число точек
            int nx = Number_Pont;
            int ny = 100;
            int x0 = Number_Pont_Rec / 2;
            int x1 = nx - Number_Pont_Rec / 2;

            Complex[] a = new Complex[nx];                                              // Переписываем одиночный пик
            for (int i = 0; i < nx; i++) a[i] = Form1.zComplex[k2 - 1].array[i, ny / 2];
            Complex [] b = new Complex[nx];                                             // Переписываем лепестки (приведенные к 1)
            for (int i = 0; i < nx; i++) b[i] = Form1.zComplex[k1 - 1].array[i, ny / 2];
                      
            double min = double.MaxValue;                                               // Max и Min боковых лепестков
            double max = double.MinValue;

            for (int i = 0; i < nx; i++)
            {
                if (min > b[i].Magnitude) min = b[i].Magnitude;
                if (max < b[i].Magnitude) max = b[i].Magnitude;
            }

            double min1 = a[x0+1 ].Magnitude;

            //double min1 = double.MaxValue;                                               //Min одиночного пика
            //for (int i = 0; i < nx; i++) { if (min1 > a[i].Magnitude) min1 = a[i].Magnitude; }

            MessageBox.Show("max = " + max + " min = " + min + " minpik = " + min1);

            for (int i = 0; i < nx; i++) { b[i] = (b[i] - min)*min1 / (max - min); }

            for (int i = x0 + 1; i < x1; i++) a[i] = b[i];                                 // Переписываем одиночный пик + боковые лепестки


            ZComplexDescriptor cmpl = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)                
                for (int j = 0; j < ny; j++)
                    cmpl.array[i, j] = a[i];

           
            

           

            Form1.zComplex[k6 - 1] = cmpl;


            VisualComplex(k6 - 1);
        }

   



        /// <summary>
        /// Фурье по строке 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);

            int nx = Form1.zComplex[k1 - 1].width ;
            int ny = Form1.zComplex[k1 - 1].height;

            int m = Furie.PowerOfTwo(nx);                                       // nx=2**m
            //MessageBox.Show(" m = " + m + " nx = " + nx + " ny = " + ny     );

            Complex[] c = new Complex[nx];
            Complex[] c1 = new Complex[nx];

            for (int i = 0; i < nx; i++) c[i] = Form1.zComplex[k1 - 1].array[i, ny/2];

            c1 = Furie.GetFourierTransform(c, m);
           
            Form1.zComplex[k2-1] = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[k2-1].array[i, j] = c1[i];

            VisualComplex(k2-1);
            //Close();
        }
/// <summary>
/// Обратное БПФ * sqrt(dx)
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);
            dx = Convert.ToInt32(textBox10.Text);

            int nx = Form1.zComplex[k1 - 1].width;
            int ny = Form1.zComplex[k1 - 1].height;

            int m = Furie.PowerOfTwo(nx);                                       // nx=2**m
            //MessageBox.Show(" m = " + m + " nx = " + nx + " ny = " + ny);

            Complex[] c = new Complex[nx];
            Complex[] c1 = new Complex[nx];

            for (int i = 0; i < nx; i++) c[i] = Form1.zComplex[k1 - 1].array[i, ny / 2];

            c1 = Furie.GetInverseFourierTransform(c, m);

            double d2 = Math.Sqrt(dx);
            Form1.zComplex[k2 - 1] = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    //Form1.zComplex[k2 - 1].array[i, j] = dx*c1[i];
                      Form1.zComplex[k2 - 1].array[i, j] = d2*c1[i];

            VisualComplex(k2 - 1);
            //Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);
            dx = Convert.ToInt32(textBox10.Text);

            int nx = Form1.zComplex[k1 - 1].width;
            int ny = Form1.zComplex[k1 - 1].height;

            int m = Furie.PowerOfTwo(nx);                                       // nx=2**m
            //MessageBox.Show(" m = " + m + " nx = " + nx + " ny = " + ny);

            Complex[] c = new Complex[nx];
            Complex[] c1 = new Complex[nx];

            for (int i = 0; i < nx; i++) c[i] = Form1.zComplex[k1 - 1].array[i, ny / 2];

            c1 = Furie.GetInverseFourierTransform(c, m);

            double d2 = 1; // Math.Sqrt(dx);
            Form1.zComplex[k2 - 1] = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    //Form1.zComplex[k2 - 1].array[i, j] = dx*c1[i];
                    Form1.zComplex[k2 - 1].array[i, j] = d2 * c1[i];

            VisualComplex(k2 - 1);
            //Close();
        }

        /// <summary>
        /// Дискретизация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            k5    = Convert.ToInt32(textBox9.Text);   // Номер кадра
            dx    = Convert.ToInt32(textBox10.Text);
            Step0 = Convert.ToInt32(textBox12.Text);

            int nx = Form1.zComplex[k5 - 1].width;
            int ny = Form1.zComplex[k5 - 1].height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i=i+dx)
                for (int j = 0; j < ny; j++)
                    cmpl.array[i + Step0, j] = Form1.zComplex[k5 - 1].array[i+Step0, j];

            Form1.zComplex[k5 - 1] = cmpl;
            VisualComplex(k5 - 1);
        }
        /// <summary>
        /// Дискретизация прямоугольного импульсами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button18_Click(object sender, EventArgs e)
        {
            k5 = Convert.ToInt32(textBox9.Text);      // Номер кадра
            dx = Convert.ToInt32(textBox10.Text);
            //Step0 = Convert.ToInt32(textBox12.Text);
            t = Convert.ToInt32(textBox13.Text);      // Размер прямоугольного импульса

            int nx = Form1.zComplex[k5 - 1].width;
            int ny = Form1.zComplex[k5 - 1].height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(nx, ny);

            double[] c = new double[nx];
            double[] c1 = new double[nx];
            for (int i = 0; i < nx; i++)  c[i] = Form1.zComplex[k5 - 1].array[i, ny/2].Magnitude; 
            int dxt= (dx-t)/2;
            for (int i = 0; i < nx; i = i + dx) for (int j = 0; j < t; j++) c1[i] += c[i + j + dxt];
            //MessageBox.Show("-------------------------------- ");

            for (int i = 0; i < nx; i++)
              for (int j = 0; j < ny; j++)
                cmpl.array[i,  j] = new Complex(c1[i]/t, 0.0);

            Form1.zComplex[k5 - 1] = cmpl;
            VisualComplex(k5 - 1);
        }


        /// <summary>
        ///  SinC => Главное окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            Number_Pont = Convert.ToInt32(textBox3.Text);       // Общее число точек
            Number_Pont_Rec = Convert.ToInt32(textBox2.Text);  // Число точек в прямоугольнике
         
            //dx = Convert.ToInt32(textBox10.Text);
            double N = Number_Pont_Rec;
            int nx = Number_Pont;
            int ny = 100;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);
            double sc = 0;
            for (int i = 0; i < nx; i++)
                for (int y = 0; y < ny; y++)
                {
                    //double x = 2*Math.PI * i / nx;
                    double x = i;
                    double d = N * (x - nx/2) / 2;
                    if (d != 0) sc = Math.Sin(d) / d; else sc = 1;
                    cmpl.array[i, y] = sc;
                }

            Form1.zArrayPicture = cmpl;

            VisualArray();

        }
/*
        private double Sinc1(double x, int n, int nx)
        {
            double sc = 0;

            double d = n * (x - Math.PI)/(2.0);
            if (d != 0) sc = Math.Sin(d) / d; else sc = 1;

            return sc;
        }
*/
        private double Sinc(double x, double dx, int n)
        {
            double sc = 0;
           
            double d = (Math.PI / dx) * (x - n  * dx);
            if (d != 0) sc = Math.Sin(d) / d; else sc = 1;
           
           return sc;
        }
        private double[] Svrtk_Sinc(double[] f, int dx, int nx)
        {

            int N_Sinc = nx / dx;
            int nx1 = nx + N_Sinc;
            double[] f_interp = new double[nx1];

            for (int x = 0; x < nx1; x++)
            {
                double s = 0;
                for (int n = 0; n < N_Sinc; n++)
                {
                    s += f[n * dx] * Sinc(x, dx, n);
                }
                f_interp[x] = s ;

            }
            return f_interp;
        }

        private double SinS(double x, int dx, int n, int nx)
        {
            double sc = 0;
            double Pi_dx_N = Math.PI / (nx * dx);
            double Pi_dx   = Math.PI /  dx;
           

            double d  = Math.Sin(Pi_dx_N * (x - n * dx));
            double d1 = Math.Sin(Pi_dx * (x - n * dx));

            if (d != 0) sc = d1 / d; else sc = nx;
         
            return sc / nx;
        }

       
        private double[] Svrtk_SinS(double[] f, int dx, int nx)
        {

            int N_Sinc = nx / dx;
            int nx1 = nx + N_Sinc;
            double[] f_interp = new double[nx1];

            for (int x = 0; x < nx1; x++)
            {
                double s = 0;
                for (int n = 0; n < N_Sinc; n++)
                {
                    s += f[n * dx] * SinS(x, dx, n, nx); 
                }
                f_interp[x] = s;

            }
            return f_interp;
        }
        /// <summary>
        /// Интерполяция с ограниченной решеткой Дирака
        /// в [k5] непрерывная функция
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void button11_Click(object sender, EventArgs e)
        {
            k5 = Convert.ToInt32(textBox9.Text);
            dx = Convert.ToInt32(textBox10.Text);
            //Number_Sinc = Convert.ToInt32(textBox11.Text);


            int nx = Form1.zComplex[k5 - 1].width;
            int ny = Form1.zComplex[k5 - 1].height;

            //nx = 128;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);

            int N_Sinc = nx / dx;
            int nx1 = nx + N_Sinc;
            double[] f = new double[nx1];
            double[] f_interp = new double[nx1];

            for (int i = 0; i < nx; i++) { f[i] = Form1.zComplex[k5 - 1].array[i, ny / 2].Magnitude; }
         

            f_interp = Svrtk_SinS(f, dx, nx);  // --------------------------------------------------------------

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    cmpl.array[i, j] = f_interp[i];


            Form1.zArrayPicture = cmpl;

            VisualArray();
            Close();
        }
        /// <summary>
        /// Интерполяция по Котельникову
        /// в [k5] непрерывная функция
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void button7_Click(object sender, EventArgs e)
        {
            k5 = Convert.ToInt32(textBox9.Text);
            dx = Convert.ToInt32(textBox10.Text);
            //Number_Sinc = Convert.ToInt32(textBox11.Text);


            int nx = Form1.zComplex[k5 - 1].width;
            int ny = Form1.zComplex[k5 - 1].height;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);
            
            int N_Sinc = nx/dx;
            int nx1 = nx + N_Sinc;
            double[] f = new double[nx1];
            double[] f_interp = new double[nx1];

            for (int i = 0; i < nx; i++) { f[i] = Form1.zComplex[k5 - 1].array[i, ny / 2].Magnitude; }
         
            f_interp = Svrtk_Sinc(f, dx, nx);

            for (int i = 0; i < nx; i++)
                  for (int j = 0; j < ny; j++)
                    cmpl.array[i, j] = f_interp[i];
        

            Form1.zArrayPicture = cmpl;

            VisualArray();
            Close();
        }
/// <summary>
/// Фурье - Интерполяция в области Фурье - ОБПФ
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            dx = Convert.ToInt32(textBox10.Text);
          
            k1 = Convert.ToInt32(textBox4.Text);  // Из первого окна
            k2 = Convert.ToInt32(textBox5.Text);  // Во второе

            int nx = Form1.zComplex[k1 - 1].width;
            int ny = Form1.zComplex[k1 - 1].height;

            Complex[] c  = new Complex[nx];
            Complex[] c1 = new Complex[nx];

            for (int i = 0; i < nx; i++) c[i] = Form1.zComplex[k1 - 1].array[i, ny / 2];

           // int m = Furie.PowerOfTwo(nx);                                       // nx=2**m
           // c1 = Furie.GetFourierTransform(c, m);                               // Фурье

            int Pi_dx = nx / dx;
            
            Complex[] array = new Complex[nx];
            for (int i = 0; i < Pi_dx/2; i++)         { array[i] = c[i]; }     // Выделение одного спектра
            for (int i = nx - Pi_dx / 2; i < nx; i++) { array[i] = c[i]; }
        
            Form1.zComplex[k2-1] = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i++)
               for (int j = 0; j < ny; j++)
                  Form1.zComplex[k2-1].array[i, j] = array[i];
            VisualComplex(k2 - 1);
        }
/// <summary>
///                 БПФ  и  Растяжение
///                 Исходный массив - непрерывный  => Дискретизация со сдвигом => БПФ => растяжение => Перенос в центр
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            dx = Convert.ToInt32(textBox10.Text);
            k1 = Convert.ToInt32(textBox4.Text);      // Из первого окна
            k2 = Convert.ToInt32(textBox5.Text);      // Во второе фурье
            k6 = Convert.ToInt32(textBox11.Text);     // В третье фурье
            Step0 = Convert.ToInt32(textBox12.Text);  // Сдвиг при дискретизации

           

            int nx = Form1.zComplex[k1 - 1].width;
            int ny = Form1.zComplex[k1 - 1].height;

            int nxd = nx / dx;
                                   
            
            Complex[] cd    = new Complex[nxd];
            Complex[] st = new Complex[nx];

            double p2 = 2*Math.PI*Step0 / nx;
           
            for (int i = 0; i < nxd; i++) cd[i] = Form1.zComplex[k1 - 1].array[i*dx+Step0, ny / 2];
            for (int i = 0; i < nx; i++) st[i] = new Complex(Math.Cos(p2*i), Math.Sin(p2 * i));        // Сдвиг на Step

            int m = Furie.PowerOfTwo(nxd);                                       // nx=2**m
            cd = Furie.GetFourierTransform(cd, m);                               // Фурье

            //  ZComplexDescriptor rez1 = new ZComplexDescriptor(nxd, ny);          // Вывод в 3-е окно
            //  for (int i = 0; i < nxd; i++)
            //      for (int j = 0; j < ny; j++)
            //          rez1.array[i, j] = cd[i];
            //   Form1.zComplex[k6 - 1] = rez1;
            //   VisualComplex(k6 - 1);
            //MessageBox.Show("--nxd = " + nxd + " m = " + m);

            Complex[] array = new Complex[nx];
                  
            for (int i = 0; i < nxd / 2; i++)       { array[i] = cd[i]; }            // Выделение одного спектра
            for (int i = nx - nxd / 2, j = nxd/2; i < nx; i++, j++) { array[i] = cd[j]; }

            //MessageBox.Show("++nxd = " + nxd +" m = " + m);
            ZComplexDescriptor rez2 = new ZComplexDescriptor(nx, ny);
          
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    rez2.array[i, j] = array[i] * st[i];  // Сдвиг на Step0

            Form1.zComplex[k2 - 1] = rez2;
           VisualComplex(k2 - 1);
           
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dx = Convert.ToInt32(textBox10.Text);
            k2 = Convert.ToInt32(textBox5.Text);   // Из второго
            k6 = Convert.ToInt32(textBox11.Text);  // В третье фурье

            int nx = Form1.zComplex[k1 - 1].width;
            int ny = Form1.zComplex[k1 - 1].height;

            int nxd = nx / dx;

            Complex[] array = new Complex[nx];
            for (int i = 0; i < nx; i++) array[i] = Form1.zComplex[k2 - 1].array[i, ny / 2];

            Complex[] c = new Complex[nx];                                  // Циклический сдвиг влево
            for (int i = 0; i < nx/2; i++)   c[i]      = array[nx/2 + i];
            for (int i = nx/2; i > 0; i--)   c[nx - i] = array[nx/2 - i];

            Complex[] cd = new Complex[nxd];
            for (int i = 0; i < nxd; i++) cd[i] = c[i*dx];                 // Сжатие на dx

            int m = Furie.PowerOfTwo(nxd);                                       // nx=2**m
            cd = Furie.GetInverseFourierTransform(cd, m);

            for (int i = 0; i < nx;  i++) c[i]  = new Complex(0.0, 0.0);
            for (int i = 0; i < nxd; i++) c[i * dx] = cd[i];                 // Расширение на dx


            Form1.zComplex[k6 - 1] = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[k6 - 1].array[i, j] =c[i];
            VisualComplex(k6 - 1);
        }
/// <summary>
///  Дискретная свертка
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox4.Text);  // Из первого окна  f1
            k2 = Convert.ToInt32(textBox5.Text);  // Из второго       f2

            int nx = Form1.zArrayDescriptor[k1 - 1].width;
            int ny = Form1.zArrayDescriptor[k1 - 1].height;

            int M = nx;
            int N = Form1.zArrayDescriptor[k2 - 1].width;

            ZArrayDescriptor cmpl = new ZArrayDescriptor(M + N - 1, ny);
            double[] rez = new double[M + N - 1];
            double[] x = new double[M];
            double[] h = new double[N];

            for (int i = 0; i < M; i++) x[i] = Form1.zArrayDescriptor[k1 - 1].array[i, ny / 2];
            for (int i = 0; i < N; i++) h[i] = Form1.zArrayDescriptor[k2 - 1].array[i, ny / 2];

            for (int i = 0; i < N + M - 1; i++)
            {
                for (int j = 0; j < M; j++) { if (i - j >= 0 && i - j < N)  rez[i] += x[i - j] * h[j];  }
            }

            for (int i = 0; i < M+N-1; i++)
                for (int j = 0; j < ny; j++)
                    cmpl.array[i, j] = rez[i];

            Form1.zArrayPicture = cmpl;

            VisualArray();

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}

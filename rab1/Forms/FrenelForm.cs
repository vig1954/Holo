using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void FrenelF(double xmax, double lambda, double d, int k1, int k2);
public delegate void FrenelNXY(int k1, int k2, int X, int Y, int N, double xmax, double lambda, double d);
public delegate void FrenelF1(int k1, int k2);
public delegate void FrenelF2(int k1, int k2);
public delegate void FrenelF3(int k1, int k2);
public delegate void FrenelF4();
public delegate void FrenelF5(int k1, int k2, int X, int Y, int N );
public delegate void FrenelF6(double[] fz, double xmax, double lambda, double d, int k2);
public delegate void FrenelF17(double step_fz);
public delegate void FrenelF18(double[] fz, double xmax, double lambda, double d, int X, int Y, int X1, int Y1, int N);

namespace rab1.Forms
{

    public partial class FrenelForm : Form
    {
        public event FrenelF  OnFrenel;
        public event FrenelF OnFrenelN;            // Френель с четным количеством точек
        public event FrenelF OnFrenelN_CUDA;       // Френель CUDA

        public event FrenelF1 OnFurie;
        public event FrenelF2 OnFurieM;
        public event FrenelF2 OnFurie_CUDA_CMPLX;   // Фурье CUDA из к1 в k2

        public event FrenelF4 OnFurie_N;
        public event FrenelF1 OnFurie_2Line;       // Фурье с четным количеством точек по строкам из k1 в k2
        public event FrenelF4 OnFurie_CUDA;
        public event FrenelF5 OnFurie_NXY;
        public event FrenelNXY OnFrenel_NXY;        // Френель с четным количеством точек с фиксированным размером из k1 в k2
       
        public event FrenelF6 OnPSI_fast;
        public event FrenelF17 OnADD_PHASE;

        public event FrenelF18 OnInterf_XY;

        //public event FrenelF2 InversComplex;

        private  static double xm = 7;
        private  static double lm = 0.5;
        private  static double dm = 135;
        private static int k1 = 1;
        private static int k2 = 0;
        private static int X = 600;
        private static int Y = 600;
        private static int X1 = 602;
        private static int Y1 = 600;
        private static int N = 2048;
       
        private static double[] fz = { 0.0, 90.0, 180.0, 270.0 };

        public FrenelForm()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(xm);
            textBox2.Text = Convert.ToString(lm);
            textBox3.Text = Convert.ToString(dm);
            textBox4.Text = Convert.ToString(k1);
            textBox5.Text = Convert.ToString(k2);
            textBox6.Text = Convert.ToString(X);
            textBox7.Text = Convert.ToString(Y);
            textBox8.Text = Convert.ToString(N);
            textBox9.Text = Convert.ToString(X1);
            textBox10.Text = Convert.ToString(Y1);

            textBox13.Text = Convert.ToString(fz[0]);
            textBox14.Text = Convert.ToString(fz[1]);
            textBox15.Text = Convert.ToString(fz[2]);
            textBox16.Text = Convert.ToString(fz[3]);
        }

        private void button1_Click(object sender, EventArgs e) // Френель с количеством точек 2**N
        {
            xm = Convert.ToDouble(textBox1.Text);
            lm = Convert.ToDouble(textBox2.Text);
            dm = Convert.ToDouble(textBox3.Text);
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);

            OnFrenel(xm * 1000, lm, dm * 1000, k1, k2); // в микрометрах
            Close();
        }

        private void button6_Click(object sender, EventArgs e)  // Френель с четным количеством точек
        {
            xm = Convert.ToDouble(textBox1.Text);
            lm = Convert.ToDouble(textBox2.Text);
            dm = Convert.ToDouble(textBox3.Text);
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);

            OnFrenelN(xm * 1000, lm, dm * 1000, k1, k2);        // в микрометрах
            Close();
        }

        private void button9_Click(object sender, EventArgs e)   // Френель (CUDA) из окна в окно
        {

            xm = Convert.ToDouble(textBox1.Text);
            lm = Convert.ToDouble(textBox2.Text);
            dm = Convert.ToDouble(textBox3.Text);
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);

            OnFrenelN_CUDA(xm * 1000, lm, dm * 1000, k1, k2);        // в микрометрах
            Close();

        }



        private void button2_Click(object sender, EventArgs e)
        {          
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);
            
            OnFurie( k1, k2);
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
           
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);

            OnFurieM(k1, k2);
            Close();
        }

        private void button8_Click(object sender, EventArgs e)    // Фурье (CUDA) из k1 в k2
        {
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);

            OnFurie_CUDA_CMPLX(k1, k2);
            Close();
        }

        private void button4_Click_1(object sender, EventArgs e)  // Фурье с четным количеством точек из главного окна
       { 
           OnFurie_N();
           Close();
       }

        private void button7_Click(object sender, EventArgs e)   // Фурье (CUDA) из главного окна
        {
            OnFurie_CUDA();
            Close();
        }


        private void button5_Click(object sender, EventArgs e)  // Преобразование Фурье с фиксированным количеством точек 
        {
            k1 = Convert.ToInt32(textBox4.Text); k2 = Convert.ToInt32(textBox5.Text);
            X = Convert.ToInt32(textBox6.Text); Y = Convert.ToInt32(textBox7.Text); N = Convert.ToInt32(textBox8.Text);
            OnFurie_NXY(k1, k2, X, Y, N);
            Close();
        }

        private void button13_Click(object sender, EventArgs e) // Преобразование Френеля с фиксированным количеством точек 
        {
            k1 = Convert.ToInt32(textBox4.Text); k2 = Convert.ToInt32(textBox5.Text);
            xm = Convert.ToDouble(textBox1.Text);
            lm = Convert.ToDouble(textBox2.Text);
            dm = Convert.ToDouble(textBox3.Text);


            X = Convert.ToInt32(textBox6.Text);  Y = Convert.ToInt32(textBox7.Text); N = Convert.ToInt32(textBox8.Text);
            OnFrenel_NXY(k1, k2, X, Y, N, xm * 1000, lm, dm * 1000);  //  OnFrenelN(xm * 1000, lm, dm * 1000, k1, k2);  
            Close();
        }

        private void FrenelForm_Load(object sender, EventArgs e)
        {

        }

        private static int M(int n)                                    // Определение нечетного делителя числа
        {
            int m = n;
            for (int i = 1; ; i++) { if (m % 2 == 0) m = m / 2; else break; }
            return m;
        }

        private static int L(int n)                                     // Определение четного делителя числа
        {
            int m = M(n);
            return n / m;
        }

        private static int T(int l)                                    // Определение степени l = 2**t
        {
            int m = 1;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > l) { m = i; break; } }
            return m;
        }

        private void button10_Click(object sender, EventArgs e)   // Преобразование Фурье с четным количеством точек по строкам
        {
            k1 = Convert.ToInt32(textBox4.Text); k2 = Convert.ToInt32(textBox5.Text);
            OnFurie_2Line(k1, k2);
            Close();
        }

        private void button11_Click(object sender, EventArgs e)  // PSI (8,9,10,11) -> Преобразование Френеля -> в к2
        {
            fz[0] = Convert.ToDouble(textBox13.Text);
            fz[1] = Convert.ToDouble(textBox14.Text);
            fz[2] = Convert.ToDouble(textBox15.Text);
            fz[3] = Convert.ToDouble(textBox16.Text);
            
            double[] fzrad = new double[4];                 // Фаза в радианах
            fzrad[0] = Math.PI * fz[0] / 180.0;
            fzrad[1] = Math.PI * fz[1] / 180.0;
            fzrad[2] = Math.PI * fz[2] / 180.0;
            fzrad[3] = Math.PI * fz[3] / 180.0;

            xm = Convert.ToDouble(textBox1.Text);
            lm = Convert.ToDouble(textBox2.Text);
            dm = Convert.ToDouble(textBox3.Text);
            //k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);                   // Номер Complex

            //OnFrenelN(xm * 1000, lm, dm * 1000, k1, k2);        // в микрометрах




            OnPSI_fast(fzrad, xm * 1000, lm, dm * 1000, k2);
            Close();
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            double step = Convert.ToDouble(textBox17.Text);
            OnADD_PHASE(step);
            Close();
        }

        private void button14_Click(object sender, EventArgs e)  // Интерферометрия сдвинутых полей
        {
            fz[0] = Convert.ToDouble(textBox13.Text);
            fz[1] = Convert.ToDouble(textBox14.Text);
            fz[2] = Convert.ToDouble(textBox15.Text);
            fz[3] = Convert.ToDouble(textBox16.Text);

            double[] fzrad = new double[4];                 // Фаза в радианах
            fzrad[0] = Math.PI * fz[0] / 180.0;
            fzrad[1] = Math.PI * fz[1] / 180.0;
            fzrad[2] = Math.PI * fz[2] / 180.0;
            fzrad[3] = Math.PI * fz[3] / 180.0;

            X  = Convert.ToInt32(textBox6.Text); Y = Convert.ToInt32(textBox7.Text);
            X1 = Convert.ToInt32(textBox9.Text); Y1 = Convert.ToInt32(textBox10.Text); 
            N  = Convert.ToInt32(textBox8.Text);

            xm = Convert.ToDouble(textBox1.Text);
            lm = Convert.ToDouble(textBox2.Text);
            dm = Convert.ToDouble(textBox3.Text);
            //k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);                  // Номер Complex

            //OnFrenelN(xm * 1000, lm, dm * 1000, k1, k2);        // в микрометрах

            OnInterf_XY(fzrad, xm * 1000, lm, dm * 1000, X, Y, X1, Y1, N);
            Close();
        }

     
    }
}

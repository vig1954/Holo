using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public delegate void ModelSin(double[] fz);
public delegate void ModelSinG(double[] fz, double gamma, double N_pol);
public delegate void ModelSinG_kr(double[] fz, double N_urovn, double gamma, double N_pol, int k, int N, double noise);
public delegate void ModelSinD(double[] fz, double N_pol, int kvant, int N_urovn);
public delegate void ModelExp(double g, int N);


namespace rab1.Forms
{
    public partial class Model_Sin : Form
    {
        public event ModelSinG_kr OnModelSin;
        public event ModelSinG_kr OnModelSin1;
        public event ModelSinG    OnModelWB;
        public event ModelSinD    OnModel_Dithering;
        public event ModelSinD    OnModel_DitheringVZ;
        public event ModelSin     OnModelAtan2;
        public event ModelSin     OnModelAtan2_L;
        public event ModelExp     OnModelExp;

        private static double[] fz = { 0.0, 90.0, 180.0, 270.0 };
        private static double gamma = 1;
        private static double N_pol = 4;
        private static int    N_kvant = 2;
        private static double    N_urovn = 255;         // Амплитуда
        private static int    kr = 0;                   // Разрядить нулями (0 - не разряжать, 1 - через 1)
        private static int    N = 1024;                 // Размер массива
        private static double noise = 0.1;              // Шум от амплитуды

        public Model_Sin()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(fz[0]); textBox2.Text = Convert.ToString(fz[1]); textBox3.Text = Convert.ToString(fz[2]); textBox4.Text = Convert.ToString(fz[3]);
            textBox5.Text = Convert.ToString(gamma);
            textBox6.Text = Convert.ToString(N_pol);
            textBox7.Text = Convert.ToString(N_kvant);
            textBox8.Text = Convert.ToString(N_urovn);
            textBox9.Text = Convert.ToString(kr);
            textBox10.Text = Convert.ToString(N);
            textBox11.Text = Convert.ToString(noise);
        }

        private void button1_Click(object sender, EventArgs e)                             // Смоделировать 4 синусоиды с N_pol полосами
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;
            gamma = Convert.ToDouble(textBox5.Text);
            N_pol = Convert.ToDouble(textBox6.Text);
            N_urovn = Convert.ToDouble(textBox8.Text);   // Амплитуда
            kr = Convert.ToInt32(textBox9.Text);
            N = Convert.ToInt32(textBox10.Text);
            noise = Convert.ToDouble(textBox11.Text);

            OnModelSin(fzrad, N_urovn, gamma, N_pol, kr, N, noise);
            Close();
        }

        private void button8_Click(object sender, EventArgs e)                         // Смоделировать 4 синусоиды с размером периода N_pol
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;
            gamma = Convert.ToDouble(textBox5.Text);
            N_pol = Convert.ToDouble(textBox6.Text);
            N_urovn = Convert.ToDouble(textBox8.Text);   // Амплитуда
            kr = Convert.ToInt32(textBox9.Text);
            N = Convert.ToInt32(textBox10.Text);
            noise = Convert.ToDouble(textBox11.Text);

            OnModelSin1(fzrad, N_urovn, gamma, N_pol, kr, N, noise);
            Close();
        }

        private void button7_Click(object sender, EventArgs e)     // В текущий комплексный массив exp(-iw)
        {
            N = Convert.ToInt32(textBox10.Text);
            gamma = Convert.ToDouble(textBox5.Text);
            OnModelExp(gamma, N);
            Close();
        }
        private void button4_Click(object sender, EventArgs e)    // Смоделировать 4 черно-белые
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;
            gamma = Convert.ToDouble(textBox5.Text);
            N_pol = Convert.ToDouble(textBox6.Text);

            OnModelWB(fzrad, gamma, N_pol);
            Close();
        }

        private void button5_Click(object sender, EventArgs e)  // Смоделировать 4 dithering (Алгоритм Флойда-Стейнберга)
        {
            double[] fzrad = new double[4];
            fz[0]   = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1]   = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2]   = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3]   = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;
          
            N_pol   = Convert.ToDouble(textBox6.Text);
            N_kvant = Convert.ToInt32(textBox7.Text);
            N_urovn = Convert.ToDouble(textBox8.Text);
            int amp = (int)N_urovn;
            OnModel_Dithering(fzrad,  N_pol, N_kvant,  amp);
            Close();
        }

        private void button6_Click(object sender, EventArgs e) // Смоделировать 4 dithering (Матрица возбуждения)
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            N_pol = Convert.ToDouble(textBox6.Text);
            N_kvant = Convert.ToInt32(textBox7.Text);
            N_urovn = Convert.ToDouble(textBox8.Text);
            int amp = (int)N_urovn;
            OnModel_DitheringVZ(fzrad, N_pol, N_kvant, amp);
            Close();
        }

       


        private void button2_Click(object sender, EventArgs e)                       //   ATAN2
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            OnModelAtan2(fzrad);
            Close();

        }

        private void button3_Click(object sender, EventArgs e)                      // Фигура Лиссажу
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            OnModelAtan2_L(fzrad);
            Close();
        }

        

     
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void DelegatPSI(double[] fz, double am);
public delegate void DelegatPSI1(int k1, int k2, int k3, int k4, int k5, double[] fz);
public delegate void DelegatIMAX(int imax);
public delegate void DelegatMaska(int k1, int k2, int k3);
public delegate void DelegatLis(double[] fz);

namespace rab1.Forms
{
    public partial class PSI : Form
    {
        public event DelegatPSI    OnPSI;
        public event DelegatPSI1   OnPSI1;
        public event DelegatIMAX   OnIMAX;
        public event DelegatIMAX   OnIMAX1;
        public event DelegatMaska  OnMaska;
        public event DelegatLis    OnLis;


        private static double[] fz = { 0.0, 90.0, 180.0, 270.0 };
        private static double am = 255;
        private static int imax = 255;
        private static int k1 = 1;
        private static int k2 = 2;
        private static int k3 = 3;
        private static int k4 = 4;
        private static int k5 = 5;
        private static int km1 = 1;
        private static int km2 = 2;
        private static int km3 = 3;

        public PSI()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(fz[0]); 
            textBox2.Text = Convert.ToString(fz[1]); 
            textBox3.Text = Convert.ToString(fz[2]); 
            textBox4.Text = Convert.ToString(fz[3]);
            textBox5.Text = Convert.ToString(am);
            textBox6.Text = Convert.ToString(imax);

            textBox8.Text = Convert.ToString(k1);
            textBox9.Text = Convert.ToString(k2);
            textBox10.Text = Convert.ToString(k3);
            textBox11.Text = Convert.ToString(k4);
            textBox12.Text = Convert.ToString(k5);

            textBox13.Text = Convert.ToString(km1);
            textBox14.Text = Convert.ToString(km2);
            textBox7.Text = Convert.ToString(km3);

        }
       
        private void button1_Click(object sender, EventArgs e)  // PSI  (амплитуда и фаза)
        {
 
            fz[0] = Convert.ToDouble(textBox1.Text); 
            fz[1] = Convert.ToDouble(textBox2.Text); 
            fz[2] = Convert.ToDouble(textBox3.Text); 
            fz[3] = Convert.ToDouble(textBox4.Text);
            am = Convert.ToDouble(textBox5.Text);
            
            double[] fzrad = new double[4];                 // Фаза в радианах
            fzrad[0] = Math.PI * fz[0] / 180.0;
            fzrad[1] = Math.PI * fz[1] / 180.0;
            fzrad[2] = Math.PI * fz[2] / 180.0;
            fzrad[3] = Math.PI * fz[3] / 180.0;
            
            OnPSI( fzrad, am);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)  // Квантование 4 кадров (8,9,10,11)
        {
            imax = Convert.ToInt32(textBox6.Text);
            OnIMAX(imax);
            Close();
        }

        private void button3_Click(object sender, EventArgs e)  // Квантование одного кадра
        {
            imax = Convert.ToInt32(textBox6.Text);
            OnIMAX1(imax);
            Close();
        }

        private void button4_Click(object sender, EventArgs e)  // PSI  1,2,3,4 -> 5
        {
            k1 = Convert.ToInt32(textBox8.Text);
            k2 = Convert.ToInt32(textBox9.Text);
            k3 = Convert.ToInt32(textBox10.Text);
            k4 = Convert.ToInt32(textBox11.Text);
            k5 = Convert.ToInt32(textBox12.Text);

            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            fz[3] = Convert.ToDouble(textBox4.Text);

            double[] fzrad = new double[4];                 // Фаза в радианах
            fzrad[0] = Math.PI * fz[0] / 180.0;
            fzrad[1] = Math.PI * fz[1] / 180.0;
            fzrad[2] = Math.PI * fz[2] / 180.0;
            fzrad[3] = Math.PI * fz[3] / 180.0;
            
            OnPSI1(k1-1, k2-1, k3-1, k4-1, k5-1, fzrad);
            Close();

        }

        private void button5_Click(object sender, EventArgs e)  // Наложение маски
        {
            km1 = Convert.ToInt32(textBox13.Text);
            km2 = Convert.ToInt32(textBox14.Text);
            km3 = Convert.ToInt32(textBox7.Text);
            OnMaska(km1-1 , km2 - 1, km3 - 1);
            Close();
        }

        private void button6_Click(object sender, EventArgs e)  //  Фигура Лиссажу данные из 9, 10, 11, 12
        {
            fz[0] = Convert.ToDouble(textBox1.Text);
            fz[1] = Convert.ToDouble(textBox2.Text);
            fz[2] = Convert.ToDouble(textBox3.Text);
            fz[3] = Convert.ToDouble(textBox4.Text);
        

            double[] fzrad = new double[4];                 // Фаза в радианах
            fzrad[0] = Math.PI * fz[0] / 180.0;
            fzrad[1] = Math.PI * fz[1] / 180.0;
            fzrad[2] = Math.PI * fz[2] / 180.0;
            fzrad[3] = Math.PI * fz[3] / 180.0;

            OnLis(fzrad);
            Close();

        }
    }
}

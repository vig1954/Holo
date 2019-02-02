using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void DelegatPSI(double[] fz, double am);
public delegate void DelegatIMAX(int imax);
//public delegate void DelegatMaska(int k1, int k2, int k3);
public delegate void DelegatLis(double[] fz);
public delegate void DelegatSdvg();

namespace rab1.Forms
{
    public partial class PSI : Form
    {
        public event DelegatPSI    OnPSI;
        public event DelegatLis    OnPSI1;
        public event DelegatSdvg   OnPSI_Carre;
        public event DelegatIMAX   OnIMAX;
        public event DelegatIMAX   OnIMAX1;
       // public event DelegatMaska  OnMaska;
        public event DelegatSdvg   OnSdvg;


        private static double[] fz = { 0.0, 90.0, 180.0, 270.0, 0.0, 90.0, 180.0, 270.0 };
        private static double am = 255;
        private static int imax = 255;

        private static int n_sdv = 4;   // Число фазовых сдвигов   textBox11

        //private static int km1 = 1;
        //private static int km2 = 2;
        //private static int km3 = 3;

        public PSI()
        {
            InitializeComponent();
            textBox1.Text  = Convert.ToString(fz[0]); 
            textBox2.Text  = Convert.ToString(fz[1]); 
            textBox3.Text  = Convert.ToString(fz[2]); 
            textBox4.Text  = Convert.ToString(fz[3]);
            textBox7.Text  = Convert.ToString(fz[4]);
            textBox8.Text  = Convert.ToString(fz[5]);
            textBox9.Text  = Convert.ToString(fz[6]);
            textBox10.Text = Convert.ToString(fz[7]);


            textBox5.Text = Convert.ToString(am);
            textBox6.Text = Convert.ToString(imax);

            textBox11.Text = Convert.ToString(n_sdv); // Число фазовых сдвигов 

            // textBox13.Text = Convert.ToString(km1);
            //  textBox14.Text = Convert.ToString(km2);
            // textBox7.Text = Convert.ToString(km3);

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

        private void button4_Click(object sender, EventArgs e)  // PSI  regCpmplex -> Главное окно
        {

            n_sdv = Convert.ToInt32(textBox11.Text);
            double[] fzrad = new double[n_sdv];

            fz[0] = Convert.ToDouble(textBox1.Text); 
            fz[1] = Convert.ToDouble(textBox2.Text); 
            fz[2] = Convert.ToDouble(textBox3.Text); 
            fz[3] = Convert.ToDouble(textBox4.Text); 
            fz[4] = Convert.ToDouble(textBox7.Text); 
            fz[5] = Convert.ToDouble(textBox8.Text); 
            fz[6] = Convert.ToDouble(textBox9.Text); 
            fz[7] = Convert.ToDouble(textBox10.Text);

            for (int i=0; i<n_sdv; i++) fzrad[i] = Math.PI * fz[i] / 180.0;

            OnPSI1(fzrad);
            Close();

        }
        private void button5_Click(object sender, EventArgs e) // PSI Carre regCpmplex -> Главное окно
        { 
            OnPSI_Carre();
            Close();
        }

        /*      private void button5_Click(object sender, EventArgs e)  // Наложение маски
               {
                   km1 = Convert.ToInt32(textBox13.Text);
                   km2 = Convert.ToInt32(textBox14.Text);
                   km3 = Convert.ToInt32(textBox7.Text);
                  // OnMaska(km1-1 , km2 - 1, km3 - 1);
                   Close();
               }
       */
        private void button6_Click(object sender, EventArgs e)  //  Угол из regComplex
        {
            OnSdvg();
            Close();

        }

        
    }
}

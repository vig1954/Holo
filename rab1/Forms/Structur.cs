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
    public delegate void Correct(double L, double d, double d1);
    public partial class Structur : Form
    {
        public event Correct On_Corr;
        public event Correct On_CorrX;

        private static double L = 2000;  // Расстояние до объекта
        private static double d = 2000;  // Расстояние от начала объекта до камеры
        private static double d1 = 400;  //Размер объекта

        public Structur()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(d);
            textBox2.Text = Convert.ToString(L);
            textBox3.Text = Convert.ToString(d1);
            
        }

        private  void button2_Click(object sender, EventArgs e)  // Определить углы
        {
            d  = Convert.ToDouble(textBox1.Text);
            L  = Convert.ToDouble(textBox2.Text);
            d1 = Convert.ToDouble(textBox3.Text);

            double f1 = Math.Atan(L / d);       f1 = f1 * 180 / Math.PI;
            double f2 = Math.Atan(L / (d-d1));  f2 = f2 * 180 / Math.PI;
            textBox4.Text = Convert.ToString(f1);
            textBox5.Text = Convert.ToString(f2);
            Show();
        }

        private void button1_Click(object sender, EventArgs e) // Скорректировать высоты
        {
            d = Convert.ToDouble(textBox1.Text);
            L = Convert.ToDouble(textBox2.Text);
            d1 = Convert.ToDouble(textBox3.Text);
            On_Corr(L, d, d1);
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            d = Convert.ToDouble(textBox1.Text);
            L = Convert.ToDouble(textBox2.Text);
            d1 = Convert.ToDouble(textBox3.Text);
            On_CorrX(L, d, d1);
            Close();
        }
    }
}

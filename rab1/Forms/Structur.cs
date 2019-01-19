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
    public delegate void Correct1(double L, double d, double d1, double x_max);
    public delegate void Scale(double x, int n);


    public partial class Structur : Form
    {
        public event Correct1 On_Corr;
        public event Correct On_CorrX;
        public event Scale   On_Scale;

        private static double L = 2000;    // Расстояние до объекта
        private static double d = 2000;    // Расстояние от начала объекта до камеры
        private static double d1 = 400;    // Размер объекта
        private static double x_max = 1;   // Максимальное смещение объекта

        private static int Number_line = 247;   // Максимальное смещение объекта

        public Structur()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(d);
            textBox2.Text = Convert.ToString(L);
            textBox3.Text = Convert.ToString(d1);
            textBox6.Text = Convert.ToString(x_max);
            textBox7.Text = Convert.ToString(Number_line);
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
            x_max = Convert.ToDouble(textBox6.Text);   // Максисальное смещение
            On_Corr(L, d, d1, x_max);
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

        private void button4_Click(object sender, EventArgs e)  // Масштабирование центрального окна
        {
            x_max       = Convert.ToDouble(textBox6.Text);
            Number_line = Convert.ToInt32(textBox7.Text);
            On_Scale(x_max, Number_line);
            Close();
        }
    }
}

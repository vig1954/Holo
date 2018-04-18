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

    public delegate void ADD_Complex(int k1, int k2);
    public delegate void Send_Complex(int k1, int k2);
    public delegate void Sub_Complex(int k3, int k4, int k5);
    public delegate void Sub_Double(int k6, int k7, int k8);
    public delegate void ROR_Double(int k9);
    public delegate void TRNS_Double();
    public delegate void ABS_Double();
    // public delegate void ROR_Double1(int k9, Form1 f);

    public partial class ADD_Cmplx : Form
    {

        public event ADD_Complex  On_ADD;
       
        public event Send_Complex On_Send;
        public event Sub_Complex  On_Sub;
        public event Sub_Complex  On_Mul;
        public event Sub_Complex  On_Add;
        public event Sub_Complex  On_Div;

        public event Sub_Double On_Sub_Double;
        public event Sub_Double On_ADD_Double;
        public event Sub_Double On_Div_Double;
        public event Sub_Complex On_MulD;
        public event Sub_Double On_Conv;

        public event ROR_Double On_ROR;
        public event ROR_Double On_ROL;

        public event ROR_Double On_ROR_CMPLX;
        public event ROR_Double On_ROL_CMPLX;

        public event TRNS_Double On_TRNS;
        public event TRNS_Double On_ROT180;
        public event ABS_Double On_ABS;

        public event ADD_Complex On_Pirs;

        private static int k1 = 2;
        private static int k2 = 3;
        private static int k3 = 3;
        private static int k4 = 2;
        private static int k5 = 3;

        private static int k6 = 1;
        private static int k7 = 2;
        private static int k8 = 3;
        private static int k9 = 2;
        private static int k10 = 2;




        public ADD_Cmplx()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(k1);
            textBox2.Text = Convert.ToString(k2);
            textBox3.Text = Convert.ToString(k3);
            textBox4.Text = Convert.ToString(k4);
            textBox5.Text = Convert.ToString(k5);
            textBox6.Text = Convert.ToString(k6);
            textBox7.Text = Convert.ToString(k7);
            textBox8.Text = Convert.ToString(k8);
            textBox9.Text = Convert.ToString(k9);
            textBox10.Text = Convert.ToString(k10);
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox2.Text);

            On_ADD(k1, k2);     //  += комплекные массивы
            Close();
        }

        private void button4_Click(object sender, EventArgs e)  // Переслать массивы
        {
            k1 = Convert.ToInt32(textBox1.Text);
            k2 = Convert.ToInt32(textBox2.Text);

            On_Send(k1, k2);     // Сложить комплекные массивы
            Close();
        }

        private void button2_Click(object sender, EventArgs e)  // Минус
        {
            k3 = Convert.ToInt32(textBox3.Text);
            k4 = Convert.ToInt32(textBox4.Text);
            k5 = Convert.ToInt32(textBox5.Text);

            On_Sub(k3, k4, k5);     // Вычесть комплекные массивы
            Close();
        }

        private void button3_Click(object sender, EventArgs e)  
        {
            k3 = Convert.ToInt32(textBox3.Text);
            k4 = Convert.ToInt32(textBox4.Text);
            k5 = Convert.ToInt32(textBox5.Text);

            On_Add(k3, k4, k5);     // Вычесть комплекные массивы
            Close();

        }
        private void button9_Click(object sender, EventArgs e) // Разделить
        {
            k3 = Convert.ToInt32(textBox3.Text);
            k4 = Convert.ToInt32(textBox4.Text);
            k5 = Convert.ToInt32(textBox5.Text);

            On_Div(k3, k4, k5);     // Разделить комплекные массивы
            Close();
        }
        private void button5_Click_1(object sender, EventArgs e)   // Вычесть вещественные массивы
        {
            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);
            k8 = Convert.ToInt32(textBox8.Text);
            //MessageBox.Show("k1=" + k6 + " k2 =" + k7 + " k3 =" + k8);
            On_Sub_Double(k6, k7, k8);     
            Close();
        }

        private void button6_Click_1(object sender, EventArgs e)   // Сложить вещественные массивы
        {
            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);
            k8 = Convert.ToInt32(textBox8.Text);

            On_ADD_Double(k6, k7, k8);     
            Close();

        }

        private void button10_Click_1(object sender, EventArgs e) // Разделить вещественные массивы
        {
            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);
            k8 = Convert.ToInt32(textBox8.Text);

            On_Div_Double(k6, k7, k8);
            Close();

        }
/*
        private void button7_Click(object sender, EventArgs e)  // Циклический сдвиг вправо
        {
            k9 = Convert.ToInt32(textBox9.Text);

            On_ROR(k9);
            Close();
        }

        private void button8_Click(object sender, EventArgs e)  // Циклический сдвиг влево
        {
            k9 = Convert.ToInt32(textBox9.Text);

            On_ROL(k9);
            Close();
        }
*/
        private void button11_Click(object sender, EventArgs e)  // Циклический сдвиг вправо комплексных чисел
        {
            k10 = Convert.ToInt32(textBox10.Text);

            On_ROR_CMPLX(k10);
            Close();
        }

        private void button12_Click(object sender, EventArgs e)  // Циклический сдвиг влево комплексных чисел
        {
            k10 = Convert.ToInt32(textBox10.Text);

            On_ROL_CMPLX(k10);
            Close();
        }

        private void button7_Click_1(object sender, EventArgs e)  // Циклический сдвиг вправо
        {
            k9 = Convert.ToInt32(textBox9.Text);

            On_ROR(k9);
            Close();
        }

        private void button8_Click_1(object sender, EventArgs e)  // Циклический сдвиг влево
        {
            k9 = Convert.ToInt32(textBox9.Text);

            On_ROL(k9);
            Close();
        }

        private void button13_Click(object sender, EventArgs e)  // Транспонирование текущего кадра
        {
            On_TRNS();
            Close();
        }
        private void button18_Click(object sender, EventArgs e) // Поворот текущего кадра на 180 градусов
        {
            On_ROT180();
            Close();
        }

        private void button17_Click(object sender, EventArgs e) // Умножение вещественных массивов
        {
            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);
            k8 = Convert.ToInt32(textBox8.Text);

            On_MulD(k6, k7, k8);
            Close();
        }

        private void button14_Click(object sender, EventArgs e)  // Умножение комплексных массивов
        {
            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);
            k8 = Convert.ToInt32(textBox8.Text);

            On_Mul(k6, k7, k8);
            Close();
        }

        private void button15_Click(object sender, EventArgs e) // Свертка
        {
            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);
            k8 = Convert.ToInt32(textBox8.Text);

            On_Conv(k6, k7, k8);
            Close();
        }

        private void button16_Click(object sender, EventArgs e) // Линейный коэффициент корреляции r-Пирсона
        {
            k6 = Convert.ToInt32(textBox6.Text);
            k7 = Convert.ToInt32(textBox7.Text);
            On_Pirs(k6, k7);
            Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            On_ABS();
            Close();
        }



        /*
private void button5_Click_1(object sender, EventArgs e)
{

}

private void button6_Click_1(object sender, EventArgs e)
{

}

private void button10_Click_1(object sender, EventArgs e)
{

}
*/
    }
}

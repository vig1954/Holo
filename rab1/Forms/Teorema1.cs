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
    public partial class Teorema1 : Form
    {

        public static VisualRegImageDelegate VisualComplex = null;

        private static int Number_kadr     = 1;
        private static int Number_Pont_Rec = 1024;
        private static int Number_Pont     = 2048;
        private static int Number_Period   = 2;
        private static int dx = 8;   // Шаг дискретизации

        private static int k1 = 1;
        private static int k2 = 2;
        private static int k3 = 1;   // Для синусоиды
        private static int k4 = 1;   // Для синусоиды умноженной на прямоугольник
        private static int k5 = 1;   // Для дискретизации

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
            Number_Pont_Rec = Convert.ToInt32(textBox2.Text);
            Number_Pont     = Convert.ToInt32(textBox3.Text);
            int regComplex  = Number_kadr - 1;

            int nx = Number_Pont;
            int ny = 100;
            int x0 = Number_Pont / 2 - Number_Pont_Rec / 2;

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
            Number_Period = Convert.ToInt32(textBox6.Text);
            k3 = Convert.ToInt32(textBox7.Text);
            int regComplex = k3 - 1;
            Number_Pont = Convert.ToInt32(textBox3.Text);

            int nx = Number_Pont;
            int ny = 100;

            double fz = 0, a1 = 1;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                   { //cmpl.array[i, j] = a1 * (Math.Sin(2.0 * Math.PI * i / Number_Period - fz) + 1.0) / 2.0;     // Число точек
                     cmpl.array[i, j] = a1 * (Math.Sin(2.0 * Math.PI * i * Number_Period / nx - fz) + 1.0) / 2.0;  // Число периодов
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

        private void button3_Click(object sender, EventArgs e)
        {
            k1 = Convert.ToInt32(textBox4.Text);
            k2 = Convert.ToInt32(textBox5.Text);

            int nx = Form1.zComplex[k1 - 1].width;
            int ny = Form1.zComplex[k1 - 1].height;

            int m = Furie.PowerOfTwo(nx);                                       // nx=2**m
            //MessageBox.Show(" m = " + m + " nx = " + nx + " ny = " + ny);

            Complex[] c = new Complex[nx];
            Complex[] c1 = new Complex[nx];

            for (int i = 0; i < nx; i++) c[i] = Form1.zComplex[k1 - 1].array[i, ny / 2];

            c1 = Furie.GetInverseFourierTransform(c, m);

            Form1.zComplex[k2 - 1] = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[k2 - 1].array[i, j] = c1[i];

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
            k5 = Convert.ToInt32(textBox9.Text);
            dx = Convert.ToInt32(textBox10.Text);

            int nx = Form1.zComplex[k5 - 1].width;
            int ny = Form1.zComplex[k5 - 1].height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < nx; i=i+dx)
                for (int j = 0; j < ny; j++)
                    cmpl.array[i, j] = Form1.zComplex[k1 - 1].array[i, j];

            Form1.zComplex[k1 - 1] = cmpl;
            VisualComplex(k1 - 1);
        }
    }
}

﻿using System;
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

    public delegate void CorrectBr1(int k1, int k2, int n);
    public delegate void CorrectBr2(int k1, int k2);
    public delegate void CorrectBr3(int k1, int k2, int N, int nx, int ny);
    public partial class CorrectBr : Form
    {
        public static VisualRegImageDelegate VisualRegImage = null;  // Визувлизация одного кадра от 0 до 11

        public event CorrectBr1 On_CorrectX;
        public event CorrectBr1 On_CorrectG;
       // public event CorrectBr1 On_CorrectClin;
        public event CorrectBr2 On_CorrectSumm;
        public event CorrectBr3 On_CorrectGxy;

        private static int n = 4096;                  // Размер массива
        private static int k1 = 1;
        private static int k2 = 2;

        private static int k11 = 1;
        private static int k21 = 2;
        private static int k31 = 5;

        private static int k12 = 1;
        private static int I0 = 0;

        private static int k13 = 1;
        private static int k23 = 2;

        private static int k14 = 1;
        private static int k24 = 2;
        private static int N_Line = 128;
        private static int nx = 4096;                  // Размер массива
        private static int ny = 2160;                  // Размер массива
        public CorrectBr()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(k1);     // Номер кадра
            textBox3.Text = Convert.ToString(k2);     // Номер кадра
            textBox2.Text = Convert.ToString(n);      // Размер массива

            textBox4.Text = Convert.ToString(k11);
            textBox5.Text = Convert.ToString(k21);
            textBox6.Text = Convert.ToString(k31);

            textBox7.Text = Convert.ToString(I0);
            textBox8.Text = Convert.ToString(k12);

            textBox11.Text = Convert.ToString(k13);    // Сложение строк
            textBox12.Text = Convert.ToString(k23);

            textBox9.Text = Convert.ToString(k14);    // Сложение строк
            textBox10.Text = Convert.ToString(k24);
            textBox13.Text = Convert.ToString(N_Line);
            textBox14.Text = Convert.ToString(nx);
            textBox15.Text = Convert.ToString(ny);
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
        /// Определения клина с обратными гамма-искажениями
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
            I0 = Convert.ToInt32(textBox7.Text);
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);

            double gamma = 1;
            int nu = 255;                                               // Число уровней
            Form1.zArrayDescriptor[k12-1] = Model_Sinus.Intensity1(nu, I0, nx, ny, gamma);
            VisualRegImage(k12-1);

            //On_CorrectClin(I0, nx, k12 - 1);
            Close();

        }
        /// <summary>
        /// Клин с 32 значениями интенсивности из массива
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //double[] cl = { 1,  2,  3,  4,  5,  6,   7,   8,   9,  10,  11,  12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,  23,  24,  25,  26,  27,  28, 29,   30,  31, 32 };
            //double[] cl = { 0,  1,  2,  3,  4,  5,   6,   7,   8,   9,  10,  11,  12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,  23,  24,  25,  26,  27, 28,   29,  30, 31 };
            //double[] cl = { 7, 15, 23, 31, 39, 47,  55,  63,  71, 79,  87,  95, 103, 111, 119, 127, 135, 143, 151, 159, 167, 175, 183, 191, 199, 207, 215, 223, 231, 239, 247, 255 };
              double[] cl = { 7, 15, 23, 31, 39, 47,  55,  63,  71, 79,  87,  95, 103, 111, 119, 127, 135, 143, 151, 159, 167, 175, 183, 191, 199, 207, 215, 223, 231, 239, 247, 255 };

            k12 = Convert.ToInt32(textBox8.Text);
            //I0 = Convert.ToInt32(textBox7.Text);
            nx = Convert.ToInt32(textBox14.Text);                       // Текущий размер
            ny = Convert.ToInt32(textBox15.Text);

            int dx = 100;
            int Nx1 = nx + dx * 2; ;
            

            ZArrayDescriptor cmpl = new ZArrayDescriptor(Nx1, ny);

            double[] am = new double[nx ];
            

            for (int i = 0; i < nx; i++) { am[i] = cl[i / 128]; }      // 0-31
           

            for (int i = 0; i < nx; i++)
               for (int j = 0; j < ny; j++)
                   {  cmpl.array[i + dx, j] = am[i];  }

            cmpl = Model_Sinus.Intens(255, 0, dx, cmpl);     // Белая и черная полоса по краям

            Form1.zArrayDescriptor[k12 - 1] = cmpl;
            VisualRegImage(k12 - 1);

        }


        /// <summary>
        /// Сложение строк от Y1 до Y2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            k13 = Convert.ToInt32(textBox11.Text);
            k23 = Convert.ToInt32(textBox12.Text);
            On_CorrectSumm(k13-1, k23 - 1);
            Close();

        }
        /// <summary>
        ///  Растяжение массива для проекции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
           // textBox9.Text = Convert.ToString(k14);    // Сложение строк
          
            k14 = Convert.ToInt32(textBox9.Text);
            k24 = Convert.ToInt32(textBox10.Text);
            N_Line = Convert.ToInt32(textBox13.Text);
            nx= Convert.ToInt32(textBox14.Text);
            ny = Convert.ToInt32(textBox15.Text);
            On_CorrectGxy(k14 - 1, k24 - 1, N_Line, nx, ny);
            Close();

        }

       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public delegate void DelegatUnrupLine(int x0, double gr);
public delegate void DelegatUnrup2pi();

namespace rab1.Forms
{
    public partial class UnRupLine : Form
    {
        private static double gr1 = -2.1;
        private static double gr2 = -2.1;
        private static int x0 = 4000;

        public event DelegatUnrupLine OnUnrupLine;
        public event DelegatUnrupLine OnUnrupLinePlus;
        public event DelegatUnrup2pi  OnUnrupLine2pi;

        public UnRupLine()
        {
            InitializeComponent();
            textBox4.Text = Convert.ToString(gr1);
            textBox1.Text = Convert.ToString(gr2);  
            textBox2.Text = Convert.ToString(x0);
        }

        private void button1_Click(object sender, EventArgs e)  // Развертка по строкам (Возрастание)
        {
            x0  = Convert.ToInt32(textBox2.Text);
            gr1 = Convert.ToDouble(textBox4.Text);
            OnUnrupLine(x0, gr1);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)  // Если меньше (убывание)
        {
            x0 = Convert.ToInt32(textBox2.Text);
            gr2 = Convert.ToDouble(textBox1.Text);
            OnUnrupLinePlus(x0, gr2);
            Close();
        }

        private void button3_Click(object sender, EventArgs e) // Обычная развертка
        {
            OnUnrupLine2pi();
            Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rab1
{
    public partial class RangeExtensionModelForm : Form
    {
        public RangeExtensionModelForm()
        {
            InitializeComponent();
        }

        private void BuildTableButton_Click(object sender, EventArgs e)
        {
            int m1 = int.Parse(ValueM1TextBox.Text);
            int m2 = int.Parse(ValueM1TextBox.Text);

            BuildTable(m1, m2);
        }

        private void BuildTable(int m1, int m2)
        {
            for (int n1 = 0; n1 < m1; n1++)
            {
                for (int n2 = 0; n2 < m2; n2++)
                {
                    
                }
            }
        }

        private void ValueM1TextBox_TextChanged(object sender, EventArgs e)
        {
            CalculateMaxRange();
        }

        private void ValueM2TextBox_TextChanged(object sender, EventArgs e)
        {
            CalculateMaxRange();
        }

        private void CalculateMaxRange()
        {
            int m1;
            int m2;

            if (int.TryParse(ValueM1TextBox.Text, out m1) && int.TryParse(ValueM2TextBox.Text, out m2))
            {
                int maxRange = m1 * m2;
                MaxRangeValueLabel.Text = maxRange.ToString();
            }
        }
    }
}

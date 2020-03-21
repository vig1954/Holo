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
            int m2 = int.Parse(ValueM2TextBox.Text);

            int range = int.Parse(RangeTextBox.Text);

            IList<Point2D> pointsList = BuildTable(m1, m2, range);

            GraphInfo graphInfo = new GraphInfo("Graphic", System.Windows.Media.Colors.Green, pointsList.ToArray(), true, false);

            IList<GraphInfo> graphCollection = new List<GraphInfo>() { graphInfo };

            ShowGraphic(graphCollection);
        }

        public List<Point2D> BuildTable(int m1, int m2, int range)
        {
            int M1 = m2;
            int M2 = m1;

            int N1 = CalculateN(M1, m1);
            int N2 = CalculateN(M2, m2);

            Dictionary<int, Point2D> pointsDictionary = new Dictionary<int, Point2D>();
            
            for (int b1 = 0; b1 < m1; b1++)
            {
                for (int b2 = 0; b2 < m2; b2++)
                {
                    int value = (M1 * N1 * b1 + M2 * N2 * b2) % (m1 * m2);
                    if (value <= range)
                    {
                        Point2D point = new Point2D(b1, b2);
                        pointsDictionary.Add(value, point);
                    }
                }
            }

            pointsDictionary = pointsDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            List<Point2D> pointsList = pointsDictionary.Select(x => x.Value).ToList();

            return pointsList;
              
        }

        private int CalculateN(int M, int m)
        {
            int n = 1;
            int value = (M * n) % m;
            while(value != 1)
            {
                n++;
                value = (M * n) % m;
            }
            return n;
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

        private void ShowGraphic(IList<GraphInfo> graphCollection)
        {
            GraphFormHost graphFormHost = new GraphFormHost();
            graphFormHost.GraphInfoCollection = graphCollection;

            Form form = new Form();
            form.Height = 300;
            form.Width = 900;
            graphFormHost.Dock = DockStyle.Fill;
            form.Controls.Add(graphFormHost);
            form.Show();
        }
    }
}

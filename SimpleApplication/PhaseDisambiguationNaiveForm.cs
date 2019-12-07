using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Processing;

namespace SimpleApplication
{
    public partial class PhaseDisambiguationNaiveForm : Form
    {
        private readonly ImageHandlerDataSource[] _dataSources;

        public PhaseDisambiguationNaiveForm(Dictionary<string, ImageHandler> dataSources)
        {
            InitializeComponent();
            _dataSources = dataSources.Select(p => new ImageHandlerDataSource(p.Key, p.Value)).ToArray();
        }

        private void PhaseDisambiguationNaiveForm_Load(object sender, EventArgs e)
        {
            foreach (var dataSource in _dataSources)
            {
                SourceSelector.Items.Add(dataSource);
            }
        }
    }
}
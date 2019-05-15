using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserInterface.DataEditors;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataProcessorViews;

namespace UserInterface.WorkspacePanel
{
    public partial class DataProcessorSettingsForm : Form
    {
        private readonly IDataProcessorView _dataProcessorView;

        private InterfaceController _interfaceController;

        public DataProcessorSettingsForm(IDataProcessorView dataProcessorView)
        {
            InitializeComponent();

            _dataProcessorView = dataProcessorView;
        }

        private void DataProcessorSettingsForm_Load(object sender, EventArgs e)
        {
            var propertyListManager = new PropertyListManager {AutoWidth = true};
            _interfaceController = new InterfaceController(panel1, propertyListManager);
            _interfaceController.BindObjectToInterface(_dataProcessorView);

            Text = _dataProcessorView.Info.Name;

            Height = propertyListManager.PreferredSize.Height + 50;
        }
    }
}

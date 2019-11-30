using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure;
using UserInterface.DataEditors;
using UserInterface.DataEditors.InterfaceBinding;

namespace Camera
{
    public partial class CameraSettingsForm : Form
    {
        private IInterfaceController _interfaceController;
        private CameraSettings _cameraSettings;

        public CameraSettingsForm(CameraSettings cameraSettings)
        {
            InitializeComponent();
            
            _cameraSettings = cameraSettings ?? throw new InvalidOperationException($"{nameof(cameraSettings)} can't be null.");
        }

        private void CameraSettingsForm_Load(object sender, EventArgs e)
        {
            _interfaceController = new InterfaceController(panel1, new PropertyListManager());
            _interfaceController.BindObjectToInterface(_cameraSettings);

            

            _cameraSettings.SyncCameraSettings();
            _cameraSettings.Load();
        }

        private void CameraSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _cameraSettings.Save();
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}

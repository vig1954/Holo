using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Processing;
using UserInterface.DataEditors.InterfaceBinding;

namespace Camera
{
    public partial class CameraInputViewForm : Form
    {
        private bool _settingsLoaded = false;
        private CameraInput _cameraInput;
        private InterfaceController _interfaceController;

        public event Action<IImageHandler> OnImageCreate;

        public CameraInputViewForm()
        {
            InitializeComponent();
        }

        private void CameraInputViewForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
            _cameraInput = new CameraInput();
            _interfaceController = new InterfaceController(panel1);
            _interfaceController.BindObjectToInterface(_cameraInput);

            _cameraInput.PreviewImageUpdated += CameraInputOnPreviewImageUpdated;
            _cameraInput.OnImageCreate += image => OnImageCreate?.Invoke(image);
        }

        private void CameraInputOnPreviewImageUpdated()
        {
            pictureBox1.Image = _cameraInput.LastPreviewImage;
        }

        private void CameraInputViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cameraInput.PreviewImageUpdated -= CameraInputOnPreviewImageUpdated;
            _cameraInput.Dispose();
            _cameraInput = null;
        }

        private void btnToggleAppearance_Click(object sender, EventArgs e)
        {
            splitContainer1.Orientation = splitContainer1.Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;

            SaveSettings();
        }

        private void LoadSettings()
        {
            var settings = Properties.Settings.Default;
            
            if (settings.SettingsInitialized)
            {
                this.Location = settings.WindowAppearance.Location;
                this.Size = settings.WindowAppearance.Size;
                splitContainer1.Orientation = settings.SplitContainerOrientation;
                splitContainer1.SplitterDistance = settings.SplitContainerFirstPanelSize;

                _settingsLoaded = true;
                return;
            }

            _settingsLoaded = true;
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (!_settingsLoaded)
                return;

            var settings = Properties.Settings.Default;
            settings.WindowAppearance = new Rectangle(Location, Size);
            settings.SplitContainerFirstPanelSize = splitContainer1.SplitterDistance;
            settings.SplitContainerOrientation = splitContainer1.Orientation;
            settings.SettingsInitialized = true;
            settings.Save();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SaveSettings();
        }

        private void CameraInputViewForm_Resize(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void CameraInputViewForm_Move(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}

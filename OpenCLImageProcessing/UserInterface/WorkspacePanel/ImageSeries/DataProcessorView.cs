using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure;
using Processing;
using UserInterface.DataProcessorViews;
using UserInterface.Events;

namespace UserInterface.WorkspacePanel.ImageSeries
{
    public partial class DataProcessorView : UserControl
    {
        private IDataProcessorView _dataProcessorView;

        private IImageHandler _dataProcessorViewValue => (IImageHandler) _dataProcessorView.GetOutputValues().Single();

        private EventManager EventManager => Singleton.Get<EventManager>();

        public DataProcessorView(IDataProcessorView dataProcessorView)
        {
            _dataProcessorView = dataProcessorView;

            _dataProcessorView.OnValueUpdated += DataProcessorViewOnValueUpdated;

            InitializeComponent();

            DataProcessorName.Text = _dataProcessorView.Info.Name;
        }

        private void DataProcessorViewOnValueUpdated()
        {
            OpenInEditor.Enabled = _dataProcessorViewValue != null;
        }

        private void OpenInEditor_Click(object sender, EventArgs e)
        {
            EventManager.Emit(new ShowInEditorEvent(_dataProcessorViewValue, this));
        }

        private void OpenSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new DataProcessorSettingsForm(_dataProcessorView);
            settingsForm.Location = Cursor.Position;
            settingsForm.Show();
        }

        private void OpenMenu_Click(object sender, EventArgs e)
        {

        }
    }
}

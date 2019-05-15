using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using Infrastructure;
using Processing;
using UserInterface.DataProcessorViews;
using UserInterface.Events;

namespace UserInterface.WorkspacePanel
{
    public class DataProcessorViewWorkspacePanelItemController : WorkspacePanelGroupableItemControllerBase
    {
        private IDataProcessorView DataProcessorView { get; set; }
        public DataProcessorViewWorkspacePanelItemController(IDataProcessorView dataProcessorView, WorkspacePanelItem view) : base(view)
        {
            DataProcessorView = dataProcessorView;
            DataProcessorView.OnValueUpdated += UpdateView;

            View.TitleChanged += t =>
            {
                var imageHandler = DataProcessorView.GetOutputValues().OfType<IImageHandler>().SingleOrDefault();

                if (imageHandler == null)
                    return;

                imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.Title, t);
            };

            View.IsOpenSettingsButtonVisible = true;
            View.IsShowInEditorButtonVisible = true;
            View.OnOpenSettingsClicked += ViewOnOpenSettingsClicked;
            View.OnShowInEditorClicked += ViewOnShowInEditorClicked;
        }

        private void ViewOnShowInEditorClicked()
        {
            var dataProcessorValue = GetDataProcessorValue();

            if (dataProcessorValue == null)
                return;

            Singleton.Get<EventManager>().Emit(new ShowInEditorEvent(dataProcessorValue, this));
        }

        private void ViewOnOpenSettingsClicked()
        {
            var settingsForm = new DataProcessorSettingsForm(DataProcessorView);
            settingsForm.Location = Cursor.Position;
            settingsForm.Show();
        }

        public override void UpdateView()
        {
            View.SetTitle(DataProcessorView.Info.Name);
            View.SetInfo(DataProcessorView.Info.Tooltip);

            View.IsShowInEditorButtonEnabled = GetDataProcessorValue() != null;

            var imageHandler = DataProcessorView.GetOutputValues().OfType<IImageHandler>().SingleOrDefault();

            if (imageHandler == null)
                return;

            if (imageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Thumbnail, out object thumbnail))
                View.SetImage((Image)thumbnail);
        }

        private IImageHandler GetDataProcessorValue() => (IImageHandler) DataProcessorView.GetOutputValues().Single();
    }
}

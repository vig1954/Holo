using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Processing;
using UserInterface.DataProcessorViews;

namespace UserInterface.WorkspacePanel
{
    public class DataProcessorViewWorkspacePanelItemController : WorkspacePanelItemControllerBase
    {
        private IDataProcessorView DataProcessorView { get; set; }
        public DataProcessorViewWorkspacePanelItemController(IDataProcessorView dataProcessorView, WorkspacePanelItem view) : base(view)
        {
            DataProcessorView = dataProcessorView;
            DataProcessorView.OnUpdated += UpdateView;

            View.TitleChanged += t =>
            {
                var imageHandler = DataProcessorView.GetOutputValues().OfType<IImageHandler>().SingleOrDefault();

                if (imageHandler == null)
                    return;

                imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.Title, t);
            };
        }

        public void UpdateView()
        {
            View.SetTitle(DataProcessorView.Info.Name);
            View.SetInfo(DataProcessorView.Info.Tooltip);

            var imageHandler = DataProcessorView.GetOutputValues().OfType<IImageHandler>().SingleOrDefault();

            if (imageHandler == null)
                return;

            if (imageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Thumbnail, out object thumbnail))
                View.SetImage((Image)thumbnail);
        }
    }
}

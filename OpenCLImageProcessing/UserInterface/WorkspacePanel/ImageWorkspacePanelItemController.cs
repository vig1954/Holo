using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Common;
using Infrastructure;
using Processing;
using UserInterface.Events;

namespace UserInterface.WorkspacePanel
{
    public class ImageWorkspacePanelItemController : WorkspacePanelGroupableItemControllerBase
    {
        private IImageHandler ImageHandler { get; set; }

        public ImageWorkspacePanelItemController(IImageHandler imageHandler, WorkspacePanelItem view) : base(view)
        {
            ImageHandler = imageHandler;
            
            if (ImageHandler != null)
            {
                View.TitleChanged += t => { ImageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.Title, t); };
                ImageHandler.ImageUpdated += data => UpdateView();
            }

            View.IsShowInEditorButtonVisible = true;
            View.OnShowInEditorClicked += ViewOnShowInEditorClicked;
        }

        private void ViewOnShowInEditorClicked()
        {
            Singleton.Get<EventManager>().Emit(new ShowInEditorEvent(ImageHandler, this));
        }

        public override void UpdateView()
        {
            ImageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Title, out object title);

            View.SetTitle((string) title ?? "Image");
            View.SetInfo($"{ImageHandler.Width} x {ImageHandler.Height}\n{ImageHandler.Format} {ImageHandler.PixelFormat}");

            if (ImageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Thumbnail, out object thumbnail))
                View.SetImage((Image) thumbnail);
        }
    }
}
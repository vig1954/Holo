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
using Processing.Utils;
using UserInterface.Events;

namespace UserInterface.WorkspacePanel.ImageSeries
{
    public partial class InputImage : UserControl
    {
        private IImageHandler _imageHandler;

        public IImageHandler ImageHandler => _imageHandler;

        public InputImage()
        {
            InitializeComponent();
        }

        public void SetImageHandler(IImageHandler imageHandler)
        {
            _imageHandler = imageHandler;

            _imageHandler.ImageUpdated += ImageHandlerOnImageUpdated;

            UpdateImage();
        }

        private void ImageHandlerOnImageUpdated(ImageUpdatedEventData obj)
        {
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (ImageHandler.Tags.TryGetValue(ImageHandlerTagKeys.Thumbnail, out object thumbnail))
            {
                Preview.Image = (Image) thumbnail;
                Preview.Refresh();
            }
        }

        private void OpenInEditorButton_Click(object sender, EventArgs e)
        {
            Singleton.Get<EventManager>().Emit(new ShowInEditorEvent(_imageHandler, this));
        }
    }
}

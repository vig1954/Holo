using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;

namespace Camera
{
    public class PictureBoxController
    {
        private PictureBox _pictureBox;
        private PictureBoxToolBase _tool;
        private ImageLayoutInfo _imageLayout;

        public ImageLayoutInfo ImageLayout => _imageLayout;

        public PictureBoxController(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;

            _imageLayout = new ImageLayoutInfo(_pictureBox.ClientSize.Width, _pictureBox.ClientSize.Height);
        }

        public void SetImage(Bitmap image, bool zoom)
        {
            if (zoom)
                image = BitmapUtil.Resize(image, _pictureBox.ClientSize, out _imageLayout);
            else
                _imageLayout = new ImageLayoutInfo(_pictureBox.ClientSize.Width, _pictureBox.ClientSize.Height, image.Width, image.Height, 0, 0, 1);

            if (_tool != null)
                _tool.ImageLayoutInfo = _imageLayout;

            _pictureBox.Image = image;
        }

        public void SetTool(PictureBoxToolBase tool)
        {
            _tool?.Detach();

            _tool = tool;

            if (_tool == null)
                return;

            if (_tool.ImageLayoutInfo == null)
                _tool.ImageLayoutInfo = new ImageLayoutInfo(_pictureBox.ClientSize.Width, _pictureBox.ClientSize.Height, _pictureBox.ClientSize.Width, _pictureBox.ClientSize.Height, 0, 0, 1);

            _tool.Attach(_pictureBox);
        }
    }
}
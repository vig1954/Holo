using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Camera
{
    public interface IImageProvider
    {
        Task<Bitmap> CaptureImage();
    }
}
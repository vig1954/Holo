using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using UserInterface.DataEditors.Tools;

namespace Camera
{
    public class CameraImageProvider : IImageProvider, IDisposable
    {
        private TaskCompletionSource<Bitmap> _captureImageTaskCompletionSource;
        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();

        public event Action<Bitmap> ImageCaptured;
        public event Action LiveViewImageUpdated;

        public bool CaptureFromLiveView { get; set; }
        public Rectangle? LiveViewSelection { get; set; }
        public Rectangle? FullSizeSelection { get; set; }

        public Bitmap LiveViewImage { get; private set; }

        public CameraImageProvider()
        {
            CameraConnector.ImageDownloaded += CameraConnector_ImageDownloaded;
            CameraConnector.LiveViewUpdated += CameraConnector_LiveViewUpdated;
        }


        public Task<Bitmap> CaptureImage()
        {
            if (_captureImageTaskCompletionSource != null && !_captureImageTaskCompletionSource.Task.IsCompleted)
                throw new InvalidOperationException();

            _captureImageTaskCompletionSource = new TaskCompletionSource<Bitmap>();

            ImageCaptured += OnCaptureImageFinished;

            if (!CaptureFromLiveView)
                CameraConnector.TakePhoto();

            return _captureImageTaskCompletionSource.Task;
        }

        private void CameraConnector_ImageDownloaded(Bitmap bitmap)
        {
            ImageCaptured?.Invoke(bitmap);
        }

        private void CameraConnector_LiveViewUpdated(Bitmap bitmap)
        {
            LiveViewImage = bitmap;
            LiveViewImageUpdated?.Invoke();

            ImageCaptured?.Invoke(bitmap);
        }

        private void OnCaptureImageFinished(Bitmap bitmap)
        {
            ImageCaptured -= OnCaptureImageFinished;

            _captureImageTaskCompletionSource.SetResult(ExtractSelection(bitmap));
        }

        private Bitmap ExtractSelection(Bitmap bitmap)
        {
            if (CaptureFromLiveView)
                return LiveViewSelection.HasValue ? SelectionUtil.ExtractSelection(bitmap, LiveViewSelection.Value) : bitmap;

            return FullSizeSelection.HasValue ? SelectionUtil.ExtractSelection(bitmap, FullSizeSelection.Value) : bitmap;
        }

        public void Dispose()
        {
            CameraConnector.ImageDownloaded -= CameraConnector_ImageDownloaded;
            CameraConnector.LiveViewUpdated -= CameraConnector_LiveViewUpdated;

            LiveViewImage?.Dispose();
        }
    }
}
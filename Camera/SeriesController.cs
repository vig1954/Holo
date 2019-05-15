using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserInterface.ImageSeries;

namespace Camera
{
    public class SeriesController
    {
        private int _currentImageIndex;
        private readonly IImageSeriesProvider _seriesProvider;
        private readonly PhaseShiftDeviceController _phaseShiftDeviceController;
        private readonly IImageProvider _imageProvider;

        private ImageSeries Series => _seriesProvider.ImageSeries;

        public bool CaptureImages { get; private set; }

        public SeriesController(IImageSeriesProvider seriesProvider, IImageProvider imageProvider, PhaseShiftDeviceController phaseShiftDeviceController)
        {
            _seriesProvider = seriesProvider;
            _imageProvider = imageProvider;
            _phaseShiftDeviceController = phaseShiftDeviceController;
        }

        public void Reset()
        {
            _currentImageIndex = 0;
        }

        public async void StartCapturing()
        {
            if (CaptureImages)
                return;

            CaptureImages = true;
            _currentImageIndex = 0;

            while (CaptureImages)
            {
                await _phaseShiftDeviceController.ExecuteStepAsync(_currentImageIndex);

                var bitmap = await _imageProvider.CaptureImage();

                Series.Inputs[_currentImageIndex++].UpdateFromBitmap(bitmap);

                if (_currentImageIndex >= Series.Length)
                    _currentImageIndex = 0;
            }
        }
        
        public void StopCapturing()
        {
            CaptureImages = false;
        }
    }
}
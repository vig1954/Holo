using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Common;
using Infrastructure;
using Processing;
using Processing.DataAttributes;
using Processing.Utils;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace Camera
{
    [DataProcessor(MenuItem = "Input", Group = "ImageInput", Name = "Camera", Tooltip = "Получение изображний с фотоаппарата, с возможностью управления зеркалом на пьезокерамике")]
    public class CameraInput //: SingleImageOutputDataProcessorBase
    {
        private const string DontApplySelectionName = "Не обрезать";
        private readonly ImageHandler[] _images = new ImageHandler[4];
        private readonly OnShotParameters _onShotParameters = new OnShotParameters();

        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();
        //private PhaseShiftDeviceControllerAdapter PhaseShiftController => Singleton.Get<PhaseShiftDeviceControllerAdapter>();

        private ImageSelectionManager ImageSelectionManager => Singleton.Get<ImageSelectionManager>();

        public IBindingManager<CameraInput> BindingManager { get; set; }

        public event Action<IImageHandler> OnImageCreate;

        // TODO: добавить StringInputAttribute
        public string ImageNamePrefix { get; set; } = "camera_";

        public event Action ImageSlotsUpdated;

        [BindToUI("Камера"), ValueCollection(ValueCollectionProviderPropertyName = nameof(AvailableCameras))]
        public EDSDKLib.Camera Camera { get; set; }

        public ObservableCollection<EDSDKLib.Camera> AvailableCameras => CameraConnector?.AvailableCameras;

        [BindToUI("Av"), ValueCollection(ValueCollectionProviderPropertyName = nameof(AvailableAvModes))]
        public CameraUIntSetting AvMode
        {
            get => CameraConnector.AvMode;
            set => CameraConnector.AvMode = value;
        }

        public ObservableCollection<CameraUIntSetting> AvailableAvModes => CameraConnector?.AvailableAvModes;

        [BindToUI("Tv"), ValueCollection(ValueCollectionProviderPropertyName = nameof(AvailableTvModes))]
        public CameraUIntSetting TvMode
        {
            get => CameraConnector.TvMode;
            set => CameraConnector.TvMode = value;
        }

        public ObservableCollection<CameraUIntSetting> AvailableTvModes => CameraConnector?.AvailableTvModes;

        [BindToUI("ISO"), ValueCollection(ValueCollectionProviderPropertyName = nameof(AvailableISOModes))]
        public CameraUIntSetting ISOMode
        {
            get => CameraConnector.ISOMode;
            set => CameraConnector.ISOMode = value;
        }

        public ObservableCollection<CameraUIntSetting> AvailableISOModes => CameraConnector?.AvailableISOModes;

        public Bitmap LastPreviewImage { get; private set; }
        public event Action PreviewImageUpdated;

        [BindToUI("Захват LV")]
        public bool CaptureLiveView { get; set; }

        // TODO: добавить аттрибут для работы со списками
        // TODO: заменить всю эту байду с проперти-аттрибутами на что-то более гибкое, возможно тип проперти PropertyBinder<T>
        [BindToUI("Слот 1", "Default"), ImageHandlerFilter(OnlyImages = true)]
        public IImageHandler ImageSlot1
        {
            get => _images[0];
            set => _images[0] = (ImageHandler) value;
        }

        [BindToUI("Слот 2", "Default"), ImageHandlerFilter(OnlyImages = true)]
        public IImageHandler ImageSlot2
        {
            get => _images[1];
            set => _images[1] = (ImageHandler) value;
        }

        [BindToUI("Слот 3", "Default"), ImageHandlerFilter(OnlyImages = true)]
        public IImageHandler ImageSlot3
        {
            get => _images[2];
            set => _images[2] = (ImageHandler) value;
        }

        [BindToUI("Слот 4", "Default"), ImageHandlerFilter(OnlyImages = true)]
        public IImageHandler ImageSlot4
        {
            get => _images[3];
            set => _images[3] = (ImageHandler) value;
        }

        [BindToUI("Обрезать по выделению"), ValueCollection(ValueCollectionProviderPropertyName = nameof(AvailableSelectionsToApply))]
        public ImageSelection SelectionToApply { get; set; }

        public ObservableCollection<ImageSelection> AvailableSelectionsToApply { get; } = new ObservableCollection<ImageSelection>();

        public CameraInput()
        {
            // TODO: далее идут ужасные костыли. плохо, очень плохо
            CameraConnector.ActiveCameraSelected += camera =>
            {
                BindingManager.SetPropertyValue(c => c.Camera, camera);
                BindingManager.SetPropertyValue(c => c.AvMode, CameraConnector.AvMode);
                BindingManager.SetPropertyValue(c => c.TvMode, CameraConnector.TvMode);
                BindingManager.SetPropertyValue(c => c.ISOMode, CameraConnector.ISOMode);
            };

            CameraConnector.LiveViewUpdated += CameraConnectorOnLiveViewUpdated;
            CameraConnector.ImageDownloaded += CameraConnectorOnImageDownloaded;

            UpdateAvailableSelectionList();

            //Output = ImageHandler.Create("preview", 960, 640, ImageFormat.RGB, ImagePixelFormat.Byte);
        }

        [OnBindedPropertyChanged(nameof(Camera))]
        public void CameraOnValueSelected(ValueUpdatedEventArgs e)
        {
            if (e.Sender != this)
                CameraConnector.SetActiveCamera(Camera);
        }

        [BindToUI("Серия снимков")]
        public void MakeSeries()
        {
            _onShotParameters.Reset();
            _onShotParameters.TakeSeries = true;
            //PhaseShiftController.SetShift(ShiftStep, _onShotParameters.CurrentImageIndex, ShiftDelay);

            CameraConnector.TakePhoto();
        }

        [BindToUI("Тестовый снимок")]
        public void MakeTestShot()
        {
            _onShotParameters.Reset();
            _onShotParameters.TakeSeries = false;
            CameraConnector.TakePhoto();
        }

        public void Awake()
        {
            UpdateAvailableSelectionList();
//            PhaseShiftController.UpdatePortNames();
//            Output?.Update();
        }

        public void FreeResources()
        {
        }

        public void Dispose()
        {
            CameraConnector.Dispose();
           // base.Dispose();
        }

        [OnBindedPropertiesChanged(nameof(ImageSlot1), nameof(ImageSlot2), nameof(ImageSlot3), nameof(ImageSlot4))]
        public void OnImageSlotUpdated(ValueUpdatedEventArgs e)
        {
            ImageSlotsUpdated?.Invoke();
        }

        private void CameraConnectorOnLiveViewUpdated(Bitmap bitmap)
        {
            using (new DebugLogger.MinimalImportanceScope(DebugLogger.ImportanceLevel.Warning))
            {
//                if (Output == null)
//                    Output = ImageHandler.FromBitmap(bitmap);
//                else
//                    Output.UpdateFromBitmap(bitmap);
//
//                Output.UploadToComputingDevice(true);
//                Output.Update();
                LastPreviewImage = bitmap;
                PreviewImageUpdated?.Invoke();

                if (CaptureLiveView)
                    ProcessBitmap(bitmap);
            }
        }

        private void UpdateAvailableSelectionList()
        {
            // TODO: это ужасно, нужен другой способ указать что значение не выбрано 
            AvailableSelectionsToApply.Clear();
            AvailableSelectionsToApply.AddRange(new ImageSelection[] { new ImageSelection { Name = DontApplySelectionName } }.Concat(ImageSelectionManager.GetAllSelections()));

            ImageSelectionManager.OnSelectionAdded += selection => AvailableSelectionsToApply.Add(selection);
        }

        private void CameraConnectorOnImageDownloaded(Bitmap bitmap)
        {
            ProcessBitmap(bitmap);
        }

        private void ProcessBitmap(Bitmap bitmap)
        {
            if (SelectionToApply != null && SelectionToApply.Name != DontApplySelectionName)
                bitmap = ImageUtils.ExtractSelection(bitmap, SelectionToApply);

            var currentImageIndex = _onShotParameters.CurrentImageIndex;
            if (_images[currentImageIndex] == null)
            {
                var newImage = ImageHandler.FromBitmapAsGreyscale(bitmap);
                _images[currentImageIndex] = newImage;

                BindingManager.SetPropertyValue(nameof(ImageSlot1).Replace("1", (currentImageIndex + 1).ToString()), (IImageHandler) newImage);

                OnImageCreate?.Invoke(newImage);
            }
            else
                _images[currentImageIndex].UpdateFromBitmap(bitmap);

            ImageSlotsUpdated?.Invoke();

            _onShotParameters.Update();
            if (CaptureLiveView && _onShotParameters.SeriesComplete)
            {
                _onShotParameters.Reset();
                return;
            }

            if (_onShotParameters.TakeSeries && !_onShotParameters.SeriesComplete)
            {
                //  PhaseShiftController.SetShift(ShiftStep, _onShotParameters.CurrentImageIndex, ShiftDelay);
                CameraConnector.TakePhoto();
            }
        }

        private class OnShotParameters
        {
            public bool TakeSeries { get; set; }
            public int CurrentImageIndex { get; set; }
            public int ImagesCount { get; set; }
            public bool SeriesComplete => CurrentImageIndex >= ImagesCount;

            public void Reset(int imagesCount = 4)
            {
                ImagesCount = imagesCount;
                CurrentImageIndex = 0;
            }

            public void Update()
            {
                if (!TakeSeries)
                    return;

                CurrentImageIndex++;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private int[] _shiftValues = new int[4];

        private bool ignoreNewImages = false;

        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();

        private ImageSelectionManager ImageSelectionManager => Singleton.Get<ImageSelectionManager>();

        public IBindingManager<CameraInput> BindingManager { get; set; }

        public event Action<IImageHandler> ImageCreate;

        // TODO: review usage
        public event Action SeriesStarted;
        public event Action SeriesComplete;

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

        [BindToUI("Серия снимков")]
        public async void MakeSeries()
        {
            BindingManager.SetPropertyValue(c => c.CaptureLiveView, false);

            _onShotParameters.Reset();
            _onShotParameters.TakeSeries = true;
            await PhaseShiftController.SetShift(GetShiftValue(_onShotParameters.CurrentImageIndex), ShiftDelay);

            CameraConnector.TakePhoto();
        }

        [BindToUI("Тестовый снимок")]
        public void MakeTestShot()
        {
            BindingManager.SetPropertyValue(c => c.CaptureLiveView, false);

            _onShotParameters.Reset();
            _onShotParameters.TakeSeries = false;
            CameraConnector.TakePhoto();
        }

        [BindToUI("Тестовый снимок с LV")]
        public void MakeTestLiveViewShot()
        {
            BindingManager.SetPropertyValue(c => c.CaptureLiveView, false);

            _onShotParameters.Reset();
            _onShotParameters.TakeSeries = false;

            if (LastPreviewImage == null)
                return;

            ProcessBitmap(LastPreviewImage);
        }

        [BindToUI, BindMembersToUI(HideProperty = true, MergeMembers = true)]
        public PhaseShiftDeviceControllerAdapter PhaseShiftController => Singleton.Get<PhaseShiftDeviceControllerAdapter>();

        [BindToUI]
        public int ShiftStep { get; set; } = 400;
        
        [BindToUI]
        public bool UseShiftValues { get; set; }
        [BindToUI]
        public int ShiftValue1 { get => _shiftValues[0]; set => _shiftValues[0] = value; }
        [BindToUI]
        public int ShiftValue2 { get => _shiftValues[1]; set => _shiftValues[1] = value; }
        [BindToUI]
        public int ShiftValue3 { get => _shiftValues[2]; set => _shiftValues[2] = value; }
        [BindToUI]
        public int ShiftValue4 { get => _shiftValues[3]; set => _shiftValues[3] = value; }

        [BindToUI("Время установления сдвига, мс")]
        public int ShiftDelay { get; set; } = 100;


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

        public void Awake()
        {
            UpdateAvailableSelectionList();
            PhaseShiftController.UpdatePortNames();
        }

        public void FreeResources()
        {
        }

        public void Dispose()
        {
            CameraConnector.Dispose();
        }

        [OnBindedPropertiesChanged(nameof(ImageSlot1), nameof(ImageSlot2), nameof(ImageSlot3), nameof(ImageSlot4))]
        public void OnImageSlotUpdated(ValueUpdatedEventArgs e)
        {
            ImageSlotsUpdated?.Invoke();
        }

        [OnBindedPropertyChanged(nameof(CaptureLiveView))]
        public void OnCaptureLiveViewUpdated(ValueUpdatedEventArgs e)
        {
            _onShotParameters.Reset();
            ignoreNewImages = false;

            _onShotParameters.TakeSeries = CaptureLiveView;
        }

        private void CameraConnectorOnLiveViewUpdated(Bitmap bitmap)
        {
            using (new DebugLogger.MinimalImportanceScope(DebugLogger.ImportanceLevel.Warning))
            {
                LastPreviewImage = bitmap;
                PreviewImageUpdated?.Invoke();

                if (ignoreNewImages)
                    return;

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

        private async void ProcessBitmap(Bitmap bitmap)
        {
            ignoreNewImages = true;

            if (SelectionToApply != null && SelectionToApply.Name != DontApplySelectionName)
                bitmap = ImageUtils.ExtractSelection(bitmap, SelectionToApply);

            var currentImageIndex = _onShotParameters.CurrentImageIndex;

            if (currentImageIndex == 0 && _onShotParameters.TakeSeries)
                SeriesStarted?.Invoke();

            if (_images[currentImageIndex] == null)
            {
                var newImage = ImageHandler.FromBitmapAsGreyscale(bitmap);
                _images[currentImageIndex] = newImage;

                BindingManager.SetPropertyValue(nameof(ImageSlot1).Replace("1", (currentImageIndex + 1).ToString()), (IImageHandler) newImage);

                ImageCreate?.Invoke(newImage);
            }
            else
                _images[currentImageIndex].UpdateFromBitmap(bitmap);

            ImageSlotsUpdated?.Invoke();

            _onShotParameters.Update();
            if (_onShotParameters.SeriesComplete || !_onShotParameters.TakeSeries)
            {
                SeriesComplete?.Invoke();

                if (CaptureLiveView)
                    _onShotParameters.Reset();
            }
            
            if (PhaseShiftController.Connected && !_onShotParameters.SeriesComplete)
                await PhaseShiftController.SetShift(GetShiftValue(_onShotParameters.CurrentImageIndex), ShiftDelay);

            ignoreNewImages = false;

            if (!CaptureLiveView && _onShotParameters.TakeSeries && !_onShotParameters.SeriesComplete)
                CameraConnector.TakePhoto();
        }

        private int GetShiftValue(int imageIndex)
        {
            return UseShiftValues ? _shiftValues[imageIndex] : ShiftStep * imageIndex;
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

                if (CurrentImageIndex < ImagesCount)
                    CurrentImageIndex++;
            }
        }
    }
}
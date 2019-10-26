using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure;
using Processing;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.ImageSeries;

namespace Camera
{
    public class CameraInputView : IImageSeriesProvider
    {
        public const int SeriesLength = 4;

        private readonly ImageHandler[] _imageHandlers = new ImageHandler[SeriesLength];
        private readonly Bitmap[] _images = new Bitmap[SeriesLength];
        private readonly SeriesController _seriesController;
        private readonly CameraImageProvider CameraImageProvider = new CameraImageProvider();


        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();

        private PhaseShiftDeviceView PhaseShiftDeviceSettingsController { get; }

        private LowLevelPhaseShiftDeviceControllerAdapter LowLevelPhaseShiftDeviceController => Singleton.Get<LowLevelPhaseShiftDeviceControllerAdapter>();
        private ImageSeriesRepository SeriesRepository => Singleton.Get<ImageSeriesRepository>();
        private CameraSettings CameraSettings { get; } = new CameraSettings();

        public IBindingManager<CameraInputView> BindingManager { get; set; }

        public Bitmap LiveViewImage => CameraImageProvider.LiveViewImage;
        public event Action LiveViewImageUpdated;
        public event Action CaptureStarted;
        public event Action CaptureFinished;

        public Rectangle? LiveViewSelection
        {
            get => CameraImageProvider.LiveViewSelection;
            set => CameraImageProvider.LiveViewSelection = value;
        }

        [BindToUI("Камера", "Камера", UiLabelMode.None), ValueCollection(ValueCollectionProviderPropertyName = nameof(AvailableCameras), AllowDefaultValue = true, DefaultValueDisplayText = "[Камера не выбрана]")]
        public EDSDKLib.Camera Camera
        {
            get => CameraConnector.ActiveCamera;
            set => CameraConnector.SetActiveCamera(value);
        }

        public ObservableCollection<EDSDKLib.Camera> AvailableCameras => CameraConnector.AvailableCameras;

        [BindToUI("Настройки камеры", "Камера")]
        public void OpenCameraSettingsFormUiAction() => OpenCameraSettingsForm();

        [BindToUI("Серия", "Захват"), ValueCollection(ValueCollectionProviderPropertyName = nameof(AvailableImageSeries), AllowDefaultValue = true, DefaultValueDisplayText = "[Новая серия]")]
        public ImageSeries ImageSeries { get; set; }

        public IRepository<ImageSeries> AvailableImageSeries => SeriesRepository;

        [BindToUI("Начать захват", "Захват")]
        public void ToggleCaptureUiAction() => ToggleCapture();

        [BindToUI("Захват с LV", "Захват")] public bool CaptureFromLiveView { get; set; } = true;

        [BindToUI("Тестовый снимок", "Захват - тест")]
        public void TestShotUiAction() => TestShot();

        [BindToUI("Сохранить в", "Захват - тест")]
        public IImageHandler TestShotDestination { get; set; }

        [BindToUI("Порт", "Пьезокерамика"), ValueCollection(ValueCollectionProviderPropertyName = nameof(PortNames), AllowDefaultValue = true, DefaultValueDisplayText = "[Порт не выбран]")]
        public string PhaseShiftDevicePort { get; set; }

        public ObservableCollection<string> PortNames { get; }

        [BindToUI("Подключить", "Пьезокерамика")]
        public void TogglePhaseShiftDeviceUiAction() => TogglePhaseShiftDevice();

        public CameraInputView(PhaseShiftDeviceView phaseShiftDeviceController)
        {
            PhaseShiftDeviceSettingsController = phaseShiftDeviceController;
            _seriesController = new SeriesController(this, CameraImageProvider, PhaseShiftDeviceSettingsController);

            CameraConnector.ActiveCameraSelected += camera =>
            {
                BindingManager?.SetPropertyValue(c => c.Camera, camera);
                CameraSettings.Load();
            };

            CameraImageProvider.LiveViewImageUpdated += () => LiveViewImageUpdated?.Invoke();

            PortNames = new ObservableCollection<string>(SerialPort.GetPortNames());
        }

        public void Shutdown()
        {
            _seriesController.StopCapturing();
        }

        public async void StartCapture()
        {
            if (_seriesController.CaptureImages)
                return;

            BindingManager.SetMemberControlEnabledState(ci => ci.Camera, false, this);
            BindingManager.SetMemberControlEnabledState(ci => ci.ImageSeries, false, this);
            BindingManager.SetMemberControlEnabledState(ci => ci.CaptureFromLiveView, false, this);
            BindingManager.SetMethodControlEnabledState(ci => ci.TestShotUiAction(), false, this);
            BindingManager.ModifyMethodControl(c => c.ToggleCaptureUiAction(), c => c.Text = "Остановить захват",
                this);


            if (ImageSeries == null)
            {
                var testImage = await CameraImageProvider.CaptureImage(); // todo: store image size for cameras

                var imageSeries = new ImageSeries(testImage.Size, $"Серия {SeriesRepository.Count + 1}");
                AvailableImageSeries.Add(imageSeries);

                BindingManager.SetPropertyValue(c => c.ImageSeries, imageSeries);
            }

            _seriesController.StartCapturing();
            CaptureStarted?.Invoke();
        }

        public void StopCapture()
        {
            if (!_seriesController.CaptureImages)
                return;

            _seriesController.StopCapturing();

            BindingManager.SetMemberControlEnabledState(ci => ci.Camera, true, this);
            BindingManager.SetMemberControlEnabledState(ci => ci.ImageSeries, true, this);
            BindingManager.SetMemberControlEnabledState(ci => ci.CaptureFromLiveView, true, this);
            BindingManager.SetMethodControlEnabledState(ci => ci.TestShotUiAction(), true, this);
            BindingManager.ModifyMethodControl(c => c.ToggleCaptureUiAction(), c => c.Text = "Начать захват", this);


            CaptureFinished?.Invoke();
        }


        private void OpenCameraSettingsForm()
        {
            if (Application.OpenForms.OfType<CameraSettingsForm>().Any() || CameraConnector.ActiveCamera == null)
                return;

            var cameraSettingsForm = new CameraSettingsForm(CameraSettings);
            cameraSettingsForm.Show();
        }

        private void ToggleCapture()
        {
            if (!_seriesController.CaptureImages)
            {
                StartCapture();
            }
            else
            {
                StopCapture();
            }
        }

        private void TestShot()
        {
        }

        private void TogglePhaseShiftDevice()
        {
            if (LowLevelPhaseShiftDeviceController.Connected)
            {
                LowLevelPhaseShiftDeviceController.Disconnect();
                BindingManager.ModifyMethodControl(c => c.TogglePhaseShiftDeviceUiAction(), c => c.Text = "Подключить", this);
            }
            else
            {
                LowLevelPhaseShiftDeviceController.Connect(PhaseShiftDevicePort);
                BindingManager.ModifyMethodControl(c => c.TogglePhaseShiftDeviceUiAction(), c => c.Text = "Отключить", this);
            }
        }
    }
}
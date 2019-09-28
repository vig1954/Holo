using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using EDSDKLib;
using Timer = Common.Timer;

namespace Camera
{
    public class CameraConnector : IDisposable
    {
        private bool _sessionOpened;
        private SDKHandler _sdkHandler;
        private EDSDKLib.Camera _activeCamera;
        private SynchronizationContext _mainThreadContext;

        public event Action<EDSDKLib.Camera> ActiveCameraSelected;
        public event Action<Bitmap> LiveViewUpdated;
        public event Action<Bitmap> ImageDownloaded;
        public event Action<IEnumerable<EDSDKLib.Camera>> AvailableCamerasUpdated;
        public event Action SessionOpened;
        public event Action SessionClosed;

        public EDSDKLib.Camera ActiveCamera => _activeCamera;
        public ObservableCollection<CameraUIntSetting> AvailableAvModes = new ObservableCollection<CameraUIntSetting>();
        public ObservableCollection<CameraUIntSetting> AvailableTvModes = new ObservableCollection<CameraUIntSetting>();
        public ObservableCollection<CameraUIntSetting> AvailableISOModes = new ObservableCollection<CameraUIntSetting>();
        public ObservableCollection<EDSDKLib.Camera> AvailableCameras = new ObservableCollection<EDSDKLib.Camera>();

        public CameraUIntSetting AvMode
        {
            get
            {
                try
                {
                    return AvailableAvModes.SingleOrDefault(av => av.Value == _sdkHandler.GetSetting(_sdkHandler.GetSetting(EDSDK.PropID_Av)));
                }
                catch (Exception ex)
                {
                    DebugLogger.Warning(ex);
                    return AvailableAvModes.First();
                }
            }
            set => SetSdkSettingIfNotNull(EDSDK.PropID_Av, value);
        }

        public CameraUIntSetting TvMode
        {
            get
            {
                try
                {
                    return AvailableTvModes.SingleOrDefault(tv => tv.Value == _sdkHandler.GetSetting(_sdkHandler.GetSetting(EDSDK.PropID_Tv)));
                }
                catch (Exception ex)
                {
                    DebugLogger.Warning(ex);
                    return AvailableTvModes.First();
                }
            }
            set => SetSdkSettingIfNotNull(EDSDK.PropID_Tv, value);
        }

        public CameraUIntSetting ISOMode
        {
            get
            {
                try
                {
                    return AvailableISOModes.SingleOrDefault(iso => iso.Value == _sdkHandler.GetSetting(_sdkHandler.GetSetting(EDSDK.PropID_ISOSpeed)));
                }
                catch (Exception ex)
                {
                    DebugLogger.Warning(ex);
                    return AvailableISOModes.First();
                }
            }
            set => SetSdkSettingIfNotNull(EDSDK.PropID_ISOSpeed, value);
        }

        public CameraConnector()
        {
//            try
//            {
                _mainThreadContext = SynchronizationContext.Current;

                _sdkHandler = new SDKHandler();
                _sdkHandler.CameraAdded += CameraAdded;
                _sdkHandler.CameraHasShutdown += CameraShutdown;
                _sdkHandler.ImageDownloaded += CameraImageDownloaded;
                _sdkHandler.LiveViewUpdated += CameraLiveViewUpdated;

                RefreshAvailableCameras();
//            }
//            catch (Exception ex)
//            {
//                DebugLogger.Warning("Camera connector initialization exception!");
//                DebugLogger.Warning(ex);
//            }
        }

        public void TakePhoto()
        {
            _sdkHandler.TakePhoto();
        }

        public bool SetActiveCamera(EDSDKLib.Camera camera)
        {
            if (_activeCamera == camera)
                return false;

            if (_sessionOpened)
            {
                CloseSession();
                camera = AvailableCameras.FirstOrDefault(c => c.Info.szDeviceDescription == camera?.Info.szDeviceDescription);

                if (camera == null)
                    DebugLogger.Log("CameraConnector.SetActiveCamera: camera lost after closing session");
            }

            if (camera == null)
                return false;

            OpenSession(camera);

            ActiveCameraSelected?.Invoke(camera);
            return true;
        }

        private void OpenSession(EDSDKLib.Camera camera)
        {
            _sdkHandler.OpenSession(camera);

            if (_sdkHandler.GetSetting(EDSDK.PropID_AEMode) != EDSDK.AEMode_Manual)
                DebugLogger.Warning("Camera is not in manual mode. Some features might not work!");

            AvailableAvModes.AddRange(_sdkHandler.GetSettingsList(EDSDK.PropID_Av)
                .Select(s => new CameraUIntSetting
                {
                    Value = (uint) s,
                    Title = CameraValues.AV((uint) s)
                }));

            AvailableTvModes.AddRange(_sdkHandler.GetSettingsList(EDSDK.PropID_Tv)
                .Select(s => new CameraUIntSetting
                {
                    Value = (uint) s,
                    Title = CameraValues.TV((uint) s)
                }));

            AvailableISOModes.AddRange(_sdkHandler.GetSettingsList(EDSDK.PropID_ISOSpeed)
                .Select(s => new CameraUIntSetting
                {
                    Value = (uint) s,
                    Title = CameraValues.ISO((uint) s)
                }));

            _activeCamera = camera;

            _sdkHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);
            _sdkHandler.StartLiveView();
            _sdkHandler.SetCapacity();

            _sessionOpened = true;
        }

        private void CloseSession()
        {
            _sdkHandler.CloseSession();
            AvailableAvModes.Clear();
            AvailableISOModes.Clear();
            AvailableTvModes.Clear();

            _sessionOpened = false;
            //RefreshAvailableCameras();
        }

        private void RefreshAvailableCameras()
        {
            AvailableCameras.Clear();
            foreach (var camera in _sdkHandler.GetCameraList())
            {
                AvailableCameras.Add(camera);
            }

            AvailableCamerasUpdated?.Invoke(AvailableCameras);
        }

        private void CameraImageDownloaded(Bitmap bmp)
        {
            if (_mainThreadContext != SynchronizationContext.Current)
                _mainThreadContext.Post(state => ImageDownloaded?.Invoke((Bitmap) state), bmp);
            else
                ImageDownloaded?.Invoke(bmp);
        }

        private void CameraShutdown(object sender, EventArgs eventArgs)
        {
            RefreshAvailableCameras();
            DebugLogger.Log("Камера была выключена.");
        }

        private void CameraAdded()
        {
            RefreshAvailableCameras();
        }

        private void CameraLiveViewUpdated(Stream imageStream)
        {
            using (new DebugLogger.MinimalImportanceScope(DebugLogger.ImportanceLevel.Warning))
            {
                using (new Timer("Live View Updated"))
                {
                    try
                    {
                        var image = new Bitmap(imageStream);

                        if (_mainThreadContext != SynchronizationContext.Current)
                            _mainThreadContext.Post(state => LiveViewUpdated?.Invoke((Bitmap) state), image);
                        else
                            LiveViewUpdated?.Invoke(image);
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.Log(ex);
                        throw;
                    }
                }
            }
        }

        private void SetSdkSettingIfNotNull(uint settingId, CameraUIntSetting value)
        {
            if (value != null)
                _sdkHandler.SetSetting(settingId, value.Value);
        }

        public void Dispose()
        {
            if (_sessionOpened)
            {
                _sdkHandler.StopLiveView();
                CloseSession();
            }
            _sdkHandler?.Dispose();
        }
    }

    public class CameraUIntSetting
    {
        public uint Value { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
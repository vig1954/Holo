using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Infrastructure;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace Camera
{
    public class CameraSettings
    {
        private CameraConnector CameraConnector => Singleton.Get<CameraConnector>();
        private SessionValues SessionValues => Singleton.Get<SessionValues>();

        public IBindingManager<CameraSettings> BindingManager { get; set; }

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

        [BindToUI("ISO"), ValueCollection(ValueCollectionProviderPropertyName = nameof(AvailableIsoModes))]
        public CameraUIntSetting IsoMode
        {
            get => CameraConnector.ISOMode;
            set => CameraConnector.ISOMode = value;
        }

        public ObservableCollection<CameraUIntSetting> AvailableIsoModes => CameraConnector?.AvailableISOModes;

        public void Save() => Save(GetCameraKey());

        public void Save(string cameraKey)
        {
            SessionValues.Set(BuildPropertyKey(cameraKey, nameof(AvMode)), AvMode.Value, this);
            SessionValues.Set(BuildPropertyKey(cameraKey, nameof(TvMode)), TvMode.Value, this);
            SessionValues.Set(BuildPropertyKey(cameraKey, nameof(IsoMode)), IsoMode.Value, this);
        }

        public void Load() => Load(GetCameraKey());

        public void Load(string cameraKey)
        {
            if (TryLoadValue(cameraKey, nameof(AvMode), AvailableAvModes, out var avModeValue))
            {
                if (BindingManager != null)
                    BindingManager.SetPropertyValue(s => s.AvMode, avModeValue);
                else
                    AvMode = avModeValue;
            }

            if (TryLoadValue(cameraKey, nameof(TvMode), AvailableTvModes, out var tvModeValue))
            {
                if (BindingManager != null)
                    BindingManager.SetPropertyValue(s => s.TvMode, tvModeValue);
                else
                    TvMode = tvModeValue;
            }

            if (TryLoadValue(cameraKey, nameof(IsoMode), AvailableIsoModes, out var isoModeValue))
            {
                if (BindingManager != null)
                    BindingManager.SetPropertyValue(s => s.IsoMode, isoModeValue);
                else
                    IsoMode = isoModeValue;
            }
        }

        public void SyncCameraSettings()
        {
            if (BindingManager == null)
                return;

            BindingManager.SyncPropertyValue(s => s.AvMode);
            BindingManager.SyncPropertyValue(s => s.TvMode);
            BindingManager.SyncPropertyValue(s => s.IsoMode);
        }

        private bool TryLoadValue(string cameraKey, string propertyName, ObservableCollection<CameraUIntSetting> valueCollection, out CameraUIntSetting value)
        {
            value = null;
            var valueKey = BuildPropertyKey(cameraKey, propertyName);
            if (SessionValues.Has(valueKey))
            {
                var uintValue = SessionValues.Get<uint>(valueKey);
                value = valueCollection.FirstOrDefault(v => v.Value == uintValue);

                return value != null;
            }

            return false;
        }

        private string BuildPropertyKey(string cameraKey, string propName)
        {
            return $"{GetType().FullName}_{cameraKey}_{propName}";
        }

        private string GetCameraKey() => CameraConnector.ActiveCamera.Info.szDeviceDescription;
    }
}
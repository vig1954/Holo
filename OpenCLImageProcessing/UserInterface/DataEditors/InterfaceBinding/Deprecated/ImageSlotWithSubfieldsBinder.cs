﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Processing;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding.Deprecated
{
    public class ImageSlotWithSubfieldsBinder : PropertyBindingBase
    {
        private readonly ImageSlotWithSubfieldsControl _imageSlot;
        private MethodInfo _onImageUpdated;
        private IImageHandler _imageSlotValue;

        public override IBindableControl Control => _imageSlot;

        public ImageSlotWithSubfieldsBinder(ImageSlotWithSubfieldsAttribute imageSlotAttribute, MemberInfo memberInfo,
            object target) : base(imageSlotAttribute, memberInfo, target)
        {
            DisplayGroup = imageSlotAttribute.Group;

            if (imageSlotAttribute.OnImageUpdated != null)
                _onImageUpdated = target.GetType().GetMethod(imageSlotAttribute.OnImageUpdated);

            _imageSlot = new ImageSlotWithSubfieldsControl
            {
                Title = imageSlotAttribute.TooltipText,
                AllowedImageFormats = imageSlotAttribute.AllowedImageFormats.ToList(),
                AllowedPixelFormats = imageSlotAttribute.AllowedPixelFormat.HasValue
                    ? new List<ImagePixelFormat>(new[] { imageSlotAttribute.AllowedPixelFormat.Value })
                    : new List<ImagePixelFormat>(),
                RequiredChannelCount = imageSlotAttribute.RequiredChannelCount
            };
            _imageSlot.SetValue(_propertyInfo.GetValue(Target), this);

            _imageSlot.ValueUpdated += e =>
            {
                if (_imageSlotValue != _imageSlot.Value && _imageSlotValue != null)
                    _imageSlotValue.ImageUpdated -= ImageUpdated;

                _imageSlotValue = (IImageHandler)_imageSlot.Value;

                if (_imageSlotValue != null)
                    _imageSlotValue.ImageUpdated += ImageUpdated;

                _propertyInfo.SetValue(Target, _imageSlot.Value);
                OnPropertyChanged();
            };
        }

        public override void Set(object value)
        {
            _imageSlot.SetValue(value, this);
        }

        public void ImageUpdated(ImageUpdatedEventData e)
        {
            _onImageUpdated?.Invoke(Target, new object[] { });
        }
    }
}

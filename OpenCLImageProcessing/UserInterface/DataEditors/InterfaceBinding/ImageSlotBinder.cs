using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Processing;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class ImageSlotBinder : PropertyBindingBase
    {
        private readonly ImageSlotControl _imageSlot;

        public override Control Control => _imageSlot;

        public ImageSlotBinder(ImageSlotAttribute imageSlotAttribute, MemberInfo memberInfo,
            object target) : base(imageSlotAttribute, memberInfo, target)
        {
            Group = imageSlotAttribute.Group;

            _imageSlot = new ImageSlotControl
            {
                Title = imageSlotAttribute.TooltipText,
                Value = (IImageHandler) _propertyInfo.GetValue(Target),
                AllowedImageFormats = imageSlotAttribute.AllowedImageFormats.ToList(),
                AllowedPixelFormats = imageSlotAttribute.AllowedPixelFormat.HasValue
                    ? new List<ImagePixelFormat>(new[] {imageSlotAttribute.AllowedPixelFormat.Value})
                    : new List<ImagePixelFormat>(),
                RequiredChannelCount = imageSlotAttribute.RequiredChannelCount
            };

            _imageSlot.OnValueChanged += () =>
            {
                _propertyInfo.SetValue(Target, _imageSlot.Value);
                OnPropertyChanged();
            };
        }

        public override void Set(object value)
        {
            _imageSlot.Value = (IImageHandler) value;
        }

        protected override void OnPropertyChangedByTarget()
        {
            _imageSlot.Value = (IImageHandler) _propertyInfo.GetValue(Target);
        }
    }
}
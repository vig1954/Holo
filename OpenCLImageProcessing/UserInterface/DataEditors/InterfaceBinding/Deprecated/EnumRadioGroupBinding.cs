using System;
using System.Reflection;
using System.Windows.Forms;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding.Deprecated
{
    public class EnumRadioGroupBinding : PropertyBindingBase
    {
        private readonly EnumRadioGroupControl _enumRadioGroupControl;
        public override IBindableControl Control => _enumRadioGroupControl;

        public EnumRadioGroupBinding(EnumRadioGroupAttribute enumRadioGroupAttribute, MemberInfo memberInfo, object target) : base(enumRadioGroupAttribute, memberInfo, target)
        {
            DisplayGroup = enumRadioGroupAttribute.Group;

            _enumRadioGroupControl = new EnumRadioGroupControl(_propertyInfo.PropertyType)
            {
                Title = enumRadioGroupAttribute.TooltipText
            };
            _enumRadioGroupControl.SetValue(_propertyInfo.GetValue(target), this);

            _enumRadioGroupControl.ValueUpdated += e =>
            {
                _propertyInfo.SetValue(Target, _enumRadioGroupControl.Value);
                OnPropertyChanged();
            };
        }
    }
}
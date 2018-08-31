using System.Reflection;
using System.Windows.Forms;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding.Deprecated
{
    public class CheckboxBinding : PropertyBindingBase
    {
        private readonly CheckboxControl _control;
        public override IBindableControl Control => _control;
        public CheckboxBinding(CheckboxAttribute checkboxAttribute, MemberInfo memberInfo, object target) : base(checkboxAttribute, memberInfo, target)
        {
            DisplayGroup = checkboxAttribute.Group;

            _control = new CheckboxControl
            {
                Checked = (bool) _propertyInfo.GetValue(Target),
                Text = checkboxAttribute.TooltipText
            };

            _control.CheckedChanged += (sender, args) =>
            {
                _propertyInfo.SetValue(Target, _control.Checked);
                OnPropertyChanged();
            };
        }
    }
}

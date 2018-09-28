using System.Reflection;
using System.Windows.Forms;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding.Deprecated
{
    public class NumberBinding : PropertyBindingBase
    {
        private readonly NumberControl _control;
        public override Control Control => _control;

        public NumberBinding(NumberAttribute numberAttribute, MemberInfo memberInfo, object target) : base(numberAttribute, memberInfo, target)
        {
            DisplayGroup = numberAttribute.Group;

            _control = new NumberControl
            {
                Text = numberAttribute.TooltipText,
                Title = numberAttribute.TooltipText,
                Minimum = numberAttribute.Min,
                Maximum = numberAttribute.Max,
                Step = numberAttribute.Step
            };
            _control.SetValue(_propertyInfo.GetValue(Target), this);

            _control.ValueUpdated += e =>
            {
                _propertyInfo.SetValue(Target, _control.Value);
                OnPropertyChanged();
            };
        }
    }
}
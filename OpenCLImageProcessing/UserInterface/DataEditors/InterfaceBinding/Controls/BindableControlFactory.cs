using System;
using System.Linq;
using Processing;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public interface IBindableControlFactory
    {
        IBindableControl Get(IBinding binding);
    }

    public class BindableControlFactory: IBindableControlFactory
    {
        public IBindableControl Get(IBinding binding)
        {
            if (binding is IBindableControlProvider bindableControlProvider)
                return bindableControlProvider.GetControl();

            IBindableControl control = null;
            if (binding is IValueBinding valueBinding)
            {
                var bindMembersToUiAttribute = valueBinding.GetAttribute<BindMembersToUIAttribute>();
                if (bindMembersToUiAttribute != null && bindMembersToUiAttribute.HideProperty)
                    control = new LabelControl();
                else if (valueBinding.ValueType.IsEnum)
                    control = new EnumRadioGroupControl();
                else if (new [] {typeof(int), typeof(float), typeof(double)}.Contains(valueBinding.ValueType))
                    control = new NumberControl();
                else if (valueBinding.ValueType == typeof(bool))
                    control = new CheckboxControl();
                else if (typeof(IImageHandler).IsAssignableFrom(valueBinding.ValueType))
                    control = new ImageHandlerControl();
                else
                    control = new LabelControl();
            }
            else if (binding is MethodBinding methodBinding)
            {
                control = new ButtonControl();
            }

            if (control == null)
                throw new NotImplementedException();

            control.SetBinding(binding);
            return control;
        }
    }
}

 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 using Common;
 using Processing;
 using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
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
                if (valueBinding.ValueType.IsEnum)
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

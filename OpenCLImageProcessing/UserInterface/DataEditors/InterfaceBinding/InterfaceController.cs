using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserInterface.DataEditors.InterfaceBinding.ControlsV2;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IInterfaceController
    {
        void BindObjectToInterface(object target);
    }

    public class InterfaceController: IInterfaceController
    {
        private Control _container;
        public IBindingProviderFactory BindingProviderFactory { get; set; }
        public IBindableControlFactory BindableControlFactory { get; set; }

        public InterfaceController(Control container)
        {
            BindingProviderFactory = new BindingProviderFactory();
            BindableControlFactory = new BindableControlFactory();
            _container = container;
        }

        public void BindObjectToInterface(object target)
        {
            var bindingProvider = BindingProviderFactory.Get(target);

            var bindings = bindingProvider.GetBindings();

            // TODO: можно оптимизировать процесс так, чтобы контролы переиспользовались
            _container.Controls.Clear();
            foreach (var binding in bindings)
            {
                var bindableControl = BindableControlFactory.Get(binding);
                if (bindableControl is Control control)
                    _container.Controls.Add(control);
                else
                    throw new InvalidOperationException();
            }
        }
    }
}

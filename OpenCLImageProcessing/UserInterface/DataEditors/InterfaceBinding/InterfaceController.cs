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

    public class InterfaceController : IInterfaceController
    {
        private Control _container;
        private PropertyTableManager _propertyTableManager;
        private IBindingProviderFactory BindingProviderFactory { get; set; } = new BindingProviderFactory();
        private IBindableControlFactory BindableControlFactory { get; set; } = new BindableControlFactory();

        public IBindingProvider BindingProvider { get; private set; }

        public InterfaceController(Control container)
        {
            _container = container;
            _propertyTableManager = new PropertyTableManager();
        }

        public void BindObjectToInterface(object target)
        {
            BindingProvider = BindingProviderFactory.Get(target);

            var bindings = BindingProvider.GetBindings();

            // TODO: можно оптимизировать процесс так, чтобы контролы переиспользовались
            _container.Controls.Clear();
            var controls = bindings.Select(BindableControlFactory.Get);

            var propertyTable = _propertyTableManager.Render(controls.ToArray());
            _container.Controls.Add(propertyTable);
            propertyTable.Dock = DockStyle.Fill;
        }
    }
}
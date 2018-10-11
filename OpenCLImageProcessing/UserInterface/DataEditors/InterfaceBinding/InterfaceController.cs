using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserInterface.DataEditors.InterfaceBinding.Controls;

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

            _container.Controls.Clear();
            var propertyTable = _propertyTableManager.Render(BindingProvider);
            _container.Controls.Add(propertyTable);

            propertyTable.Dock = DockStyle.Fill;
        }
    }
}
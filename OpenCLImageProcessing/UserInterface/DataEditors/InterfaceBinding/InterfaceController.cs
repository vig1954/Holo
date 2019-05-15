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
        private IPropertyRenderer _propertyRenderer;
        private IBindingProviderFactory BindingProviderFactory { get; set; } = new BindingProviderFactory();
        private IBindableControlFactory BindableControlFactory { get; set; } = new BindableControlFactory();

        public IBindingProvider BindingProvider { get; private set; }

        public InterfaceController(Control container, IPropertyRenderer renderer = null)
        {
            _container = container;
            _container.Resize += ContainerOnResize;
            _propertyRenderer = renderer ?? new PropertyTableManager();
        }

        private void ContainerOnResize(object sender, EventArgs e)
        {
            foreach (var control in _container.Controls.Cast<Control>())
            {
                control.Width = _container.ClientSize.Width - _container.Padding.Left - _container.Padding.Right;
            }
        }

        public void BindObjectToInterface(object target)
        {
            BindingProvider = BindingProviderFactory.Get(target);

            _container.Controls.Clear();
            var propertyContainer = _propertyRenderer.Render(BindingProvider);
            _container.Controls.Add(propertyContainer);

            propertyContainer.Dock = DockStyle.Fill;
        }
    }
}
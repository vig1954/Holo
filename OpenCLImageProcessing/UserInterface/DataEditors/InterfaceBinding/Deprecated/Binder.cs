using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.Deprecated
{
    public class Binder: IInterfaceBinder
    {
        private object _target;

        public IReadOnlyCollection<BindingBase> Bindings { get; private set; }
        public IEnumerable<BindingBase> AllBindings => Bindings.SelectMany(b => b is SubfieldGroupBinding subfieldGroupBinding ? subfieldGroupBinding.ComplexFieldBinder.Bindings : new[] {b});


        public Binder()
        {
        }

        public Binder(object target)
        {
            ProcessObject(target);
        }

        public void ProcessObject(object target)
        {
            _target = target;
            if (_target == null)
            {
                Bindings = new BindingBase[0];
                return;
            }

            var targetType = target.GetType();
            var members = targetType.GetMembers();

            Bindings = members.Select(m => BindingFactory.CreateFor(m, target))
                .Where(b => b != null)
                .SelectMany(b => b is MergeSubfieldsBinding mergeSubfieldsBinding ? mergeSubfieldsBinding.ComplexFieldBinder.Bindings : new[] {b})
                .ToArray();
        }

        public void PopulateControl(Control container)
        {
            foreach (var binding in Bindings)
            {
                var bindingControl = (Control)binding.Control;
                bindingControl.Width = container.ClientRectangle.Width;
                container.Controls.Add(bindingControl);
            }
        }

        public PropertyBindingBase GetPropertyBindingWithEmptyValueForType(Type type)
        {
            return AllBindings.OfType<PropertyBindingBase>().FirstOrDefault(b => b.PropertyType.IsAssignableFrom(type) && b.Get() == null);
        }

        public void Dispose()
        {
        }
    }
}

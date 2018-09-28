using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.DataEditors.InterfaceBinding.ControlsV2;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public abstract class ObservableCollectionBindingBase : PropertyBinding, IValueBinding, IBindableControlProvider
    {
        public delegate void ActionWithSenderParameter(object sender);

        public event ActionWithSenderParameter AllowedValuesUpdated;

        public ObservableCollectionBindingBase(PropertyInfo member, IBindingTargetProvider targetProvider) : base(member, targetProvider)
        {
            //if (member.PropertyType)
        }

        public ControlsV2.IBindableControl GetControl()
        {
            var dropdownControl = new DropdownControl();
            dropdownControl.SetBinding(this);

            return dropdownControl;
        }

        public abstract IEnumerable<object> GetAllowedValues();

        public abstract void SetAllowedValues(IEnumerable<object> allowedValues);

        protected void OnAllowedValuesUpdated(object sender)
        {
            AllowedValuesUpdated?.Invoke(sender);
        }
    }

    public class ObservableCollectionBinding<T> : ObservableCollectionBindingBase
    {
        public IReadOnlyCollection<T> AllowedValues { get; private set; }

        public ObservableCollectionBinding(PropertyInfo member, IBindingTargetProvider targetProvider) : base(member, targetProvider)
        {
            var valueCollectionAttribute = member.GetCustomAttribute<ValueCollectionAttribute>();

            AllowedValues = valueCollectionAttribute == null ? new T[] { } : valueCollectionAttribute.ValueCollection.Select(i => (T) i).ToArray();
        }

        public override IEnumerable<object> GetAllowedValues()
        {
            return AllowedValues.Select(v => (object) v);
        }

        public override void SetAllowedValues(IEnumerable<object> allowedValues)
        {
            AllowedValues = allowedValues.Select(v => (T) v).ToArray();
            OnAllowedValuesUpdated(this);
        }

        public void SetAllowedValues(IEnumerable<T> allowedValues)
        {
            AllowedValues = allowedValues.ToArray();
            OnAllowedValuesUpdated(this);
        }
    }
}

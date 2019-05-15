using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public abstract class ObservableCollectionBindingBase : PropertyBinding, IValueBinding, IBindableControlProvider
    {
        protected bool AllowDefaultValue { get; set; }
        public string DefaultValueDisplayText { get; set; }

        public object DefaultValue { get; set; }

        public delegate void ActionWithSenderParameter(object sender);

        public event ActionWithSenderParameter AllowedValuesUpdated;

        public ObservableCollectionBindingBase(PropertyInfo member, IBindingTargetProvider targetProvider) : base(member, targetProvider)
        {
            //if (member.PropertyType)
        }

        public IBindableControl GetControl()
        {
            var dropdownControl = new DropdownControl();
            dropdownControl.SetBinding(this);

            return dropdownControl;
        }

        public abstract IEnumerable<object> GetAllowedValues();

        public abstract void SetAllowedValues(IEnumerable<object> allowedValues);

        public override object GetValue()
        {
            return base.GetValue();
        }

        public override void SetValue(object value, object sender)
        {
            if (value == DefaultValue)
                value = (DefaultValue as DefaultValueWrapper).DefaultValue;

            base.SetValue(value, sender);
        }

        public bool IsDefaultValue(object value)
        {
            return (DefaultValue as DefaultValueWrapper).DefaultValue == value;
        }

        protected void OnAllowedValuesUpdated(object sender)
        {
            AllowedValuesUpdated?.Invoke(sender);
        }

        protected class DefaultValueWrapper
        {
            public object DefaultValue { get; set; }
            public string DefaultValueDisplayText { get; set; }

            public override string ToString() => DefaultValueDisplayText;
        }
    }

    public class ObservableCollectionBinding<T> : ObservableCollectionBindingBase
    {
        public IReadOnlyCollection<T> AllowedValues { get; private set; }

        public ObservableCollectionBinding(PropertyInfo member, IBindingTargetProvider targetProvider) : base(member, targetProvider)
        {
            var valueCollectionAttribute = member.GetCustomAttribute<ValueCollectionAttribute>();

            if (valueCollectionAttribute == null)
                throw new InvalidOperationException();

            AllowDefaultValue = valueCollectionAttribute.AllowDefaultValue;
            DefaultValueDisplayText = valueCollectionAttribute.DefaultValueDisplayText ?? "[No Value]";
            DefaultValue = new DefaultValueWrapper
            {
                DefaultValue = (T)typeof(T).GetDefaultValue(),
                DefaultValueDisplayText = DefaultValueDisplayText
            };

            if (valueCollectionAttribute.ValueCollectionProviderPropertyName.IsNullOrEmpty())
            {
                AllowedValues = valueCollectionAttribute.ValueCollection.Select(i => (T) i).ToArray();
                return;
            }

            var target = targetProvider.Target;
            var valueCollectionProviderProperty = target.GetType().GetProperty(valueCollectionAttribute.ValueCollectionProviderPropertyName);

            if (valueCollectionProviderProperty == null || !typeof(ICollection<T>).IsAssignableFrom(valueCollectionProviderProperty.PropertyType) || !typeof(INotifyCollectionChanged).IsAssignableFrom(valueCollectionProviderProperty.PropertyType))
                throw new InvalidOperationException();

            var value = valueCollectionProviderProperty.GetValue(target);

            if (value is INotifyCollectionChanged notifyCollectionChanged && value is ICollection<T> collection)
            {
                notifyCollectionChanged.CollectionChanged += (s, e) =>
                {
                    SetAllowedValues(collection);
                };

                AllowedValues = collection.ToArray();
            }
        }

        public override IEnumerable<object> GetAllowedValues()
        {
            var result = AllowedValues.Select(v => (object) v);

            if (AllowDefaultValue)
                result = result.Concat(new[] { DefaultValue });

            return result;
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
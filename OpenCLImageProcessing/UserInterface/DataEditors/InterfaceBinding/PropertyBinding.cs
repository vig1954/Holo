using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using Infrastructure;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class PropertyBinding : BindingBase, ISynchronizableBinding, IDisposable
    {
        private PropertyInfo _propertyInfo;

        public Type ValueType => _propertyInfo.PropertyType;

        public bool ReadOnly => _propertyInfo.CanWrite;

        public string SynchronizationKey { get; }
        public IBindingSynchronizer Synchronizer { get; private set; }

        public event Action<ValueUpdatedEventArgs> ValueUpdated;

        public PropertyBinding(PropertyInfo member, IBindingTargetProvider targetProvider) : base(member, targetProvider)
        {
            _propertyInfo = member;

            if (!ValueType.IsPrimitive)
                return;

            SynchronizationKey = $"{_propertyInfo.DeclaringType.Name}_{_propertyInfo.Name}";
            Synchronizer = new ValueBindingSynchronizer(this);
        }
        
        public void SetValue(object value, object sender)
        {
            var target = _targetProvider.Target;
            _propertyInfo.SetValue(target, value);

            ValueUpdated?.InvokeExcludingTarget(new ValueUpdatedEventArgs(sender), sender);
        }

        public object GetValue()
        {
            var target = _targetProvider.Target;
            return _propertyInfo.GetValue(target);
        }

        public void Dispose()
        {
            Synchronizer?.Dispose();
            Synchronizer = null;
        }
    }
}
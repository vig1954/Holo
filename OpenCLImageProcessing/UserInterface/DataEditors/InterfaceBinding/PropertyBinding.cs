﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class PropertyBinding : BindingBase, IValueBinding
    {
        private PropertyInfo _propertyInfo;

        public Type ValueType => _propertyInfo.PropertyType;

        public event Action<ValueUpdatedEventArgs> ValueUpdated;

        public PropertyBinding(PropertyInfo member, IBindingTargetProvider targetProvider) : base(member, targetProvider)
        {
            _propertyInfo = member;
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
    }
}

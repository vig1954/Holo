﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding
{
    
    public interface IBindingProvider
    {
        IEnumerable<IBinding> GetBindings();
    }

    public interface IBindingManager<TTarget> where TTarget: class
    {
        void SyncPropertyValue<TPropertyType>(Expression<Func<TTarget, TPropertyType>> propertyAccess);
        void SyncPropertyValue(string propertyName);
        void SetPropertyValue<TPropertyType>(Expression<Func<TTarget, TPropertyType>> propertyAccess, TPropertyType value);
        void SetPropertyValue<TPropertyType>(string propertyName, TPropertyType value);
        void SetAvailableValuesForProperty<TPropertyType>(Expression<Func<TTarget, TPropertyType>> propertyAccess, IEnumerable<TPropertyType> availableValues);
        void RaiseMemberBindingEvent<TMemberType>(Expression<Func<TTarget, TMemberType>> memberAccess, BindingEvent @event);
        void RaiseMethodBindingEvent(Expression<Action<TTarget>> methodCall, BindingEvent @event);
    }

    public static class BindingProviderExtensions
    {
        public static void SetFirstEmptyValue(this IBindingProvider self, object value, object sender)
        {
            var valueBindings = self.GetBindings().ToFlat().OfType<IValueBinding>().FilterOutHiddenProperties().FilterOutReadOnlyProperties();
            var valueBindingsOfValueType = valueBindings.Where(b => b.ValueType.IsInstanceOfType(value));
            var firstEmptyValueBindingOfValueType = valueBindingsOfValueType.FirstOrDefault(b => !b.HasValue());

            if (firstEmptyValueBindingOfValueType != null)
                firstEmptyValueBindingOfValueType.SetValue(value, sender);
        }
    }
}

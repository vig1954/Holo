using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBindingProviderFactory
    {
        IBindingProvider Get(object target);
    }

    public class BindingProviderFactory: IBindingProviderFactory
    {
        public IBindingProvider Get(object target)
        {
            if (target is IBindingProvider provider)
                return provider;

            return (IBindingProvider)typeof(ObjectBindingProvider<>).MakeGenericType(target.GetType()).InvokeConstructor(target);
        }
        private class ObjectBindingProvider<TTarget> : IBindingProvider, IDisposable, IBindingTargetProvider, IBindingManager<TTarget> where TTarget : class
        {
            private IEnumerable<IBinding> _bindings => _memberBindings.Select(g => g.Value);
            private IDictionary<MemberInfo, IBinding> _memberBindings;

            public IBindingFactory BindingFactory { get; set; } = new BindingFactory();
            public object Target { get; }

            public ObjectBindingProvider(object target)
            {
                Target = target;

                var properties = target.GetType().GetProperties();
                var bindingManagerProperty = properties.FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(IBindingManager<TTarget>)));
                if (bindingManagerProperty == null)
                    return;

                bindingManagerProperty.SetValue(target, this);
            }

            public IEnumerable<IBinding> GetBindings()
            {
                if (_memberBindings.IsNullOrEmpty())
                {
                    var targetMembers = Target.GetType().GetMembers().Where(m => m.HastAttribute<BindToUIAttribute>()).OrderBy(m => m.GetCustomAttribute<BindToUIAttribute>().Order);
                    _memberBindings = targetMembers.ToDictionary(m => m, m => BindingFactory.GetBinding(m, this));
                }

                return _bindings;
            }
            
            public void Dispose()
            {
                // dispose bindings???
                _memberBindings = null;
            }

            public void SetPropertyValue<TPropertyType>(Expression<Func<TTarget, TPropertyType>> propertyAccess, TPropertyType value)
            {
                GetPropertyBindingByPropertyAccessExpression(propertyAccess).SetValue(value, Target); // todo: возможно, нужно явно передавать sender
            }

            public void SetAvailableValuesForProperty<TPropertyType>(Expression<Func<TTarget, TPropertyType>> propertyAccess, IEnumerable<TPropertyType> availableValues)
            {
                var binding = GetPropertyBindingByPropertyAccessExpression(propertyAccess);

                var observableCollectionBinding = (ObservableCollectionBinding<TPropertyType>) binding;
                observableCollectionBinding.SetAllowedValues(availableValues);
            }

            private PropertyBinding GetPropertyBindingByPropertyAccessExpression<TPropertyType>(Expression<Func<TTarget, TPropertyType>> propertyAccess)
            {
                var propertyExpression = (MemberExpression) propertyAccess.Body;
                var propertyInfo = (PropertyInfo) propertyExpression.Member;
                var binding = (PropertyBinding)_memberBindings[propertyInfo];

                return binding;
            }
        }
    }
}

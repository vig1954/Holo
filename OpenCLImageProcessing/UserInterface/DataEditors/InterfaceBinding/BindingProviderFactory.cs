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
            private IDictionary<MemberInfo, IBinding> _memberBindings;

            public IBindingFactory BindingFactory { get; set; } = new BindingFactory();
            public object Target { get; }

            public ObjectBindingProvider(object target)
            {
                Target = target;

                var properties = target.GetType().GetProperties();
                var bindingManagerProperty = properties.FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(IBindingManager<TTarget>)));
                if (bindingManagerProperty != null)
                    bindingManagerProperty.SetValue(target, this);

                var targetMembers = Target.GetType().GetMembers().Where(m => m.HastAttribute<BindToUIAttribute>()).OrderBy(m => m.GetCustomAttribute<BindToUIAttribute>().Order);
                _memberBindings = targetMembers.ToDictionary(m => m, m => BindingFactory.GetBinding(m, this));

                var methodInfos = target.GetType().GetMethods();
                var onPropertyChangedMethods = methodInfos.Where(m => m.HastAttribute<OnBindedPropertyChangedAttribute>()).ToArray();
                var bindings = GetBindings().ToArray();
                if (onPropertyChangedMethods.Any())
                {
                    foreach (var methodInfo in onPropertyChangedMethods)
                    {
                        var propertyName = methodInfo.GetCustomAttribute<OnBindedPropertyChangedAttribute>().PropertyName;
                        var propertyBinding = bindings.OfType<PropertyBinding>().SingleOrDefault(pb => pb.Name == propertyName);

                        if (propertyBinding == null)
                            throw new InvalidOperationException($"Метод {target.GetType().Name}.{methodInfo.Name} имеет аттрибут {nameof(OnBindedPropertyChangedAttribute)}, связаный со свойством {propertyName}, однако это свойство не имеет привязок и поэтому не может быть прослушано с использованием указанного атрибута.");

                        propertyBinding.ValueUpdated += e => InvokeOnPropertyChangedMethod(methodInfo, e);
                    }
                }

                var onAnyPropertyChangedMethods = methodInfos.Where(m => m.HastAttribute<OnAnyBindedPropertyChangedAttribute>()).ToArray();
                if (onAnyPropertyChangedMethods.Any())
                {
                    foreach (var propertyBinding in bindings.OfType<PropertyBinding>())
                    {
                        propertyBinding.ValueUpdated += InvokeOnAnyPropertyChangedMethods;
                    }
                }

                void InvokeOnPropertyChangedMethod(MethodInfo methodInfo, ValueUpdatedEventArgs e)
                {
                    var methodParameterInfos = methodInfo.GetParameters();
                    if (methodParameterInfos.Length == 1 && methodParameterInfos.Single().ParameterType == typeof(ValueUpdatedEventArgs))
                        methodInfo.Invoke(target, new object[] { e });
                    else
                        methodInfo.Invoke(target, new object[] {});
                }

                void InvokeOnAnyPropertyChangedMethods(ValueUpdatedEventArgs e)
                {
                    foreach (var methodInfo in onAnyPropertyChangedMethods)
                    {
                        InvokeOnPropertyChangedMethod(methodInfo, e);
                    }
                }
            }

            public IEnumerable<IBinding> GetBindings()
            {
                return _memberBindings.Select(g => g.Value);
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

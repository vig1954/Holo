using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBindingFactory
    {
        IBinding GetBinding(MemberInfo member, IBindingTargetProvider targetProvider);
    }

    public class BindingFactory : IBindingFactory
    {
        public IBinding GetBinding(MemberInfo member, IBindingTargetProvider targetProvider)
        {
            if (member is PropertyInfo property)
            {
                if (property.HastAttribute<ValueCollectionAttribute>())
                    return (IBinding) typeof(ObservableCollectionBinding<>).MakeGenericType(property.PropertyType).InvokeConstructor(property, targetProvider);

                return new PropertyBinding(property, targetProvider);
            }

            if (member is MethodInfo method)
                return new MethodBinding(method, targetProvider);

            throw new NotSupportedException($"Member of type {member.GetType().Name} is not supported.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBindingFactory
    {
        IBinding GetBinding(MemberInfo member);
    }

    public class BindingFactory : IBindingFactory
    {
        public IBinding GetBinding(MemberInfo member)
        {
            if (member is PropertyInfo property)
                return new PropertyBinding(property);
            if (member is MethodInfo method)
                return new MethodBinding(method);

            throw new NotSupportedException($"Member of type {member.GetType().Name} is not supported.");
        }
    }
}

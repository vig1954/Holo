using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class MemberInfoExtensions
    {
        public static bool HastAttribute<TAttribute>(this MemberInfo self) where TAttribute : Attribute
        {
            return self.GetCustomAttributes<TAttribute>().Any();
        }

        public static bool HastAttribute<TAttribute>(this ParameterInfo self) where TAttribute : Attribute
        {
            return self.GetCustomAttributes<TAttribute>().Any();
        }
    }
}
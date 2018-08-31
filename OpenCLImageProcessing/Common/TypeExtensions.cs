using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class TypeExtensions
    {
        public static object GetDefaultValue(this Type self)
        {
            if (self.IsValueType)
                return Activator.CreateInstance(self);

            return null;
        }
    }
}

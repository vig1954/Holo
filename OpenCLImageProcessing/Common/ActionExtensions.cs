using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ActionExtensions
    {
        public static void InvokeExcludingTarget<T>(this Action<T> self, T value, object target)
        {
            foreach (var d in self.GetInvocationList())
            {
                if (d.Target != target)
                    d.DynamicInvoke(value);
            }
        }
    }
}

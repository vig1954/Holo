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

        public static object InvokeConstructor<TArg>(this Type self, TArg arg1)
        {
            var constructor = self.GetConstructor(new[] { typeof(TArg) });

            if (constructor == null)
                throw new InvalidOperationException($"Не удалось найти конструктор для типа {self}, принимающий {typeof(TArg)} в качестве единственного аргумента.");

            return constructor.Invoke(new[] { (object)arg1 });
        }

        public static object InvokeConstructor<TArg1, TArg2>(this Type self, TArg1 arg1, TArg2 arg2)
        {
            var constructor = self.GetConstructor(new[] { typeof(TArg1), typeof(TArg2) });

            if (constructor == null)
                throw new InvalidOperationException($"Не удалось найти конструктор для типа {self}, принимающий {typeof(TArg1)} и {typeof(TArg2)} в качестве аргументов.");

            return constructor.Invoke(new object[] { arg1, arg2 });
        }
    }
}

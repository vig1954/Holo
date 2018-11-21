using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Common
{
    public static class ObjectExtensions
    {
        public static T UnboxAndCastTo<T>(this object self)
        {
            // TODO: cache
            var unboxAndCastFunc = BuildUnboxAndCastFunc<T>(self.GetType());
            return unboxAndCastFunc(self);
        }

        public static object UnboxAndCastTo(this object self, Type castTo)
        {
            if (self is double doubleValue && castTo == typeof(float))
                return (float) doubleValue;

            if (self.GetType() == castTo)
                return self;

            // Не работает :)
//            var type = typeof(ObjectExtensions);
//            var method = type.GetMethod(nameof(BuildUnboxAndCastFunc), BindingFlags.NonPublic | BindingFlags.Static);
//            var buildUnboxAndCastFuncMethodInfo = method.MakeGenericMethod(castTo);
//            var unboxAndCastMethodFunc =  (Delegate)buildUnboxAndCastFuncMethodInfo.Invoke(null, new[] { castTo });
//
//            return unboxAndCastMethodFunc.DynamicInvoke(self);
            throw new NotImplementedException();
        }

        private static Func<object, T> BuildUnboxAndCastFunc<T>(Type objectType)
        {
            var parameter = Expression.Parameter(typeof(object), "o");
            var unboxCastExpression = Expression.Unbox(parameter, objectType);
            var castToExpression = Expression.MakeUnary(ExpressionType.Convert, unboxCastExpression, typeof(T));
            var lambda = (Expression<Func<object, T>>) Expression.Lambda(castToExpression, parameter);

            return lambda.Compile();
        }
    }
}

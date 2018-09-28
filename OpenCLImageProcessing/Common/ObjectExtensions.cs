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
            return BuildUnboxAndCastFunc<T>(self.GetType())(self);
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

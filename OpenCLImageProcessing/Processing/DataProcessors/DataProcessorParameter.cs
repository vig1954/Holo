using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;

namespace Processing.DataProcessors
{
    public abstract class DataProcessorParameterBase
    {
        protected object Value;
        public Type Type { get; }
        public string Name { get; } = "";
        public bool IsOutput { get; }
        public bool HasValue => Value != null;
        public event Action<DataProcessorParameterUpdatedEventArgs> ValueUpdated;

        protected DataProcessorParameterBase(Type type)
        {
            Type = type;
        }

        protected DataProcessorParameterBase(ParameterInfo parameterInfo)
        {
            Name = parameterInfo.Name;

            var customAttributes = parameterInfo.GetCustomAttributes();
            IsOutput = customAttributes.OfType<Processing.DataProcessors.DataProcessorParameterAttributes.OutputAttribute>().Any();

            Type = parameterInfo.ParameterType;
        }

        public object Get()
        {
            return Value;
        }

        public void Set(object value, object sender)
        {
            var oldValue = Value;
            Value = value;

            var e = new DataProcessorParameterUpdatedEventArgs(this, sender, oldValue);

            if (ValueUpdated == null)
                return;

            foreach (var d in ValueUpdated.GetInvocationList())
            {
                if (d.Target != sender)
                    d.DynamicInvoke(e);
            }
        }

        public override string ToString()
        {
            return $"[{Type.Name}]{(Name.IsNullOrEmpty() ? "Parameter" : Name)}";
        }
    }

    public class DataProcessorParameterUpdatedEventArgs
    {
        public DataProcessorParameterBase Parameter { get; }
        public object Sender { get; }
        public object OldValue { get; }

        public DataProcessorParameterUpdatedEventArgs(DataProcessorParameterBase parameter, object sender, object oldValue)
        {
            Parameter = parameter;
            Sender = sender;
            OldValue = oldValue;
        }
    }

    public class DataProcessorParameter<T> : DataProcessorParameterBase
    {
        public DataProcessorParameter() : base(typeof(T))
        {
        }

        public DataProcessorParameter(ParameterInfo parameterInfo) : base(parameterInfo)
        {
        }

        public new T Get() => (T) base.Get();

        public void Set(T value, object sender) => base.Set(value, sender);
    }

    public static class DataProcessorParameterFactory
    {
        public static DataProcessorParameterBase CreateFor(Type t)
        {
            var concreteType = typeof(DataProcessorParameter<>).MakeGenericType(t);
            return (DataProcessorParameterBase) Activator.CreateInstance(concreteType);
        }

        public static DataProcessorParameterBase CreateFor(ParameterInfo parameterInfo)
        {
            var concreteType = typeof(DataProcessorParameter<>).MakeGenericType(parameterInfo.ParameterType);
            var constructor = concreteType.GetConstructor(new[] {typeof(ParameterInfo)});
            return (DataProcessorParameterBase) constructor.Invoke(new object[] {parameterInfo});
        }
    }

    public static class DataProcessorParameterExtensions
    {
        public static IEnumerable<DataProcessorParameter<T>> ParametersOfType<T>(this IEnumerable<DataProcessorParameterBase> self)
        {
            var type = typeof(T);
            return self.Where(v => type.IsAssignableFrom(v.Type)).Select(v => (DataProcessorParameter<T>) v);
        }

        public static DataProcessorParameter<T> As<T>(this DataProcessorParameterBase self)
        {
            return (DataProcessorParameter<T>) self;
        }
    }
}
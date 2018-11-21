using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Common;
using Processing;
using UserInterface.DataEditors.InterfaceBinding;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataProcessorViews
{
    public abstract class DataProcessorParameterBase : ISynchronizableBinding, IDisposable
    {
        protected ParameterInfo _parameterInfo;
        protected object Value;

        public string DisplayName { get; }
        public string DisplayGroup { get; }

        public Type ValueType { get; }
        public string Name { get; } = "";
        public bool IsOutput { get; }
        public bool HasValue => Value != null;
        public string SynchronizationKey { get; }
        public IBindingSynchronizer Synchronizer { get; private set; }


        public event Action<ValueUpdatedEventArgs> ValueUpdated;

        protected DataProcessorParameterBase(Type type)
        {
            ValueType = type;

            SynchronizationKey = null;
            Synchronizer = null;
        }

        protected DataProcessorParameterBase(ParameterInfo parameterInfo)
        {
            Name = parameterInfo.Name;

            IsOutput = parameterInfo.HastAttribute<Processing.DataProcessors.DataProcessorParameterAttributes.OutputAttribute>() || parameterInfo.Name.ToLower() == "output" || parameterInfo.IsOut;

            ValueType = parameterInfo.ParameterType;

            _parameterInfo = parameterInfo;

            DisplayName = (GetAttribute<DisplayNameAttribute>()?.DisplayName ?? Name)?.SeparateUpperCase().FirstLetterToUpperCase();
            DisplayGroup = ""; // TODO

            if (parameterInfo.HasDefaultValue)
                Value = parameterInfo.DefaultValue;

            var defaultValueAttribute = parameterInfo.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValueAttribute != null)
                Value = defaultValueAttribute.Value;
            
            if (!ValueType.IsPrimitive)
                return;

            SynchronizationKey = $"{_parameterInfo.Member.DeclaringType.Name}_{parameterInfo.Member.Name}_{parameterInfo.Name}";
            Synchronizer = new ValueBindingSynchronizer(this);
        }

        public object GetValue()
        {
            return Value;
        }

        public void SetValue(object value, object sender)
        {
            var oldValue = Value;
            Value = value;

            var e = new DataProcessorParameterUpdatedEventArgs(this, sender, oldValue);

            ValueUpdated?.InvokeExcludingTarget(e, sender);
        }

        public override string ToString()
        {
            return $"[{ValueType.Name}]{(Name.IsNullOrEmpty() ? "Parameter" : Name)}";
        }

        
        public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
        {
            var attribute = _parameterInfo?.GetCustomAttribute<TAttribute>();

            if (attribute == null && typeof(TAttribute).IsAssignableFrom(typeof(BindToUIAttribute)))
                attribute = (TAttribute) (Attribute) new BindToUIAttribute(_parameterInfo.Name);

            if (attribute == null && typeof(TAttribute).IsAssignableFrom(typeof(BindMembersToUIAttribute)) && typeof(IImageHandler).IsAssignableFrom(ValueType))
                attribute = (TAttribute) (Attribute) new BindMembersToUIAttribute();

            return attribute;

        }

        public bool HasAttribute<TAttribute>() where TAttribute : Attribute
        {
            return _parameterInfo?.GetCustomAttribute<TAttribute>() != null;
        }
        public void Dispose()
        {
            Synchronizer?.Dispose();
            Synchronizer = null;
        }
    }

    public class DataProcessorParameterUpdatedEventArgs : ValueUpdatedEventArgs
    {
        public DataProcessorParameterBase Parameter { get; }
        public object OldValue { get; }

        public DataProcessorParameterUpdatedEventArgs(DataProcessorParameterBase parameter, object sender, object oldValue) : base(sender)
        {
            Parameter = parameter;
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

        public new T GetValue() => (T) base.GetValue();

        public void SetValue(T value, object sender) => base.SetValue(value, sender);
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
            return self.Where(v => type.IsAssignableFrom(v.ValueType)).Select(v => (DataProcessorParameter<T>) v);
        }

        public static DataProcessorParameter<T> As<T>(this DataProcessorParameterBase self)
        {
            return (DataProcessorParameter<T>) self;
        }
    }
}
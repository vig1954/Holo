using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBinding
    {
        string DisplayName { get; }
        string DisplayGroup { get; }

        TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute;

        event Action<BindingEvent> BindingEvent;
        void RaiseBindingEvent(BindingEvent @event);
    }

    public interface IBindingTargetProvider
    {
        object Target { get; }
    }

    public abstract class BindingBase : IBinding
    {
        protected MemberInfo _memberInfo;
        protected IBindingTargetProvider _targetProvider;

        public event Action<BindingEvent> BindingEvent;

        public string Name { get; }
        public string DisplayName { get; }
        public string DisplayGroup { get; } = "";

        public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
        {
            return _memberInfo.GetCustomAttribute<TAttribute>();
        }

        protected BindingBase(MemberInfo member, IBindingTargetProvider targetProvider)
        {
            _targetProvider = targetProvider;
            _memberInfo = member;

            Name = member.Name;

            var bindToUiAttribute = member.GetCustomAttribute<BindToUIAttribute>();
            if (bindToUiAttribute != null)
            {
                DisplayName = bindToUiAttribute.DisplayName;
                DisplayGroup = bindToUiAttribute.DisplayGroup;
            }

            if (DisplayName.IsNullOrEmpty())
                DisplayName = Name.SeparateUpperCase();
        }

        public void RaiseBindingEvent(BindingEvent @event)
        {
            BindingEvent?.Invoke(@event);
        }
    }

    public class BindingEvent
    {
        public string Event { get; }
        public object Param { get; }
        public object Sender { get; }

        public BindingEvent(string @event, object param, object sender)
        {
            Event = @event;
            Param = param;
            Sender = sender;
        }
    }
}
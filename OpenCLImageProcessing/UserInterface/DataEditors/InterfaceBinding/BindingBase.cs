using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBinding
    {
        string DisplayName { get; }
        string DisplayGroup { get; }
    }

    public abstract class BindingBase : IBinding
    {
        protected IBindingTargetProvider _targetProvider;

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DisplayGroup { get; set; } = "";

        protected BindingBase(MemberInfo member, IBindingTargetProvider targetProvider)
        {
            _targetProvider = targetProvider;

            Name = member.Name;

            var bindToUiAttribute = member.GetCustomAttribute<BindToUIAttribute>();
            if (bindToUiAttribute != null)
            {
                DisplayName = bindToUiAttribute.DisplayName;
                DisplayGroup = bindToUiAttribute.DisplayGroup;
            }
            else
            {
                DisplayName = Name;
            }
        }
    }
}
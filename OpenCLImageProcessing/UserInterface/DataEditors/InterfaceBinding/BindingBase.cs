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

        IBindableControl Control { get; }
    }

    public abstract class BindingBase : IBinding
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DisplayGroup { get; set; } = "";
        
        protected BindingBase(MemberInfo member)
        {
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

        public abstract IBindableControl Control { get; }
    }
}

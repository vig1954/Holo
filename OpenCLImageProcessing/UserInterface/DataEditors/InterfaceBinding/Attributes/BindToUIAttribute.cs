using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class BindToUIAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string DisplayGroup { get; set; }

        public BindToUIAttribute(string displayName = "", string displayGroup = "")
        {
            DisplayName = displayName;
            DisplayGroup = displayGroup;
        }
    }
}

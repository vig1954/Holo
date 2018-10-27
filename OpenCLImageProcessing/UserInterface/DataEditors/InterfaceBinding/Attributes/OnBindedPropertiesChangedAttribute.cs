using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OnBindedPropertiesChangedAttribute : Attribute
    {
        public string[] PropertyNames { get; set; }

        public OnBindedPropertiesChangedAttribute(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }
    }
}

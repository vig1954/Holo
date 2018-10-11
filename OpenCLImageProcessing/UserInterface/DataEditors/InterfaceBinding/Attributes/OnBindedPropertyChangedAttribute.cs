using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OnBindedPropertyChangedAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public OnBindedPropertyChangedAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OnAnyBindedPropertyChangedAttribute : Attribute
    {
    }
}

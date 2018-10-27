using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace UserInterface.DataEditors.InterfaceBinding.Attributes
{
    /// <summary>
    /// Атрибут, которым нужно помечать свойство, которое имеет ограниченный набор значений, возвращаемый другим свойством, имя которого указывается в конструкторе
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueCollectionAttribute: Attribute
    {
        public string ValueCollectionProviderPropertyName { get; set; }
        public object[] ValueCollection { get; }

        public ValueCollectionAttribute(params object[] values)
        {
            ValueCollection = values;
        }
    }
}

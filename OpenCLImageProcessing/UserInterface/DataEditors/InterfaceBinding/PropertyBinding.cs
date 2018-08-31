using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class PropertyBinding : BindingBase
    {
        public PropertyBinding(PropertyInfo member) : base(member)
        {
        }

        public override IBindableControl GetControl()
        {
            throw new NotImplementedException();
        }
    }
}

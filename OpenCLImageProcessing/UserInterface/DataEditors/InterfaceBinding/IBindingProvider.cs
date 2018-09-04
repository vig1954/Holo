using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding
{
    
    public interface IBindingProvider
    {
        IEnumerable<BindingBase> GetBindings();
    }
}

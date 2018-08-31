using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IInterfaceBinder : IBindingProvider, IDisposable
    {
        void ProcessObject(object obj);
    }

    public class InterfaceBinder : IInterfaceBinder
    {
        public IEnumerable<BindingBase> GetBindings()
        {
            throw new NotImplementedException();
        }

        public void ProcessObject(object obj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void PopulateControl(Control container)
        {
            throw new NotImplementedException();
        }
    }
}

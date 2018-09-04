using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBindingTargetProvider
    {
        object Target { get; }
    }

    public interface IObjectBindingProvider : IBindingProvider, IDisposable, IBindingTargetProvider
    {
        void SetObject(object obj);
    }

    public class ObjectBindingProvider : IObjectBindingProvider
    {
        public object Target { get; }

        public IEnumerable<BindingBase> GetBindings()
        {
            throw new NotImplementedException();
        }

        public void SetObject(object obj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

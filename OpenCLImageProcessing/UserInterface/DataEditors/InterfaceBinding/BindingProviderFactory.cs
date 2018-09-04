using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBindingProviderFactory
    {
        IBindingProvider Get(object target);
    }

    public class BindingProviderFactory: IBindingProviderFactory
    {
        public IBindingProvider Get(object target)
        {
            if (target is IBindingProvider provider)
                return provider;

            var objectBindingProvider = new ObjectBindingProvider();
            objectBindingProvider.SetObject(target);

            return objectBindingProvider;
        }
    }
}

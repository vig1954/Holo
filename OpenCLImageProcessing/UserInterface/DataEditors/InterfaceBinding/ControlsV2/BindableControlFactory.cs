using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public interface IBindableControlFactory
    {
        IBindableControl Get(IBinding binding);
    }

    public class BindableControlFactory: IBindableControlFactory
    {
        public IBindableControl Get(IBinding binding)
        {
            throw new NotImplementedException();
        }
    }
}

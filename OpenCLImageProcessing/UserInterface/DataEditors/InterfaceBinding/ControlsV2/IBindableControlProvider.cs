using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public interface IBindableControlProvider
    {
        IBindableControl GetControl();
    }
}

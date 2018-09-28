using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public interface IBindableControl
    {
        bool HideLabel { get; } // TODO: change to label mode, see BindToUI attribute
        IBinding Binding { get; }
        void SetBinding(IBinding binding);
    }
}

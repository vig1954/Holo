using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding.BindingEvents
{
    public class PerformBindableControlActionEvent : BindingEvent
    {
        private Action<IBindableControl> _act;

        public PerformBindableControlActionEvent(Action<IBindableControl> act, object sender) : base(nameof(PerformBindableControlActionEvent), act, sender)
        {
            _act = act;
        }

        public void Perform(IBindableControl control)
        {
            _act(control);
        }
    }
}
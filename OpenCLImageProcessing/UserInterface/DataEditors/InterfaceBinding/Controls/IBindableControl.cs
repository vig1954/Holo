using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBindableControl
    {
        string Title { get; set; }
        object Value { get; }
        void SetValue(object value, object sender);
    }

    public class BindableControlValueUpdatedEventArgs
    {
        public object Sender { get; }

        public BindableControlValueUpdatedEventArgs(object sender)
        {
            Sender = sender;
        }
    }
}

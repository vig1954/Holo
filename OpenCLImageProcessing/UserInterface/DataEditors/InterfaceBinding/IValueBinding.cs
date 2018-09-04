using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IValueBinding: IBinding
    {
        Type ValueType { get; }
        event Action<ValueUpdatedEventArgs> ValueUpdated;
        void SetValue(object value, object sender);
        object GetValue();
    }

    public class ValueUpdatedEventArgs
    {
        public object Sender { get; }

        public ValueUpdatedEventArgs(object sender)
        {
            Sender = sender;
        }
    }
}

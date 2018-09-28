using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Common;

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

    public static class ValueBindingExtensions
    {
        public static bool HasValue(this IValueBinding self)
        {
            return self.GetValue() == null || self.GetValue() == self.ValueType.GetDefaultValue();
        }
    }
}

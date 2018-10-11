using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

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
            return !(self.GetValue() == null || self.GetValue() == self.ValueType.GetDefaultValue());
        }

        public static IEnumerable<IValueBinding> FilterOutHiddenProperties(this IEnumerable<IValueBinding> self)
        {
            return self.Where(vb => !(vb.GetAttribute<BindMembersToUIAttribute>()?.HideProperty ?? false));
        }

        public static IEnumerable<IValueBinding> FilterOutReadOnlyProperties(this IEnumerable<IValueBinding> self)
        {
            return self.Where(vb => !(vb is PropertyBinding pb) || !pb.ReadOnly);
        }
    }

    public static class BindingExtensions
    {
        public static IEnumerable<IBinding> ToFlat(this IEnumerable<IBinding> self)
        {
            var bindingProviderFactory = new BindingProviderFactory();
            var bindings = self.SelectMany(BindingsToFlat);

            IEnumerable<IBinding> BindingsToFlat(IBinding binding)
            {
                if (binding is IValueBinding valueBinding)
                {
                    var bindMembersToUiAttribute = valueBinding.GetAttribute<BindMembersToUIAttribute>();
                    var value = valueBinding.GetValue();

                    if (value != null && bindMembersToUiAttribute != null)
                    {
                        var bindingProvider = bindingProviderFactory.Get(value);
                        return new[] { binding }.Concat(bindingProvider.GetBindings().SelectMany(BindingsToFlat));
                    }
                }

                return new[] { binding };
            }


            return bindings;
        }
    }
}

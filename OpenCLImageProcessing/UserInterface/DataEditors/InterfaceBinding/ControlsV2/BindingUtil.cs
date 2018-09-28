using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public static class BindingUtil
    {
        public static IValueBinding PrepareValueBinding(IBinding binding, IValueBinding previousBinding, Action<ValueUpdatedEventArgs> bindingValueUpdatedHandler, IReadOnlyCollection<Type> supportedTypes)
        {
            return PrepareValueBinding(binding, previousBinding, bindingValueUpdatedHandler, supportedTypes.Contains);
        }

        public static IValueBinding PrepareValueBinding(IBinding binding, IValueBinding previousBinding, Action<ValueUpdatedEventArgs> bindingValueUpdatedHandler, Func<Type, bool> supportedTypePredicate)
        {
            if (!(binding is IValueBinding valueBinding))
                throw new NotSupportedException($"Binding of type '{binding.GetType().Name}' is not supported.");

            if (!supportedTypePredicate(valueBinding.ValueType))
                throw new NotSupportedException($"Value of type '{valueBinding.ValueType.Name}' is not supported.");

            if (previousBinding != null)
                previousBinding.ValueUpdated -= bindingValueUpdatedHandler;
            
            valueBinding.ValueUpdated += bindingValueUpdatedHandler;

            return valueBinding;
        }
    }
}

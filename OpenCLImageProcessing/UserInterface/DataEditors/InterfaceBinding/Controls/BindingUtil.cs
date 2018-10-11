using System;
using System.Collections.Generic;
using System.Linq;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
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

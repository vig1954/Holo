using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public class DropdownControl : ComboBox, IBindableControl
    {
        private bool _suppressBindingSelectedIndexChangedEventHandlerExecution = false;
        private ObservableCollectionBindingBase _binding;

        public UiLabelMode LabelMode { get; private set; }

        public IBinding Binding => _binding;

        public DropdownControl()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void SetBinding(IBinding binding)
        {
            _binding = (ObservableCollectionBindingBase) BindingUtil.PrepareValueBinding(binding, _binding, BindingOnValueUpdated, t => true);

            _binding.AllowedValuesUpdated += BindingOnAllowedValuesUpdated;

            SetAllowedValues(_binding.GetAllowedValues().ToArray(), false);
            SetValue(_binding.GetValue());

            LabelMode = _binding.GetAttribute<BindToUIAttribute>().LabelMode;
        }

        private void BindingOnAllowedValuesUpdated(object sender)
        {
            SetAllowedValues(_binding.GetAllowedValues().ToArray(), true);
        }

        private void SetAllowedValues(IReadOnlyCollection<object> newItems, bool setDefaultValueIfNothingSelected)
        {
            var currentValue = SelectedItem;

            Items.Clear();

            foreach (var item in newItems)
            {
                
                Items.Add(item);

                if (setDefaultValueIfNothingSelected && currentValue == null || currentValue != null && currentValue.Equals(item))
                    currentValue = item;
            }

            if (currentValue != null)
                SelectedItem = currentValue;

            if (setDefaultValueIfNothingSelected && SelectedItem == null && newItems.Any())
                SelectedItem = newItems.FirstOrDefault();
        }

        private void BindingOnValueUpdated(ValueUpdatedEventArgs e)
        {
            if (e.Sender == this)
                return;

            SetValue(_binding.GetValue());
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (_suppressBindingSelectedIndexChangedEventHandlerExecution)
                return;

            _binding.SetValue(SelectedItem, this);
        }

        private void SetValue(object value)
        {
            _suppressBindingSelectedIndexChangedEventHandlerExecution = true;

            SelectedItem = _binding.IsDefaultValue(value) ? _binding.DefaultValue : value;

            _suppressBindingSelectedIndexChangedEventHandlerExecution = false;
        }
    }
}
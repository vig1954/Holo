﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public class CheckboxControl : CheckBox, IBindableControl
    {
        private bool _suppressBindingStateChangedEventHandlerExecution = false;
        private IValueBinding _binding;
        public bool HideLabel { get; private set; }
        public IBinding Binding => _binding;

        public void SetBinding(IBinding binding)
        {
            _binding = BindingUtil.PrepareValueBinding(binding, _binding, BindingOnValueUpdated, new[] { typeof(bool) });

            SetCheckedState((bool)_binding.GetValue());

            HideLabel = _binding.GetAttribute<BindToUIAttribute>().HideLabel;
        }

        private void BindingOnValueUpdated(ValueUpdatedEventArgs e)
        {
            if (e.Sender == this)
                return;

            SetCheckedState((bool)_binding.GetValue());
        }

        protected override void OnCheckStateChanged(EventArgs e)
        {
            base.OnCheckStateChanged(e);

            if (_suppressBindingStateChangedEventHandlerExecution)
                return;

            _binding.SetValue(Checked, this);
        }

        private void SetCheckedState(bool state)
        {
            _suppressBindingStateChangedEventHandlerExecution = true;
            Checked = state;
            _suppressBindingStateChangedEventHandlerExecution = false;
        }
    }
}

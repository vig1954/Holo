using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Processing.DataAttributes;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public partial class NumberControl : UserControl, IBindableControl
    {
        private IReadOnlyCollection<Type> _supportedTypes = new[] { typeof(int), typeof(float), typeof(double) };
        private int _precision = 0;
        private double _value;
        private double _minValue = double.MinValue;
        private double _maxValue = double.MaxValue;
        private double _addDelta = 1;
        private bool _suppressValueInputHandlerExecution = false;
        private IValueBinding _binding;

        public bool HideLabel { get; private set; }
        public IBinding Binding => _binding;

        public NumberControl()
        {
            InitializeComponent();
        }

        public void SetBinding(IBinding binding)
        {
            _binding = BindingUtil.PrepareValueBinding(binding, _binding, BindingOnValueUpdated, _supportedTypes);

            var value = _binding.GetValue().UnboxAndCastTo<double>();
            var precisionAttribute = _binding.GetAttribute<PrecisionAttribute>();

            _precision = precisionAttribute?.FractionalDigits ?? value.GetFractionalDigits();
            _addDelta = 1.0 / Math.Pow(10, _precision);

            SetValueInternal(value, false);

            HideLabel = _binding.GetAttribute<BindToUIAttribute>().HideLabel;
        }

        private void BindingOnValueUpdated(ValueUpdatedEventArgs obj)
        {
            if (obj.Sender == this)
                return;

            SetValueInternal(_binding.GetValue().UnboxAndCastTo<double>(), false);
        }

        private object GetValue(Type type)
        {
            if (type == typeof(int))
                return (int)_value;

            if (type == typeof(float))
                return (float) _value;

            return _value;
        }
        
        private void SetValueInternal(double value, bool updateBindingValue = true)
        {
            _value = value;

            if (_value < _minValue)
                _value = _minValue;

            if (_value > _maxValue)
                _value = _maxValue;

            _value = _precision > 0 ? Math.Round(_value, Math.Min(_precision, 15)) : Math.Round(_value);
            var format = _precision > 0 ? "#." + "0".Repeat(_precision) : "#";

            SetTextValue(_value.ToString(format));

            if (!updateBindingValue || _binding == null)
                return;

            _binding.SetValue(GetValue(_binding.ValueType), this);
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (_suppressValueInputHandlerExecution)
                return;

            if (double.TryParse(txtValue.Text, out var value))
            {
                SetInputInvalidState(false);

                if (!txtValue.Text.EndsWith(","))
                    SetValueInternal(value);
            }
            else
                SetInputInvalidState(true);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetValueInternal(_value + _addDelta);
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            SetValueInternal(_value - _addDelta);
        }

        private void SetInputInvalidState(bool invalid)
        {
            txtValue.BackColor = invalid ? Color.DarkOrange : Color.White;
        }

        private void SetTextValue(string value)
        {
            _suppressValueInputHandlerExecution = true;

            var cursorPos = txtValue.SelectionStart;
            txtValue.Text = value;
            txtValue.SelectionStart = cursorPos;

            _suppressValueInputHandlerExecution = false;
        }
    }
}

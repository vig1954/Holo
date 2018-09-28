using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public partial class EnumRadioGroupControl : UserControl, IBindableControl
    {
        private bool _suppressRadioCheckedChangedEventHandlerExecution = false;
        private IValueBinding _binding;

        public Enum _value;
        public bool HideLabel { get; private set; }
        public IBinding Binding => _binding;

        public void SetBinding(IBinding binding)
        {
            _binding = BindingUtil.PrepareValueBinding(binding, _binding, BindingValueUpdated, t => t.IsEnum);

            UpdateRadioButtons(_binding.ValueType);
            HideLabel = _binding.GetAttribute<BindToUIAttribute>().HideLabel;
            SetValue((Enum)_binding.GetValue());
        }

        public EnumRadioGroupControl()
        {
            InitializeComponent();
        }

        private void BindingValueUpdated(ValueUpdatedEventArgs e)
        {
            if (e.Sender == this)
                return;

            SetValue((Enum)_binding.GetValue());
        }

        private void UpdateRadioButtons(Type enumType)
        {
            var values = EnumExtensions.GetValues(enumType);

            foreach (var radio in groupBox1.Controls.OfType<RadioButton>())
            {
                groupBox1.Controls.Remove(radio);
            }

            var ry = 15;
            foreach (var value in values)
            {
                var radio = new RadioButton
                {
                    Left = 3,
                    Top = ry,
                    Text = value.ToString(),
                    Tag = value
                };

                radio.CheckedChanged += (o, e) =>
                {
                    if (_suppressRadioCheckedChangedEventHandlerExecution)
                        return;

                    if (radio.Checked)
                        UpdateValueInternal((Enum) radio.Tag);
                };

                groupBox1.Controls.Add(radio);

                ry += radio.Height + 3;
            }

            this.Height = ry;
        }

        private void SetValue(Enum value)
        {
            _suppressRadioCheckedChangedEventHandlerExecution = true;

            foreach (var radio in groupBox1.Controls.OfType<RadioButton>())
            {
                if (Equals((Enum) radio.Tag, value))
                    radio.Checked = true;
            }

            _suppressRadioCheckedChangedEventHandlerExecution = false;
        }

        private void UpdateValueInternal(Enum value, bool updateBindingValue = true)
        {
            _value = value;

            if (updateBindingValue)
                _binding.SetValue(_value, this);
        }
    }
}

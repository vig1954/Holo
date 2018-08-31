using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public partial class EnumRadioGroupControl : UserControl, IBindableControl
    {
        public event Action<BindableControlValueUpdatedEventArgs> ValueUpdated;

        public string Title
        {
            get => groupBox1.Text;
            set => groupBox1.Text = value;
        }

        public object Value => (Enum)groupBox1.Controls.OfType<RadioButton>().Single(r => r.Checked).Tag;
        
        public void SetValue(object value, object sender)
        {
            groupBox1.Controls.OfType<RadioButton>().Single(r => r.Tag.Equals(value)).Checked = true;

            ValueUpdated?.Invoke(new BindableControlValueUpdatedEventArgs(sender));
        }

        public EnumRadioGroupControl(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException("Type should be enum.");

            InitializeComponent();

            var values = EnumExtensions.GetValues(enumType);
            int x = 6, y = 19;
            foreach (var value in values)
            {
                var radioButton = new RadioButton
                {
                    Text = value.ToString(),
                    Location = new Point(x, y),
                    Tag = value,
                    Anchor = AnchorStyles.Left | AnchorStyles.Top
                };

                radioButton.CheckedChanged += (sender, args) =>
                {
                    if (radioButton.Checked)
                        ValueUpdated?.Invoke(new BindableControlValueUpdatedEventArgs(this));
                };
                
                groupBox1.Controls.Add(radioButton);

                y += 23;
            }

            Height = y + 5;
        }
    }
}

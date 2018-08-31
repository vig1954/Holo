using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public partial class DropdownWithLabel : UserControl, IBindableControl
    {
        public string Title
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        public object Value
        {
            get => DropDown.SelectedItem;
        }

        public ComboBox.ObjectCollection Items => DropDown.Items;

        public event Action<object, EventArgs> SelectedIndexChanged;

        public DropdownWithLabel()
        {
            InitializeComponent();
            DropDown.SelectedIndexChanged += (sender, args) =>
            {
                SelectedIndexChanged?.Invoke(sender, args);
                ValueUpdated?.Invoke(new BindableControlValueUpdatedEventArgs(sender));
            };
        }

        public event Action<BindableControlValueUpdatedEventArgs> ValueUpdated;
        public void SetValue(object value, object sender)
        {
            if (DropDown.Items.Contains(value))
                throw new InvalidOperationException();

            DropDown.SelectedValue = value;

            ValueUpdated?.Invoke(new BindableControlValueUpdatedEventArgs(sender));
        }
    }
}

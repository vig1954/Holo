using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public class CheckboxControl : CheckBox, IBindableControl
    {
        public string Title
        {
            get => this.Text;
            set => this.Text = value;
        }

        public object Value => Checked;

        public event Action<BindableControlValueUpdatedEventArgs> ValueUpdated;

        public CheckboxControl()
        {
            CheckedChanged += (s, e) => ValueUpdated?.Invoke(new BindableControlValueUpdatedEventArgs(this));
        }

        public void SetValue(object value, object sender)
        {
            Checked = (bool) value;

            ValueUpdated?.Invoke(new BindableControlValueUpdatedEventArgs(sender));
        }
    }
}

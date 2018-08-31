using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public class ButtonControl : Button, IBindableControl
    {
        public string Title
        {
            get => Text;
            set => Text = value;
        }

        public object Value => null;
        public event Action<BindableControlValueUpdatedEventArgs> ValueUpdated;

        public void SetValue(object value, object sender)
        {
            throw new NotImplementedException();
        }
    }
}
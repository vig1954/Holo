using System.Drawing;
using System.Windows.Forms;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public class LabelControl: Label, IBindableControl
    {
        private IValueBinding _binding;
        public bool HideLabel { get; private set; }
        public IBinding Binding => _binding;

        public LabelControl()
        {
            AutoSize = false;
            TextAlign = ContentAlignment.MiddleLeft;
            Dock = DockStyle.Fill;
            AutoEllipsis = true;
        }
        
        public void SetBinding(IBinding binding)
        {
            _binding = BindingUtil.PrepareValueBinding(binding, _binding, BindingOnValueUpdated, t => true);

            HideLabel = _binding.GetAttribute<BindToUIAttribute>().HideLabel;

            SetValue(_binding.GetValue());
        }

        private void BindingOnValueUpdated(ValueUpdatedEventArgs e)
        {
            if (e.Sender == this)
                return;

            SetValue(_binding.GetValue());
        }

        private void SetValue(object value)
        {
            Text = value?.ToString() ?? "Not Set";
        }
    }
}

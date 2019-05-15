using System;
using System.Windows.Forms;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public class ButtonControl : Button, IBindableControl
    {
        private MethodBinding _binding;

        public UiLabelMode LabelMode => UiLabelMode.None;
        public IBinding Binding => _binding;

        public ButtonControl()
        {
            this.Height = 20;
        }

        public void SetBinding(IBinding binding)
        {
            if (!(binding is MethodBinding methodBinding))
                throw new NotSupportedException();

            _binding = methodBinding;
            Text = _binding.DisplayName;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            _binding.Invoke();
        }
    }
}

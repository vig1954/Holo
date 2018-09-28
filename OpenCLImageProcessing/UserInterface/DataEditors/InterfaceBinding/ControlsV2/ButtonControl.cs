using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.ControlsV2
{
    public class ButtonControl : Button, IBindableControl
    {
        private MethodBinding _binding;

        public bool HideLabel { get; } = true;
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

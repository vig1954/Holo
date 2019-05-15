using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public interface IBindableControl
    {
        UiLabelMode LabelMode { get; } // TODO: change to label mode, see BindToUI attribute
        IBinding Binding { get; }
        void SetBinding(IBinding binding);
    }
}

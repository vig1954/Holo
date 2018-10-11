namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public interface IBindableControl
    {
        bool HideLabel { get; } // TODO: change to label mode, see BindToUI attribute
        IBinding Binding { get; }
        void SetBinding(IBinding binding);
    }
}

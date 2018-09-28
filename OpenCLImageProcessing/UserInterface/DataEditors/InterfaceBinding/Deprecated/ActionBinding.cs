using System.Reflection;
using System.Windows.Forms;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding.Deprecated
{
    public class ActionBinding : BindingBase
    {
        private readonly MethodInfo _methodInfo;
        private readonly ButtonControl _button;

        public override Control Control => _button;

        public ActionBinding(ActionAttribute actionAttribute, MemberInfo memberInfo, object target) : base(actionAttribute, memberInfo, target)
        {
            if (memberInfo.MemberType != MemberTypes.Method)
                ThrowCantCreateBindingForMemberTypeException();

            _methodInfo = memberInfo.DeclaringType.GetMethod(memberInfo.Name);

            DisplayGroup = actionAttribute.Group;

            _button = new ButtonControl {Text = actionAttribute.TooltipText};
            _button.Click += (sender, args) => Invoke();
        }

        public void Invoke()
        {
            _methodInfo.Invoke(Target, null);
        }
    }
}

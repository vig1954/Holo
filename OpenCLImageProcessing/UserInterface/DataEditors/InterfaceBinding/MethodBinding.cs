using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class MethodBinding : BindingBase
    {
        private MethodInfo _methodInfo;

        public MethodBinding(MethodInfo member, IBindingTargetProvider targetProvider) : base(member, targetProvider)
        {
            _methodInfo = member;
        }

        public void Invoke()
        {
            var target = _targetProvider.Target;
            _methodInfo.Invoke(target, new object[] { null });
        }
    }
}

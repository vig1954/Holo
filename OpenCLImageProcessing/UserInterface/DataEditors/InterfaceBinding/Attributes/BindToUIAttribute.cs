using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding.Attributes
{
    /// <summary>
    /// Если необходимо задать значение свойства так, чтобы это отразилось в интерфейсе, это необходимо делать через <see cref="IBindingManager{TTarget}"/>.
    /// Для этого заводится свойство с типом значения <see cref="IBindingManager{TTarget}"/>, которому будет автоматически назначено значение при привязке свойств к интерфейсу.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class BindToUIAttribute : Attribute
    {
        public string DisplayName { get; }
        public string DisplayGroup { get; }
        public int Order { get; }

        public UiLabelMode LabelMode { get; } // TODO: replace with label position? None / Row / Title

        public BindToUIAttribute(string displayName = "", string displayGroup = "", UiLabelMode labelMode = UiLabelMode.Title, [CallerLineNumber]int order = 0)
        {
            DisplayName = displayName;
            DisplayGroup = displayGroup;
            LabelMode = labelMode;
            Order = order;
        }
    }

    public enum UiLabelMode
    {
        None,
        Inline,
        Title
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface.DataEditors.InterfaceBinding.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BindMembersToUIAttribute : Attribute
    {
        public bool HideProperty { get; set; }
        public bool MergeMembers { get;set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.DataAttributes
{
    [AttributeUsage(AttributeTargets.Class /*TODO: remove*/ | AttributeTargets.Method)]
    public class DataProcessorAttribute : Attribute
    {
        public string MenuItem { get; set; } = "Processors";
        public string Name { get; set; }
        public string Group { get; set; }
        public string Tooltip { get; set; }

        public DataProcessorAttribute(string name = "", string group = "", string menuItem = "", string tooltip = "")
        {
            Name = name;
            Group = group;
            Tooltip = tooltip;
            MenuItem = menuItem;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Processing.DataAttributes;

namespace UserInterface.DataProcessorViews
{
    public class DataProcessorInfo
    {
        public string MenuItem { get; }
        public string Name { get; }
        public string Group { get; }
        public string Tooltip { get; }

        public DataProcessorInfo(MethodInfo methodInfo)
        {
            var dataProcessorAttribute = methodInfo.GetCustomAttribute<DataProcessorAttribute>();

            if (dataProcessorAttribute == null)
                throw new InvalidOperationException("Метод должен иметь DataProcessor аттрибут.");

            MenuItem = dataProcessorAttribute.MenuItem;
            Name = dataProcessorAttribute.Name;
            Group = dataProcessorAttribute.Group;
            Tooltip = dataProcessorAttribute.Tooltip;
        }
    }
}

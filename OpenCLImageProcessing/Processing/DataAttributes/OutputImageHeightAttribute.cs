using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.DataAttributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class OutputImageHeightAttribute : Attribute
    {
    }
}
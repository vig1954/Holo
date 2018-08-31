using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.DataProcessors.DataProcessorParameterAttributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class OutputAttribute : Attribute
    {
    }
}
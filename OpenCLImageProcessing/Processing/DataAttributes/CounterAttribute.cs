using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.DataAttributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CounterAttribute : Attribute
    {
        public float Increment { get; }

        public CounterAttribute(float increment = 1)
        {
            Increment = increment;
        }
    }
}
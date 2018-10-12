using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.DataAttributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class PrecisionAttribute : Attribute
    {
        public int FractionalDigits { get; }

        public PrecisionAttribute(int fractionalDigits)
        {
            FractionalDigits = fractionalDigits;
        }
    }
}

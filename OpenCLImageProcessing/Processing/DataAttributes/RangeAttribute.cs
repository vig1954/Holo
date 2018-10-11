using System;

namespace Processing.DataAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        public double? Min { get; set; }
        public double? Max { get; set; }

        public RangeAttribute(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public RangeAttribute()
        {

        }
    }
}

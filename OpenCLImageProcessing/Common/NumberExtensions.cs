using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class NumberExtensions
    {
        public static float Abs(this float self)
        {
            return Math.Abs(self);
        }

        public static int Abs(this int self)
        {
            return Math.Abs(self);
        }

        public static int GetFractionalDigits(this double self)
        {
            var stringRepresentation = self.ToString();

            var eDigits = 0;
            var indexOfE = stringRepresentation.IndexOf("E");
            if (indexOfE > 0)
            {
                eDigits = int.Parse(stringRepresentation.Substring(indexOfE + 1)).Abs();
                stringRepresentation = stringRepresentation.Substring(0, indexOfE);
            }

            var indexOfPoint = stringRepresentation.IndexOf(",");
            if (indexOfPoint > 0)
                return stringRepresentation.Length - indexOfPoint - 1 + eDigits;
            
            return eDigits;
        }
    }
}

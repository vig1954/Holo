using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Common
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static string Join(this IEnumerable<string> self, string separator)
        {
            return String.Join(separator, self);
        }

        public static string ToCommaSeparated(this IEnumerable<string> self)
        {
            return self.Join(", ");
        }

        public static string Repeat(this string self, int times)
        {
            var result = "";
            for (var i = 0; i < times; i++)
            {
                result += self;
            }

            return result;
        }

        public static string SeparateUpperCase(this string self, string separator = " ")
        {
            var entries = Regex.Split(self, "(?<!^)(?=[A-Z0-9])");
            return entries.Join(separator);
        }

        public static string FirstLetterToUpperCase(this string self)
        {
            return self.Substring(0, 1).ToUpper() + self.Substring(1);
        }

        public static string ToJson(this object self)
        {
            return JsonConvert.SerializeObject(self);
        }

        public static T FromJson<T>(this string self)
        {
            return JsonConvert.DeserializeObject<T>(self);
        }
    }
}

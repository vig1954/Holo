﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
            var entries = Regex.Split(self, "(?<!^)(?=[A-Z])");
            return entries.Join(separator);
        }
    }
}

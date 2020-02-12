using System;
using System.Collections.Generic;
using System.Text;

namespace MyKindergarten.ExtensionMethods
{
    internal static class StringExtensions
    {
        internal static bool IsNullOrWhitespace (this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        internal static string Format(this string input, params object[] values)
        {
            return string.Format(input, values);
        }
    }
}

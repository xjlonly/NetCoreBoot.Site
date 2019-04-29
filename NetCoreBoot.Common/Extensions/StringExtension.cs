using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBoot.Common
{
    public static class StringExtension
    {
        public static bool IsEmptyOrNull(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}

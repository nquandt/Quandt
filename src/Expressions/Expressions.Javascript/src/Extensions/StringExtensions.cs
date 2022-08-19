using System;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Expressions.Javascript.Extensions
{
    public static class StringExtensions
    {
        public static int CharCodeAt(this string str, int index)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return (int)str[index];
        }
    }
}

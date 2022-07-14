using System;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Abstractions
{
    public class SupportsQueryFormatsAttribute : Attribute
    {
        public string[] FormatTypes { get; private set; }
        public SupportsQueryFormatsAttribute(params string[] types)
        {
            FormatTypes = types ?? new string[0];
        }
    }
}

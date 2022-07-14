using System;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Abstractions
{
    public class SupportsAdditionalContentTypesAttribute : Attribute
    {
        public string[] ContentTypes { get; private set; }
        public SupportsAdditionalContentTypesAttribute(params string[] types)
        {
            ContentTypes = types ?? new string[0];
        }
    }
}

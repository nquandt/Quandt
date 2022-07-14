using System;
using System.Collections.Generic;

namespace Quandt.Endpoints.Models
{
    internal class ApiRegistration
    {
        public string Template { get; set; } = "";
        public List<string> SupportedHttpMethods { get; set; } = new List<string>();
        public Type? Type { get; set; } = null;
    }
}

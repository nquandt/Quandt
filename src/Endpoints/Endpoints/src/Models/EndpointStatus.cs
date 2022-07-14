using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Endpoints.Models
{
    internal class EndpointStatus
    {
        public bool IsOkay { get; set; } = true;
        public string Message { get; set; } = "";
        public object Error { get; set; } = null;
    }
}

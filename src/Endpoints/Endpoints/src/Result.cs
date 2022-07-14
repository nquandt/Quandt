using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Endpoints
{
    public class Result
    {
        public bool Success { get; set; } = true;
        public object Error { get; set; } = null;
        public string Message { get; set; } = "";
    }
    public class Result<T> : Result
    {
        public T ResultObject { get; set; } = default(T);
        
    }
}

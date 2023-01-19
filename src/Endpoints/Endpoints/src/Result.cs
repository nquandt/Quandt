using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Endpoints
{
    public enum Status
    {
        SUCCESS,
        ERROR,
        BADREQUEST,
        UNAUTHORIZED
    }

    public class Result
    {
        public Status Status { get; set; } = Status.SUCCESS;
        public object? Error { get; set; } = null;
        public string? Message { get; set; } = null;
    }

    public class Result<T> : Result where T : class
    {
        public T? Data { get; set; } = null;
    }
}

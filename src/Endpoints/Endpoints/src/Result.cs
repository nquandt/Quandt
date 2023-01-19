using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonPropertyOrder(order: 0)]
        public Status Status { get; set; } = Status.SUCCESS;
        [JsonPropertyOrder(order: 1)]
        public string? Message { get; set; } = null;
        [JsonPropertyOrder(order: 2)]
        public object? Error { get; set; } = null;                
    }

    public class Result<T> : Result where T : class
    {
        [JsonPropertyOrder(order: 3)]
        public T? Data { get; set; } = null;
    }
}

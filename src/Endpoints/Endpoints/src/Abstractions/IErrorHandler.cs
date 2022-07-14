using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints.Abstractions
{
    internal interface IErrorHandler
    {
        Task Handle(HttpContext context, EndpointStatus status, CancellationToken ct);
    }
}

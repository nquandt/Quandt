using Microsoft.AspNetCore.Http;
using Quandt.Endpoints.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints.Abstractions
{
    internal interface IErrorHandler
    {
        Task Handle(HttpContext context, EndpointStatus status, CancellationToken ct);
    }
}

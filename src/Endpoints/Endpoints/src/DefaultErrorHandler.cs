using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Quandt.Endpoints.Abstractions;
using Quandt.Endpoints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints
{
    internal class DefaultErrorHandler : IErrorHandler
    {
        public DefaultErrorHandler(IServiceProvider serviceProvider) { }

        public async Task Handle(HttpContext context, EndpointStatus status, CancellationToken ct)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var json = $"{{\"Message\":\"{status.Message}\"}}";

            await context.Response.WriteAsync(json, ct);
        }
    }
}

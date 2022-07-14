using Quandt.Endpoints.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints.PipelineHandlers
{
    internal class DefaultHandler : IHandler<IBaseEndpointAsync>
    {
        public async Task Handle(IBaseEndpointAsync endpoint, CancellationToken ct)
        {
            await endpoint.HandleContextAsync(ct);
        }
    }
}

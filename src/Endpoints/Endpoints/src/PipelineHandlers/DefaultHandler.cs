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
        Func<IBaseEndpointAsync, CancellationToken, Task>[] IHandler<IBaseEndpointAsync>.Funcs => new Func<IBaseEndpointAsync, CancellationToken, Task>[]
        {
            (IBaseEndpointAsync baseEndpointAsync, CancellationToken ct) => baseEndpointAsync.HandleContextAsync(ct)
        };
    }
}

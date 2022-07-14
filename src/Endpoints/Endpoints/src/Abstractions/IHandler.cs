using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints.Abstractions
{
    internal interface IHandler<T> : IHandler
    {
        //Func<IBaseEndpointAsync, CancellationToken, Task>[] Funcs { get; }
        //Task Handle(IBaseEndpointAsync endpoint, CancellationToken cancellationToken);
    }

    internal interface IHandler
    {
        Task Handle(IBaseEndpointAsync endpoint, CancellationToken ct);
    }
}

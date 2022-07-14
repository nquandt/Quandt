using Quandt.Endpoints.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Endpoints.Abstractions
{
    internal interface IEndpointPipeline<T> : IEndpointPipeline where T : IBaseEndpointAsync
    {
        List<IHandler<T>> Handlers { get; set; }
    }

    internal interface IEndpointPipeline { }
}

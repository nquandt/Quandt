using Quandt.Endpoints.Abstractions;
using Quandt.Endpoints.PipelineHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Endpoints
{
    internal class WithRequestEndpointPipeline<T> : IEndpointPipeline<IWithRequestEndpoint>
    {
        public List<IHandler<IWithRequestEndpoint>> Handlers { get; set; } = new List<IHandler<IWithRequestEndpoint>>()
        {
            new DefaultWithRequestHandler<T>()
        };
    }
}

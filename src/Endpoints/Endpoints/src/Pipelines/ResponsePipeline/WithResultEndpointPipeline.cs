using Quandt.Endpoints.Abstractions;
using Quandt.Endpoints.PipelineHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Endpoints
{
    internal class WithResultEndpointPipeline<T> : IEndpointPipeline<IWithResultEndpoint>
    {
        public List<IHandler<IWithResultEndpoint>> Handlers { get; set; } = new List<IHandler<IWithResultEndpoint>>()
        {
            new DefaultWithResultHandler<T>()
        };
    }
}

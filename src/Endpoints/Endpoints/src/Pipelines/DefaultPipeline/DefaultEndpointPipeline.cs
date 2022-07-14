using Quandt.Endpoints.Abstractions;
using Quandt.Endpoints.PipelineHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Endpoints
{
    internal class DefaultEndpointPipeline : IEndpointPipeline<IBaseEndpointAsync>
    {
        public List<IHandler<IBaseEndpointAsync>> Handlers { get; set; } = new List<IHandler<IBaseEndpointAsync>>()
        {
            new DefaultHandler()
        };
    }
}

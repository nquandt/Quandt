using Microsoft.AspNetCore.Http;
using Quandt.Abstractions;
using Quandt.Endpoints.Models;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Quandt.Endpoints.Abstractions
{
    internal class AsyncEndpointDelegate<T> where T : IBaseEndpointAsync
    {
        public async Task ExecuteAsync(T api, EndpointPipelineRunner pipeline, HttpContext context, ISerializerFactory serializerFactory, CancellationToken cancellationToken)
        {
            api.Context = context;
            api.SerializerFactory = serializerFactory;
            await pipeline.Handle(api, cancellationToken);
        }
    }
    internal class EndpointDelegate<T> where T : IBaseEndpoint
    {
        public void Execute(T api, HttpContext context, ISerializerFactory serializerFactory)
        {
            api.Context = context;
            api.Serializer = serializerFactory.GetSerializer("__default");
            //api.HandleContext();
        }
    }

    internal interface IWithResultEndpoint<T> : IWithResultEndpoint { }
    internal interface IWithResultEndpoint : IBaseEndpointAsync { }

    internal interface IWithRequestEndpoint<T> : IWithRequestEndpoint { }
    internal interface IWithRequestEndpoint : IBaseEndpointAsync { }

    internal interface IBaseEndpointAsync
    {
        internal object Result { get; set; }

        internal object Request { get; set; }
        internal EndpointStatus Status { get; }
        HttpContext Context { get; set; }
        internal ISerializerFactory SerializerFactory { get; set; }
        internal Task HandleContextAsync(CancellationToken ct);
    }
    internal interface IBaseEndpoint
    {
        public HttpContext Context { get; set; }
        public ISerializer Serializer { get; set; }
        //internal void HandleContext();
    }
}

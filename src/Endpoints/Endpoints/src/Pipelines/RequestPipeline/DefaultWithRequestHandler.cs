using Microsoft.AspNetCore.Http;
using Quandt.Endpoints.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints.PipelineHandlers
{
    internal class DefaultWithRequestHandler<T> : IHandler<IWithRequestEndpoint>
    {
        private readonly Func<T> _createNewT;
        private readonly EndpointModelCache<T> _cache;
        public DefaultWithRequestHandler()
        {
            _cache = new EndpointModelCache<T>();

            var ctor = typeof(T).GetConstructor(new Type[0]);            
            var lamda = Expression.Lambda(typeof(Func<T>), Expression.New(ctor));

            _createNewT = (Func<T>)lamda.Compile();
        }

        public async Task Handle(IBaseEndpointAsync endpoint, CancellationToken ct)
        {
            var ep = (IWithRequestEndpoint<T>)endpoint;
            
            endpoint.Request = await GetModelFromContext(endpoint.Context, endpoint.SerializerFactory, ct);
        }

        private async Task<T> GetModelFromContext(HttpContext context, ISerializerFactory serializerFactory, CancellationToken ct)
        {
            T model;
            //var result = new Result<TRequest>();
            if (context.Request.Method != HttpMethods.Get
                && context.Request.ContentLength > 0
                && context.Request.ContentType != null
                && serializerFactory.SupportsContentType(context.Request.ContentType))
            {
                using (var stream = context.Request.Body)
                {
                    var serializer = serializerFactory.GetSerializer(context.Request.ContentType);

                    model = await serializer.DeserializeAsync<T>(stream);
                }
            }
            else
            {
                model = _createNewT();
            }

            _cache.SetProperties(context, model);

            return model;
        }
    }
}

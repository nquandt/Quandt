using Microsoft.AspNetCore.Http;
using Quandt.Endpoints.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints.PipelineHandlers
{
    internal class DefaultWithRequestHandler<T> : IHandler<IWithRequestEndpoint>
    {
        private static async Task<T> GetModelFromContext(HttpContext context, ISerializerFactory serializerFactory, CancellationToken ct)
        {
            //var result = new Result<TRequest>();
            if (context.Request.Method != HttpMethods.Get
                && context.Request.ContentLength > 0
                && context.Request.ContentType != null
                && serializerFactory.SupportsContentType(context.Request.ContentType))
            {
                using (var stream = context.Request.Body)
                {
                    var serializer = serializerFactory.GetSerializer(context.Request.ContentType);

                    return await serializer.DeserializeAsync<T>(stream);
                }
            }


            return default(T);
        }

        private static async Task ConvertStreamToModel(IBaseEndpointAsync endpoint, CancellationToken ct)
        {
            endpoint.Request = await GetModelFromContext(endpoint.Context, endpoint.SerializerFactory, ct);
        }

        Func<IBaseEndpointAsync, CancellationToken, Task>[] IHandler<IWithRequestEndpoint>.Funcs => new Func<IBaseEndpointAsync, CancellationToken, Task>[]
        {
            ConvertStreamToModel
        };
    }
}

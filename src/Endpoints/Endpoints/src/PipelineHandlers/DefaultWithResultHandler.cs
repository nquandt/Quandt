using Quandt.Abstractions;
using Quandt.Endpoints.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints.PipelineHandlers
{

    internal class DefaultWithResultHandler<T> : IHandler<IWithResultEndpoint>
    {
        Func<IBaseEndpointAsync, CancellationToken, Task>[] IHandler<IWithResultEndpoint>.Funcs => new Func<IBaseEndpointAsync, CancellationToken, Task>[]
        {
            ResponseWithModel
        };


        private static async Task ResponseWithModel(IBaseEndpointAsync endpoint, CancellationToken ct)
        {
            var context = endpoint.Context;
            var serializerFactory = endpoint.SerializerFactory;

            ISerializer serializer;
            if (context.Request.Query.ContainsKey("format") && serializerFactory.SupportsContentType(context.Request.Query["format"][0]))
            {
                serializer = serializerFactory.GetSerializer(context.Request.Query["format"]);
            }
            else if (context.Request.Headers.Accept.Count > 0 && context.Request.Headers.Accept[0] != "*/*")
            {
                if (serializerFactory.SupportsContentTypes(out string contentType, context.Request.Headers.Accept.ToArray()))
                {
                    serializer = serializerFactory.GetSerializer(contentType);
                }
                else
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    endpoint.Status.IsOkay = false;
                    return;
                    //return new Result()
                    //{
                    //    Success = false,
                    //    Message = "There is no serializer that supports that Accept type"
                    //};
                }
            }
            else
            {
                serializer = serializerFactory.GetSerializer("__default");
            }

            using (var memStream = new MemoryStream())
            {
                await serializer.SerializeAsync(memStream, endpoint.Result);
                memStream.Position = 0;

                context.Response.ContentType = serializer.ContentType;
                context.Response.ContentLength = memStream.Length;
                await memStream.CopyToAsync(context.Response.Body);
            }

            return;
        }
    }

}

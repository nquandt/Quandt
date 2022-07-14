using Microsoft.AspNetCore.Http;
using Quandt.Abstractions;
using System.IO;

namespace Quandt.Endpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// </summary>
    //public static class BaseEndpointSync
    //{
    //    public static class WithRequest<TRequest> where TRequest : new()
    //    {
    //        public abstract class WithResult<TResponse> : WithRequestEndpointSync<TRequest>
    //        {
    //            public abstract TResponse Handle(TRequest request);
    //            internal override void HandleContext()
    //            {
    //                var model = GetRequestModel();

    //                var obj = Handle(model);

    //                using (var memStream = new MemoryStream())
    //                {
    //                    Serializer.SerializeSync<TResponse>(memStream, obj);
    //                    memStream.Position = 0;

    //                    Context.Response.ContentType = "application/json";
    //                    Context.Response.ContentLength = memStream.Length;
    //                    memStream.CopyTo(Context.Response.Body);
    //                }
    //            }
    //        }

    //        public abstract class WithoutResult : WithRequestEndpointSync<TRequest>
    //        {
    //            public abstract void Handle(TRequest request);
    //            internal override void HandleContext()
    //            {
    //                var model = GetRequestModel();

    //                Handle(model);
    //            }
    //        }
    //    }

    //    public static class WithoutRequest
    //    {
    //        public abstract class WithResult<TResponse> : IBaseEndpoint
    //        {
    //            public HttpContext Context { get; set; }

    //            public ISerializer Serializer { get; set; }
    //            public abstract TResponse Handle();
    //            internal void HandleContext()
    //            {
    //                var obj = Handle();

    //                using (var memStream = new MemoryStream())
    //                {
    //                    Serializer.SerializeSync<TResponse>(memStream, obj);
    //                    memStream.Position = 0;

    //                    Context.Response.ContentType = "application/json";
    //                    Context.Response.ContentLength = memStream.Length;
    //                    memStream.CopyTo(Context.Response.Body);
    //                }
    //            }
    //        }

    //        public abstract class WithoutResult : IBaseEndpoint
    //        {
    //            public HttpContext Context { get; set; }

    //            public ISerializer Serializer { get; set; }
                                
    //            public abstract void Handle();
    //            internal void HandleContext()
    //            {
    //                Handle();
    //            }
    //        }
    //    }
    //}
}

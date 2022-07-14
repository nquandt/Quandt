using Microsoft.AspNetCore.Http;
using Quandt.Abstractions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Quandt.Endpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// </summary>
    public static class BaseEndpointAsync
    {
        public static class WithRequest<TRequest> where TRequest : new()
        {
            public abstract class WithResult<TResponse> : IWithRequestEndpoint<TRequest>, IWithResultEndpoint<TResponse>
            {
                //private static EndpointModelCache<TRequest> _cache = new EndpointModelCache<TRequest>();
                public HttpContext Context { get; set; }
                ISerializerFactory IBaseEndpointAsync.SerializerFactory
                {
                    get
                    {
                        return _serializerFactory;
                    }
                    set
                    {
                        _serializerFactory = value;
                    }
                }
                private ISerializerFactory _serializerFactory;

                EndpointStatus IBaseEndpointAsync.Status
                {
                    get
                    {
                        return _status;
                    }
                }
                object IBaseEndpointAsync.Request
                {
                    get
                    {
                        return _request;
                    }
                    set
                    {
                        _request = (TRequest)value;
                    }
                }
                private TRequest _request;
                object IBaseEndpointAsync.Result
                {
                    get
                    {
                        return _result;
                    }
                    set
                    {
                        _result = (TResponse)value;
                    }
                }
                private TResponse _result;

                private readonly EndpointStatus _status = new EndpointStatus();
                public abstract Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
                async Task IBaseEndpointAsync.HandleContextAsync(CancellationToken ct)
                {
                    try
                    {
                        var result = await HandleAsync(_request, ct);
                        if (result.Success)
                        {
                            _result = result.ResultObject;
                        }
                        else
                        {
                            _status.IsOkay = false;
                            _status.Message = result.Message;
                        }
                    }catch(Exception e)
                    {
                        _status.IsOkay = false;
                        _status.Message = e.Message;
                    }
                }
                //private async Task PrivateHandleAsync(CancellationToken ct)
                //{
                //    var modelResult = await GetRequestModel(Context, _cache, _serializerFactory, ct);

                //    if (!modelResult.Success) { return; }


                //    var obj = await HandleAsync(modelResult.ResultObject, ct);



                //    await ResponseWithModel(Context, obj, _serializerFactory);

                //    return;
                //}
            }

            //public abstract class WithoutResult : IBaseEndpointAsync
            //{
            //    private static EndpointModelCache<TRequest> _cache = new EndpointModelCache<TRequest>();
            //    public HttpContext Context { get; set; }
            //    ISerializerFactory IBaseEndpointAsync.SerializerFactory
            //    {
            //        get
            //        {
            //            return _serializerFactory;
            //        }
            //        set
            //        {
            //            _serializerFactory = value;
            //        }
            //    }
            //    private ISerializerFactory _serializerFactory;
            //    public abstract Task HandleAsync(TRequest request, CancellationToken cancellationToken);
            //    async Task IBaseEndpointAsync.HandleContextAsync(CancellationToken ct)
            //    {
            //        var model = await GetRequestModel(Context, _cache, _serializerFactory, ct);

            //        await HandleAsync(model, ct);
            //    }
            //}

            //public abstract class WithIResult : IBaseEndpointAsync
            //{
            //    private static EndpointModelCache<TRequest> _cache = new EndpointModelCache<TRequest>();
            //    public HttpContext Context { get; set; }
            //    ISerializerFactory IBaseEndpointAsync.SerializerFactory
            //    {
            //        get
            //        {
            //            return _serializerFactory;
            //        }
            //        set
            //        {
            //            _serializerFactory = value;
            //        }
            //    }
            //    private ISerializerFactory _serializerFactory;
            //    public abstract Task<IResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
            //    async Task IBaseEndpointAsync.HandleContextAsync(CancellationToken ct)
            //    {
            //        var model = await GetRequestModel(Context, _cache, _serializerFactory, ct);

            //        var res = await HandleAsync(model, ct);

            //        await res.ExecuteAsync(Context);
            //    }
            //}
        }

        //public static class WithoutRequest
        //{
        //    public abstract class WithResult<TResponse> : AbstractAsyncEndpoint
        //    {
        //        public abstract Task<TResponse> HandleAsync(CancellationToken cancellationToken);
        //        internal override async Task HandleContextAsync(CancellationToken ct)
        //        {
        //            var obj = await HandleAsync(ct);

        //            using (var memStream = new MemoryStream())
        //            {
        //                await Serializer.SerializeAsync<TResponse>(memStream, obj);
        //                memStream.Position = 0;

        //                Context.Response.ContentType = "application/json";
        //                Context.Response.ContentLength = memStream.Length;
        //                await memStream.CopyToAsync(Context.Response.Body);
        //            }
        //        }
        //    }

        //    public abstract class WithoutResult : AbstractAsyncEndpoint
        //    {
        //        public abstract Task HandleAsync(CancellationToken cancellationToken);
        //        internal override async Task HandleContextAsync(CancellationToken ct)
        //        {
        //            await HandleAsync(ct);
        //        }
        //    }

        //    public abstract class WithIResult : AbstractAsyncEndpoint
        //    {
        //        public abstract Task<IResult> HandleAsync(CancellationToken cancellationToken);
        //        internal override async Task HandleContextAsync(CancellationToken ct)
        //        {
        //            var res = await HandleAsync(ct);

        //            await res.ExecuteAsync(Context);
        //        }
        //    }
        //}
    }
}

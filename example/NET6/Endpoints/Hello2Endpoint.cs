using Microsoft.AspNetCore.Mvc;
using Quandt.Endpoints;
using WebProject1.Endpoints.HelloUserEndpointModels;

namespace WebProject1.Endpoints
{
    [Route($"api2/{{{nameof(HelloUserEndpointRequest.User)}}}/{{{nameof(HelloUserEndpointRequest.Action)}}}")]
    public class Hello2Endpoint : BaseEndpointAsync
        .WithRequest<HelloUserEndpointRequest>
        .WithResult<HelloUserEndpointRequest>
    {
        [HttpPost]
        public override Task<Result<HelloUserEndpointRequest>> HandleAsync(HelloUserEndpointRequest request, CancellationToken cancellationToken)
        {
            //var data = Context.GetRouteValue(");
            var result = new Result<HelloUserEndpointRequest>()
            {
                ResultObject = request
            };
            return Task.FromResult(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Quandt.Endpoints;
using WebProject1.Endpoints.HelloUserEndpointModels;

namespace WebProject1.Endpoints
{
    [Route($"api/{{{nameof(HelloUserEndpointRequest.User)}}}/{{{nameof(HelloUserEndpointRequest.Action)}}}")]
    public class HelloUserEndpoint : BaseEndpointAsync
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

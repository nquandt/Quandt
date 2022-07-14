using Microsoft.AspNetCore.Mvc;
using Quandt.Endpoints;
using WebProject1.Endpoints.Hello4EndpointModels;

namespace WebProject1.Endpoints
{
    [Route($"api4/{{{nameof(Hello4EndpointRequest.User)}}}/{{{nameof(Hello4EndpointRequest.Action)}}}")]
    public class Hell42Endpoint : BaseEndpointAsync
        .WithRequest<Hello4EndpointRequest>
        .WithResult<Hello4EndpointRequest>
    {
        [HttpPost]
        public override Task<Result<Hello4EndpointRequest>> HandleAsync(Hello4EndpointRequest request, CancellationToken cancellationToken)
        {
            //var data = Context.GetRouteValue(");
            var result = new Result<Hello4EndpointRequest>()
            {
                ResultObject = request
            };
            return Task.FromResult(result);
        }
    }
}

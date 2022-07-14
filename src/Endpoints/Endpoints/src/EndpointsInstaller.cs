using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Quandt.Endpoints.Abstractions;
using Quandt.Endpoints.PipelineHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Quandt.Endpoints
{
    public class EndpointsInstaller
    {
        private readonly IEnumerable<Type> _asyncEndpoints;
        private readonly IEnumerable<Type> _endpoints;

        private readonly Dictionary<Type, string> _mapping = new Dictionary<Type, string>()
        {
            [typeof(HttpGetAttribute)] = "GET",
            [typeof(HttpPostAttribute)] = "POST",
            [typeof(HttpPutAttribute)] = "PUT",
            [typeof(HttpDeleteAttribute)] = "DELETE"
        };
#if NET6_0
        public EndpointsInstaller(Type[]? types = null)
#elif NETSTANDARD2_0
        public EndpointsInstaller(Type[] types = null)
#endif
        {
            IEnumerable<Assembly> assemblies = types != null
                ? types.Select(x => x.Assembly).Distinct()
                : AppDomain.CurrentDomain.GetAssemblies();

            _asyncEndpoints = GetImplementationsFromAssemblies<IBaseEndpointAsync>(assemblies);
            _endpoints = GetImplementationsFromAssemblies<IBaseEndpoint>(assemblies);
        }

        private IEnumerable<Type> GetImplementationsFromAssemblies<T>(IEnumerable<Assembly> assemblies)
        {
            return assemblies.Select(x => x.GetTypes().Where(x => typeof(T).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)).Aggregate((x, y) => x.Concat(y)).Distinct();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var endpoint in _endpoints)
            {
                services.AddTransient(endpoint);
            }
            foreach (var asyncEndpoint in _asyncEndpoints)
            {
                services.AddTransient(asyncEndpoint);
            }

            services.AddSingleton<ISerializerFactory, DefaultSerializerFactory>();            
            services.AddSingleton<EndpointPipelineRunner>();
        }

        public void Configure(WebApplication app)
        {
            var pipeline = app.Services.GetRequiredService<EndpointPipelineRunner>();

            var _delegateTypeFactory = new DelegateTypeFactory();

            foreach (var endpoint in _asyncEndpoints)
            {                
                pipeline.RegisterPipelinesFor<IWithRequestEndpoint>(endpoint, typeof(WithRequestEndpointPipeline<>));
                pipeline.RegisterPipelinesFor<IBaseEndpointAsync>(endpoint, typeof(DefaultEndpointPipeline));
                pipeline.RegisterPipelinesFor<IWithResultEndpoint>(endpoint, typeof(WithResultEndpointPipeline<>));

                var atts = endpoint.GetCustomAttributes(typeof(RouteAttribute), true)
                    .Select(x => (RouteAttribute)x);
                if (!atts.Any()) { continue; }
                var routeAtt = atts.First()!;


                var method = endpoint.GetMethod("HandleAsync")!;

                var httpMethodsAtts = method.GetCustomAttributes(typeof(HttpMethodAttribute), true)
                    .Select(x => x.GetType());


                var finalMethods = new List<string>();
                foreach (var att in httpMethodsAtts)
                {
                    if (_mapping.ContainsKey(att))
                    {
                        finalMethods.Add(_mapping[att]);
                    }
                }

                var args = endpoint.BaseType?.GenericTypeArguments;
                if (args != null)
                {
                    var generic = typeof(AsyncEndpointDelegate<>);
                    Type[] typeArgs = { endpoint };//, typeof(HttpContext), typeof(CancellationToken) };

                    Type constructed = generic.MakeGenericType(typeArgs);

                    var instance = Activator.CreateInstance(constructed)!;
                    var methodInfo = instance.GetType().GetMethod(nameof(AsyncEndpointDelegate<IBaseEndpointAsync>.ExecuteAsync))!;

                    var del = Delegate.CreateDelegate(_delegateTypeFactory.CreateDelegateType(methodInfo), instance, methodInfo);

                    app.MapMethods(routeAtt.Template, finalMethods, del)
                        .WithName(endpoint.Name);
                    //.Accepts(args.First(), "application/json")
                    //.Produces(200, args.Last(), "application/json");
                }
            }
        }
    }
}

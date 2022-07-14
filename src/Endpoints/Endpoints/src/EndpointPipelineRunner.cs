using Microsoft.Extensions.DependencyInjection;
using Quandt.Endpoints.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Quandt.Endpoints
{
    internal class EndpointPipelineRunner
    {
        private readonly IErrorHandler _errorHandler;
        public EndpointPipelineRunner(IServiceProvider serviceProvider)
        {
            _errorHandler = serviceProvider.GetService<IErrorHandler>() ?? new DefaultErrorHandler(serviceProvider);
            _pipelines = new Dictionary<Type, IHandler[]>();
        }

        private Dictionary<Type, IHandler[]> _pipelines;

        public void RegisterPipelinesFor<T>(Type endpoint, Type pipeline) where T : IBaseEndpointAsync
        {
            if (!endpoint.GetInterfaces().Any(x => x == typeof(T))) { return; }
            //var genericType = pipeline.GetInterfaces().Where(x => x.IsGenericType).Single().GenericTypeArguments[0];

            var pipelines = typeof(IEndpointPipeline).Assembly.GetTypes().Where(x => typeof(IEndpointPipeline).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract);
            Console.WriteLine(typeof(T).Name);

            IEndpointPipeline<T> instance = pipeline.IsGenericType
                ? (IEndpointPipeline<T>)Activator.CreateInstance(pipeline.MakeGenericType(new Type[] { endpoint.GetInterfaces().Where(x => x.IsGenericType && typeof(T).IsAssignableFrom(x)).Single().GenericTypeArguments[0] }))
                : (IEndpointPipeline<T>)Activator.CreateInstance(pipeline);

            RegisterActions(endpoint, instance);
        }

        public void RegisterActions<T>(Type type, IEndpointPipeline<T> pipeline) where T : IBaseEndpointAsync
        {
            if (!_pipelines.ContainsKey(type))
            {
                _pipelines.Add(type, pipeline.Handlers.ToArray());
                return;
            }

            var list = _pipelines[type].ToList();
            foreach (var handler in pipeline.Handlers)
            {
                list.Add(handler);
            }

            _pipelines[type] = list.ToArray();
        }

        public async Task Handle(IBaseEndpointAsync endpoint, CancellationToken ct)
        {
            var pipeline = _pipelines[endpoint.GetType()];

            foreach (var handler in pipeline)
            {
                await handler.Handle(endpoint, ct);
                if (!endpoint.Status.IsOkay)
                {
                    await _errorHandler.Handle(endpoint.Context, endpoint.Status, ct);
                }
            }
        }
    }
}

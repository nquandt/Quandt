using Quandt.Endpoints.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Quandt.Endpoints
{
    internal class EndpointPipeline
    {
        private readonly IErrorHandler _errorHandler;
        public EndpointPipeline(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            _pipelines = new Dictionary<Type, Func<IBaseEndpointAsync, CancellationToken, Task>[]>();
        }

        private Dictionary<Type, Func<IBaseEndpointAsync, CancellationToken, Task>[]> _pipelines;

        public void RegisterPipelinesFor<T>(Type endpoint) where T : IBaseEndpointAsync
        {
            if (!endpoint.GetInterfaces().Any(x => x == typeof(T))) { return; }

            var handlers = typeof(IHandler).Assembly.GetTypes().Where(x => typeof(IHandler).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract);
            Console.WriteLine(typeof(T).Name);
            foreach (var handler in handlers.Where(x => x.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandler<>)).Single().GenericTypeArguments[0] == typeof(T)))
            {
                var inter = endpoint.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IWithRequestEndpoint<>)).Single().GenericTypeArguments[0];

                if (handler.IsGenericType)
                {

                    Type[] typeArgs = { inter };
                    Type constructed = handler.MakeGenericType(typeArgs);

                    var instance = (IHandler<T>)Activator.CreateInstance(constructed);

                    RegisterActions(endpoint, instance);
                }
                else
                {                    
                    var instance = (IHandler<T>)Activator.CreateInstance(handler);

                    RegisterActions(endpoint, instance);
                }
            }
        }

        public void RegisterActions<T>(Type type, IHandler<T> handler) where T : IBaseEndpointAsync
        {
            if (!_pipelines.ContainsKey(type))
            {
                _pipelines.Add(type, handler.Funcs);
                return;
            }

            var list = _pipelines[type].ToList();
            foreach(var func in handler.Funcs)
            {
                list.Add(func);
            }

            _pipelines[type] = list.ToArray();
        }

        public async Task Handle(IBaseEndpointAsync endpoint, CancellationToken ct)
        {
            var pipeline = _pipelines[endpoint.GetType()];

            foreach(var action in pipeline)
            {
                await action(endpoint, ct);
                if (!endpoint.Status.IsOkay)
                {
                    await _errorHandler.Handle(endpoint.Context, endpoint.Status, ct);
                }
            }
        }
    }
}

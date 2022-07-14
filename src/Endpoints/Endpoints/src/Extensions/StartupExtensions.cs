using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quandt.Endpoints;
using System;

namespace Quandt.Endpoints.Extensions
{
    public static class StartupExtensions
    {
#if NET6_0
        private static EndpointsInstaller? _endpointsInstaller;

        public static IServiceCollection AddEndpointsForServices(this IServiceCollection services, Type[]? types = null)
        {
            _endpointsInstaller = new EndpointsInstaller(types);

            _endpointsInstaller.ConfigureServices(services);

            return services;
        }
        public static WebApplication AddEndpointsForApp(this WebApplication app, Type[]? types = null)
        {
            if (_endpointsInstaller != null)
            {
                _endpointsInstaller.Configure(app);
                _endpointsInstaller = null;
            }

            return app;
        }
#elif NETSTANDARD2_0
        private static EndpointsInstaller _endpointsInstaller;

        public static IServiceCollection AddEndpointsForServices(this IServiceCollection services, Type[] types = null)
        {
            _endpointsInstaller = new EndpointsInstaller(types);

            _endpointsInstaller.ConfigureServices(services);

            return services;
        }
        public static IApplicationBuilder AddEndpointsForApp(this IApplicationBuilder app, Type[] types = null)
        {
            if (_endpointsInstaller != null)
            {
                _endpointsInstaller.Configure(app);
                _endpointsInstaller = null;
            }           

            return app;
        }
#endif
    }
}

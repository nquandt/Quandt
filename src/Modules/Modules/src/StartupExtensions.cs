using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Quandt.Modules.Extensions
{
    public static class StartupExtensions
    {
#if NET6_0
    private static ModulesInstaller? _moduleInstaller;

        public static IServiceCollection AddModulesForServices(this IServiceCollection services, params Assembly[] markers)
        {
            _moduleInstaller = new ModulesInstaller(markers);

            _moduleInstaller.AddModules(services);

            return services;
        }
        public static WebApplication AddModulesForServices(this WebApplication app)
        {
            if (_moduleInstaller != null)
            {
                _moduleInstaller.ConfigureModules(app);
                _moduleInstaller = null;
            }

            return app;
        }
#elif NETSTANDARD2_0
        private static ModulesInstaller _moduleInstaller;

        public static IServiceCollection AddModulesForServices(this IServiceCollection services, params Assembly[] markers)
        {
            _moduleInstaller = new ModulesInstaller(markers);

            _moduleInstaller.AddModules(services);

            return services;
        }
        public static IApplicationBuilder ConfigureModulesForApplication(this IApplicationBuilder app)
        {
            if (_moduleInstaller != null)
            {
                _moduleInstaller.ConfigureModules(app);
                _moduleInstaller = null;
            }           

            return app;
        }
#endif
    }
}

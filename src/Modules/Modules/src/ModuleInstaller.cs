using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Quandt.Modules
{
    internal class ModulesInstaller
    {   
        private List<IModule> _modules;

#if NET6_0
        public ModulesInstaller(Type[]? types = null)
#elif NETSTANDARD2_0
        public ModulesInstaller(Type[] types = null)
#endif
        {
            IEnumerable<Assembly> assemblies = types != null  && types.Any()
                ? types.Select(x => x.Assembly).Distinct()
                : AppDomain.CurrentDomain.GetAssemblies();
            
            _modules = new List<IModule>();
            foreach (var assembly in assemblies)
            {
                var modules = assembly
                    .ExportedTypes                   
                   .Where(x => typeof(IModule).IsAssignableFrom(x) && x.IsClass);                

                foreach (var moduleType in modules)
                {
                    var obj = Activator.CreateInstance(moduleType);
                    if (obj == null)
                    {
                        throw new NullReferenceException("Module cannot be null");
                    }
                    var module = (IModule)obj;

                    _modules.Add(module);
                }
            }               
        }

        public IServiceCollection AddModules(IServiceCollection services)
        {
            foreach(var module in _modules)
            {
                module.ConfigureServices(services);
            }            

            return services;
        }

#if NET6_0
        public WebApplication ConfigureModules(WebApplication app)
        {
            foreach (var module in _modules)
            {
                module.Configure(app);
            }

            return app;
        }
#elif NETSTANDARD2_0
        public IApplicationBuilder ConfigureModules(IApplicationBuilder app)
        {
            foreach (var module in _modules)
            {
                module.Configure(app);
            }

            return app;
        }
#endif
    }
}

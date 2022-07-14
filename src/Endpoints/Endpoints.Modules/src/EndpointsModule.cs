using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quandt.Modules;
using Quandt.Endpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Endpoints.Modules
{
    public class EndpointsModule : IModule
    {
        private EndpointsInstaller _endpointsInstaller;

        public EndpointsModule()
        {
            _endpointsInstaller = new EndpointsInstaller();
        }
        public void Configure(WebApplication app)
        {
            _endpointsInstaller.Configure(app);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _endpointsInstaller.ConfigureServices(services);            
        }
    }
}

using Quandt.Abstractions;
using Quandt.Modules;
using WebProject1.Serializers;

namespace Quandt.Example.NET6.Modules
{
    public class SerializerModule : IModule
    {
        public void Configure(WebApplication app)
        {            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISerializer, JsonSerializer>();
            services.AddSingleton<ISerializer, NewtsonJsonSerializer>();
        }
    }
}

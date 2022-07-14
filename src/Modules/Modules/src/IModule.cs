using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Quandt.Modules
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);
#if NET6_0
        void Configure(WebApplication app);
#elif NETSTANDARD2_0
        void Configure(IApplicationBuilder app);
#endif
    }
}
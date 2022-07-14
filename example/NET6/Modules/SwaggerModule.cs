using Quandt.Modules;
using Swashbuckle.AspNetCore.Newtonsoft;

namespace Quandt.Example.NET6.Modules
{
    public class SwaggerModule : IModule
    {
        public void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddSwaggerGenNewtonsoftSupport();
        }
    }
}

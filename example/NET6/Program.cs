using Quandt.Abstractions;
using Quandt.Modules.Extensions;
using WebProject1.Serializers;

var builder = WebApplication.CreateBuilder(args);

var markers = new Type[]
{
    typeof(Program),
    typeof(Quandt.Endpoints.Modules.AssemblyMarker)
};


builder.Services.AddModulesForServices(markers.Select(x => x.Assembly).ToArray());

var app = builder.Build();

app.AddModulesForServices();

app.Run();
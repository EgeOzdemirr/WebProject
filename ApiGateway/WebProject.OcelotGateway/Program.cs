using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer("OcelotAuthenticationScheme", opt =>
{
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.Audience = "ResourceOcelot";
    opt.RequireHttpsMetadata = false;
});

var environmentOcelotFile = $"ocelot.{builder.Environment.EnvironmentName}.json";
var ocelotFile = File.Exists(environmentOcelotFile) ? environmentOcelotFile : "ocelot.json";
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile(ocelotFile).Build();

builder.Services.AddOcelot(configuration);

var app = builder.Build();

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();

using System.Reflection;
using System.Text.Json.Serialization;
using DietMenu.Api.Application;
using DietMenu.Api.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Graylog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Graylog(new GraylogSinkOptions
    {
        HostnameOrAddress = Environment.GetEnvironmentVariable("GRAYLOG_ADDRESS") ?? "localhost",
        Port = 12201,
    })
    .CreateLogger();

try
{
    Log.Information("Starting host...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseSerilog()
        .ConfigureAppConfiguration((_, config) => { config.AddEnvironmentVariables(prefix: "CONFIG_"); })
        ;

    builder.Services
        .AddMediatR(Assembly.GetExecutingAssembly());

    builder.Services
        .AddApplication();

    builder.Services
        .AddInfrastructure(builder.Configuration);

    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    builder.Services
        .AddEndpointsApiExplorer();

    builder.Services
        .AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(s => s.FullName);
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DietMenu WebApi",
                Version = "v1",
            });
        });

    var app = builder.Build();

    app.Use(async (context, next) =>
    {
        context.Request.PathBase = new PathString("/api");
        await next();
    });

    app.UseInfrastructure();

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "DietMenu WebApi v1"));

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Graylog;
using SilentMike.DietMenu.Mailing.Application;
using SilentMike.DietMenu.Mailing.Infrastructure;
using SilentMike.DietMenu.Mailing.WebApi.Filters;

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
        .AddControllers(options =>
        {
            options.Filters.Add<SerilogLoggingActionFilter>();
        })
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
                Title = "DietMenu Email WebApi",
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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "DietMenu Email WebApi v1"));

    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            var request = httpContext.Request;

            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            foreach (var (name, value) in request.Headers)
            {
                diagnosticContext.Set(name, value);
            }

            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            var endpoint = httpContext.GetEndpoint();

            if (endpoint is { })
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
        };
    });

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

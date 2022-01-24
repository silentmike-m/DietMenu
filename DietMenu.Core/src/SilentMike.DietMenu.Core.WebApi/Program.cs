using System.Reflection;
using System.Text.Json.Serialization;
using Serilog;
using SilentMike.DietMenu.Core.Application;
using SilentMike.DietMenu.Core.Infrastructure;
using SilentMike.DietMenu.Core.WebApi.Extensions;
using SilentMike.DietMenu.Core.WebApi.Filters;

var seqAddress = Environment.GetEnvironmentVariable("SEQ_ADDRESS") ?? "http://localhost:5341";

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Seq(seqAddress)
    .CreateLogger();

try
{
    Log.Information("Starting host...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables("CONFIG_");

    builder.Host.UseSerilog((_, lc) =>
        lc.Enrich.WithProperty("AppName", "SilentMike DietMenu")
            .Enrich.WithProperty("Version", "1.0.0")
            .WriteTo.Console()
            .WriteTo.Seq(seqAddress)
    );

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
            options.Filters.Add<ApiExceptionFilterAttribute>();
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    builder.Services
        .AddEndpointsApiExplorer();

    var app = builder.Build();

    app.UseKestrelResponseHandlerMiddleware();

    app.UseInfrastructure();

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
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using SilentMike.DietMenu.Mailing.Application;
using SilentMike.DietMenu.Mailing.Infrastructure;
using SilentMike.DietMenu.Mailing.WebApi;
using SilentMike.DietMenu.Mailing.WebApi.Filters;

const int EXIT_FAILURE = 1;
const int EXIT_SUCCESS = 0;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables("CONFIG_");

builder.Host.UseSerilog((_, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty(nameof(ServiceConstants.ServiceName), ServiceConstants.ServiceName)
    .Enrich.WithProperty(nameof(ServiceConstants.ServiceVersion), ServiceConstants.ServiceVersion));

builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers(options =>
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

var corsOptions = builder.Configuration.GetSection(CorsOptions.SECTION_NAME).Get<CorsOptions>();
corsOptions ??= new CorsOptions();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy",
        policyBuilder => policyBuilder
            .WithOrigins(corsOptions.AllowedOrigins)
            .AllowCredentials()
            .WithHeaders(corsOptions.AllowedHeaders)
            .WithMethods(corsOptions.AllowedMethods));
});

try
{
    Log.Information("Starting host...");

    var app = builder.Build();

    app.UseCors("Policy");

    app.Use(async (context, next) =>
    {
        context.Request.PathBase = new PathString("/takeoff");
        await next();
    });

    app.UseInfrastructure();

    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = EnrichDiagnosticContext;
    });

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    await app.RunAsync();

    return EXIT_SUCCESS;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");

    return EXIT_FAILURE;
}
finally
{
    Log.CloseAndFlush();
}

void EnrichDiagnosticContext(IDiagnosticContext diagnosticContext, HttpContext httpContext)
{
    var request = httpContext.Request;

    diagnosticContext.Set("Host", request.Host);
    diagnosticContext.Set("Protocol", request.Protocol);
    diagnosticContext.Set("Scheme", request.Scheme);

    foreach (var (name, value) in request.Headers /*.Where(kv => kv.Key.Equals("X-Real-IP"))*/)
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
}

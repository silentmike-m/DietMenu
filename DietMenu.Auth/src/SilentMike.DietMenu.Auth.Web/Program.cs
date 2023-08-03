using System.Text.Json.Serialization;
using IdentityServer4;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;
using SilentMike.DietMenu.Auth.Application;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Infrastructure;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.Web;
using SilentMike.DietMenu.Auth.Web.Common.Constants;
using SilentMike.DietMenu.Auth.Web.Filters;
using SilentMike.DietMenu.Auth.Web.Interfaces;
using SilentMike.DietMenu.Auth.Web.Services;

const int EXIT_FAILURE = 1;
const int EXIT_SUCCESS = 0;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("clients.json")
    .AddEnvironmentVariables("CONFIG_")
    ;

builder.Host.UseSerilog((_, configuration) => configuration
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty(nameof(ServiceConstants.ServiceName), ServiceConstants.ServiceName)
    .Enrich.WithProperty(nameof(ServiceConstants.ServiceVersion), ServiceConstants.ServiceVersion));

builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<ICurrentRequestService, CurrentRequestService>();
builder.Services.AddScoped<IHttpContextSignInService, HttpContextSignInService>();
builder.Services.AddSingleton<IIdentityPageUrlService, IdentityPageUrlService>();

builder.Services
    .AddAuthentication()
    .AddInfrastructure()
    ;

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
    {
        policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });

    options.AddPolicy(PolicyNames.SYSTEM, policy =>
    {
        policy.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var corsOptions = builder.Configuration.GetSection(CorsOptions.SECTION_NAME).Get<CorsOptions>();
corsOptions ??= new CorsOptions();

builder.Services
    .AddCors(options => options.AddPolicy("Policy", corsBuilder => corsBuilder
        .WithOrigins(corsOptions.AllowedOrigins)
        .AllowCredentials()
        .WithHeaders(corsOptions.AllowedHeaders)
        .WithMethods(corsOptions.AllowedMethods)
    ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();

try
{
    Log.Information("Starting host...");

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseInfrastructure(builder.Configuration);

    app.UseHttpsRedirection();
    app.UseHttpLogging();

    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = EnrichDiagnosticContext;
    });

    app.UseStaticFiles();

    app.UseRouting();

    app.UseCors("Policy");

    app.UseIdentityServer();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
    });

    await app.RunAsync();

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
}

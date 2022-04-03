using System.Reflection;
using System.Text.Json.Serialization;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;
using SilentMike.DietMenu.Auth.Application;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Infrastructure;
using SilentMike.DietMenu.Auth.Web.Filters;
using SilentMike.DietMenu.Auth.Web.Services;

var seqAddress = Environment.GetEnvironmentVariable("SEQ_ADDRESS") ?? "http://localhost:5341";

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables("CONFIG_")
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Seq(seqAddress)
    .CreateLogger();

try
{
    logger.Information("Starting host...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .AddJsonFile("clients.json")
        .AddEnvironmentVariables("CONFIG_");

    builder.Host.UseSerilog((_, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.WithProperty("AppName", "SilentMike DietMenu Auth")
        .Enrich.WithProperty("Version", "1.0.0")
        .WriteTo.Seq(seqAddress)
    );

#if Linux
builder.Host.UseSystemd();
#endif

    builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

    builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
    builder.Services.AddScoped<ICurrentRequestService, CurrentRequestService>();

    builder.Services.AddAuthentication()
        .AddLocalApi();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
        {
            policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
        });

        options.AddPolicy("System", policy =>
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

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "Policy",
            corsBuilder => corsBuilder
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(
                    "http://localhost",
                    "http://127.0.0.1",
                    "http://localhost:30001",
                    "http://127.0.0.1:30001",
                    "https://localhost:8080",
                    "https://127.0.0.1:8080")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddRazorPages();

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

    app.UseStaticFiles();

    app.UseRouting();

    app.UseCors("Policy");

    app.UseIdentityServer();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapRazorPages();

    await app.RunAsync();
}
catch (Exception exception)
{
    logger.Fatal(exception, "Host terminated unexpectedly.");
}
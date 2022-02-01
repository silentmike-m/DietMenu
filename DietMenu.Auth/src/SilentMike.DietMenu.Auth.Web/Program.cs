using System;
using System.Reflection;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

    builder.Configuration.AddEnvironmentVariables("CONFIG_");

    builder.Host.UseSerilog((ctx, lc) => lc
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

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilterAttribute>();
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddRazorPages();

    var app = builder.Build();

    app.UseMiddleware<SwaggerAuthorizationMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseInfrastructure();

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
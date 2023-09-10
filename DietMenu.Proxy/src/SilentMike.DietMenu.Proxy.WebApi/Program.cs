using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SilentMike.DietMenu.Proxy.Infrastructure;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Interfaces;
using SilentMike.DietMenu.Proxy.Infrastructure.Shared;
using SilentMike.DietMenu.Proxy.WebApi.Middlewares;
using SilentMike.DietMenu.Proxy.WebApi.Models;
using SilentMike.DietMenu.Proxy.WebApi.Services;
using CookieAuthenticationEvents = SilentMike.DietMenu.Proxy.WebApi.Events.CookieAuthenticationEvents;

const int EXIT_FAILURE = 1;
const int EXIT_SUCCESS = 0;
const int TOKEN_LIFETIME_VALIDATION_TOLERANCE_IN_MINUTES = 1;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("reverseproxy.json")
    .AddEnvironmentVariables("CONFIG_");

builder.Host.UseSerilog(
    (_, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithProperty(nameof(ServiceConstants.ServiceName), ServiceConstants.ServiceName)
            .Enrich.WithProperty(nameof(ServiceConstants.ServiceVersion), ServiceConstants.ServiceVersion)
            ;
    }
);

builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);

builder.Services.AddHealthChecks();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<TokenService>();

builder.Services.AddScoped<CookieAuthenticationEvents>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

var corsOptions = builder.Configuration.GetSection(CorsOptions.SECTION_NAME).Get<CorsOptions>();
corsOptions ??= new CorsOptions();

builder.Services
    .AddCors(
        options => options.AddPolicy(
            "Default", corsBuilder => corsBuilder
                .WithOrigins(corsOptions.AllowedOrigins)
                .AllowCredentials()
                .WithHeaders(corsOptions.AllowedHeaders)
                .WithMethods(corsOptions.AllowedMethods)
        )
    );

builder.Services.Configure<CookiePolicyOptions>(options => options.Secure = CookieSecurePolicy.Always);

var identityServerOptions = builder.Configuration.GetSection(IdentityServer4Options.SECTION_NAME).Get<IdentityServer4Options>();
identityServerOptions ??= new IdentityServer4Options();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddAuthorization(
        options =>
            options.AddPolicy(
                "uiPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                }
            )
    )
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddJwtBearer(
        JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = identityServerOptions.Authority;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = true,
                LifetimeValidator = (before, expires, _, _) =>
                {
                    var now = DateTime.UtcNow;

                    before ??= now;
                    expires ??= now;

                    before = before.Value.AddMinutes(-TOKEN_LIFETIME_VALIDATION_TOLERANCE_IN_MINUTES);
                    expires = expires.Value.AddMinutes(TOKEN_LIFETIME_VALIDATION_TOLERANCE_IN_MINUTES);

                    return before <= now && now <= expires;
                },
            };
        }
    )
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => options.EventsType = typeof(CookieAuthenticationEvents))
    .AddOpenIdConnect(
        OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = identityServerOptions.Authority;

            options.ClientId = identityServerOptions.ClientId;

            options.ClientSecret = identityServerOptions.ClientSecret;

            options.GetClaimsFromUserInfoEndpoint = true;

            options.RequireHttpsMetadata = false;

            options.ResponseType = OpenIdConnectResponseType.Code;

            options.SaveTokens = true;

            options.Scope.Clear();
            options.Scope.Add("user");
            options.Scope.Add(IdentityServerConstants.StandardScopes.OfflineAccess);
            options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
            options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);

            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role",
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
            };

            options.UsePkce = true;

            options.Events = new OpenIdConnectEvents
            {
                OnAuthenticationFailed = context =>
                {
                    context.Response.Redirect("/logout");

                    return Task.CompletedTask;
                },
            };
        }
    )
    ;

builder.Services.AddHttpContextAccessor();

try
{
    Log.Information("Starting host...");

    var app = builder.Build();

    app.UseSession();

    app.UseCors("Default");

    app.UseHttpsRedirection();

    app.UseHttpLogging();
    app.UseHealthChecks("/health");

    app.UseCookiePolicy();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(
        endpoints =>
        {
            endpoints.MapGet(
                "/getToken", async (context) =>
                {
                    var service = context.RequestServices.GetRequiredService<IIdentityServerService>();

                    var token = service.GetAccessToken(context.Session.Id);

                    if (string.IsNullOrEmpty(token))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }
                    else
                    {
                        await context.Response.WriteAsync(token, CancellationToken.None);
                    }
                }
            );

            endpoints.MapGet(
                "/keepAlive", [Authorize] async (context) =>
                {
                    var service = context.RequestServices.GetRequiredService<IIdentityServerService>();
                    await service.RefreshTokenAsync(context.Session.Id, force: true);
                }
            );

            endpoints.MapGet(
                "/logout", async context =>
                {
                    var service = context.RequestServices.GetRequiredService<IIdentityServerService>();
                    service.ClearToken(context.Session.Id);

                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                }
            );

            endpoints.MapGet(
                "/logged-out", async context =>
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await context.SignOutAsync();
                }
            );

            endpoints.MapReverseProxy(
                proxyPipeline =>
                {
                    proxyPipeline.Use(
                        (context, next) =>
                        {
                            if (context.Request.Path.StartsWithSegments("/api") || context.Request.Path.StartsWithSegments("/takeoff") || context.Request.Path.StartsWithSegments("/file"))
                            {
                                var cacheControlValues = new[]
                                {
                                    "no-cache", "no-store",
                                };

                                context.Response.Headers.Add("Cache-Control", new StringValues(cacheControlValues));
                            }

                            return next();
                        }
                    );

                    proxyPipeline.UseProxyPipelineIdentityTokenMiddleware();
                }
            );
        }
    );

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

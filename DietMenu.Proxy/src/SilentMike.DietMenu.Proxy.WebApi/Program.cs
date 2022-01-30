using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SilentMike.DietMenu.Proxy.WebApi;
using Yarp.ReverseProxy.Transforms;

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
        .AddJsonFile("reverseproxy.json")
        .AddEnvironmentVariables("CONFIG_");

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.WithProperty("AppName", "SilentMike DietMenu Proxy")
        .Enrich.WithProperty("Version", "1.0.0")        
        .WriteTo.Seq(seqAddress)
    );

    builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);
    builder.Services.AddHealthChecks();

    builder.Services
        .AddCors(options => options.AddPolicy(name: "Default", corsBuilder => corsBuilder
            .WithOrigins(
                "http://localhost",
                "https://silentmike.dietmenu.pl")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

    builder.Services.Configure<CookiePolicyOptions>(options => options.Secure = CookieSecurePolicy.Always);

    var identityServerOptions = builder.Configuration.GetSection(IdentityServer4Options.SectionName).Get<IdentityServer4Options>();

    builder.Services.AddAuthorization(options => options.AddPolicy("uiPolicy", policy => policy.RequireAuthenticatedUser()));

    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
        .AddTransforms(builderContext =>
        {
            if (!string.IsNullOrEmpty(builderContext.Route.AuthorizationPolicy))
            {
                builderContext.AddRequestTransform(async transformContext =>
                {
                    var token = await transformContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
                    transformContext.ProxyRequest.Headers.Add("Authorization", $"Bearer {token}");
                });
            }
        })
        ;

    builder.Services
        .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = identityServerOptions.Authority;
            options.RequireHttpsMetadata = false;

            options.ClientId = "bff";
            options.ClientSecret = "secret";
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.UsePkce = true;

            options.Scope.Clear();
            options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
            options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
            options.Scope.Add("user");

            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                NameClaimType = "name",
                RoleClaimType = "role",
            };
        })
        ;

    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    app.UseCors("Default");

    app.UseHttpsRedirection();

    app.UseHttpLogging();
    app.UseHealthChecks("/health");

    app.UseCookiePolicy();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/logout", async context =>
        {
            await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignOutAsync();
            context.Response.Redirect("/");
        });

        endpoints.MapReverseProxy();
    });

    await app.RunAsync();
}
catch (Exception exception)
{
    logger.Fatal(exception, "Host terminated unexpectedly.");
}
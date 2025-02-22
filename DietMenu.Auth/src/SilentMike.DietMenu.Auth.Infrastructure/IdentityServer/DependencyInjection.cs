﻿namespace SilentMike.DietMenu.Auth.Infrastructure.IdentityServer;

using System.Reflection;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.IdentityServer.Services;

internal static class DependencyInjection
{
    private const string IDENTITY_SERVER_CLIENTS_CONFIGURATION_SECTION = "IdentityServer:Clients";

    public static void AddIdentityServer4(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var migrationsAssembly = Assembly.GetExecutingAssembly().GetName().Name;

        services.Configure<IdentityServerOptions>(configuration.GetSection(IdentityServerOptions.SECTION_NAME));

        var identityServerOptions = configuration.GetSection(IdentityServerOptions.SECTION_NAME).Get<IdentityServerOptions>();
        identityServerOptions ??= new IdentityServerOptions();

        services
            .AddIdentityServer(
                options => options.IssuerUri = identityServerOptions.IssuerUri
            )
            .AddOperationalStore(
                options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(
                        connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly)
                    );
                }
            )
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<User>()
            .AddInMemoryApiResources(Setup.GetApiResources())
            .AddInMemoryApiScopes(Setup.GetApiScopes())
            .AddInMemoryClients(configuration.GetSection(IDENTITY_SERVER_CLIENTS_CONFIGURATION_SECTION))
            .AddInMemoryIdentityResources(Setup.GetIdentityResources())
            .AddInMemoryPersistedGrants()
            .AddProfileService<ProfileService>()
            ;

        services.AddTransient<IProfileService, ProfileService>();
    }

    internal static AuthenticationBuilder AddIdentityServer4Authentication(this AuthenticationBuilder builder)
    {
        builder.AddLocalApi();

        return builder;
    }
}

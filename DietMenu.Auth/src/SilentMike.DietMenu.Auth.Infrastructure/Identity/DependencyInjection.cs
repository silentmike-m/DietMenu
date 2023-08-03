namespace SilentMike.DietMenu.Auth.Infrastructure.Identity;

using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Domain.Services;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Services;

internal static class DependencyInjection
{
    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetSection(IdentityOptions.SECTION_NAME));

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var identityOptions = configuration.GetSection(IdentityOptions.SECTION_NAME).Get<IdentityOptions>();
        identityOptions ??= new IdentityOptions();

        services.AddDbContext<IDietMenuDbContext, DietMenuDbContext>(options => options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = identityOptions.RequireConfirmedAccount)
            .AddEntityFrameworkStores<DietMenuDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IFamilyRepository, FamilyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ISystemMigrationService, SystemMigrationService>();
    }

    public static void UseIdentity(this IApplicationBuilder _, DietMenuDbContext context, PersistedGrantDbContext grantContext, ISystemMigrationService systemMigrationService)
    {
        context.Database.Migrate();

        grantContext.Database.Migrate();

        systemMigrationService.MigrateSystemAsync().Wait();
    }
}

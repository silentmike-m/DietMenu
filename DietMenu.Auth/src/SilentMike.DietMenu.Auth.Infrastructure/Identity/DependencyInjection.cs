namespace SilentMike.DietMenu.Auth.Infrastructure.Identity;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Domain.Services;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
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
    }

    public static void UseIdentity(this IApplicationBuilder _, IConfiguration configuration, DietMenuDbContext context, UserManager<User> userManager)
    {
        context.Database.Migrate();

        var identityOptions = configuration.GetSection(IdentityOptions.SECTION_NAME).Get<IdentityOptions>();
        identityOptions ??= new IdentityOptions();

        AddSystemUser(identityOptions, userManager);
    }

    private static void AddSystemUser(IdentityOptions options, UserManager<User> userManager)
    {
        var user = userManager.FindByNameAsync(options.SystemUserEmail).Result;

        if (user is not null)
        {
            return;
        }

        var userId = Guid.NewGuid();

        var family = new Family
        {
            Id = userId,
            Name = "System",
        };

        user = new User
        {
            Id = userId.ToString(),
            Email = options.SystemUserEmail,
            EmailConfirmed = true,
            Family = family,
            FamilyId = family.Id,
            FamilyKey = family.Key,
            FirstName = "Saruman",
            LastName = "White",
            UserName = options.SystemUserEmail,
        };

        _ = userManager.CreateAsync(user, options.SystemUserPassword).Result;
    }
}

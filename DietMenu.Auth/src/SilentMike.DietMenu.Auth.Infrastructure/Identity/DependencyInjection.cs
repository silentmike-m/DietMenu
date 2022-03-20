namespace SilentMike.DietMenu.Auth.Infrastructure.Identity;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal static class DependencyInjection
{
    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetSection(IdentityOptions.SectionName));

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DietMenuDbContext>(options => options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<DietMenuUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<DietMenuDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void UseIdentity(
        this IApplicationBuilder _,
        IConfiguration configuration,
        DietMenuDbContext context,
        UserManager<DietMenuUser> userManager)
    {
        context.Database.Migrate();

        var identityOptions = configuration.GetSection(IdentityOptions.SectionName).Get<IdentityOptions>();

        AddSystemUser(identityOptions, userManager);
    }

    private static void AddSystemUser(IdentityOptions options, UserManager<DietMenuUser> userManager)
    {
        var user = userManager.FindByNameAsync(options.SystemUserEmail).Result;

        if (user is not null)
        {
            return;
        }

        var userId = Guid.NewGuid();

        var family = new DietMenuFamily
        {
            Id = userId,
            Name = "System",
        };

        user = new DietMenuUser
        {
            Id = userId.ToString(),
            Email = options.SystemUserEmail,
            EmailConfirmed = true,
            Family = family,
            FamilyId = family.Id,
            FirstName = "Saruman",
            LastName = "White",
            UserName = options.SystemUserEmail,
        };

        _ = userManager.CreateAsync(user, options.SystemUserPassword).Result;
    }
}

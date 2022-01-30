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

        services.AddDbContext<IDietMenuDbContext, DietMenuDbContext>(options => options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<DietMenuUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<DietMenuDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void UseIdentity(this IApplicationBuilder _, DietMenuDbContext context, UserManager<DietMenuUser> userManager)
    {
        context.Database.Migrate();

        AddSystemUser(userManager);
    }

    private static void AddSystemUser(UserManager<DietMenuUser> userManager)
    {
        var user = userManager.FindByNameAsync("saruman@dietmenu.pl").Result;

        if (user is not null)
        {
            return;
        }

        var family = new DietMenuFamily
        {
            Id = Guid.NewGuid(),
            Name = "System",
        };

        user = new DietMenuUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "saruman@dietmenu.pl",
            EmailConfirmed = true,
            Family = family,
            FamilyId = family.Id,
            FirstName = "Saruman",
            LastName = "White",
            UserName = "saruman@dietmenu.pl",
        };

        _ = userManager.CreateAsync(user, "P@ssw0rd").Result;
    }
}

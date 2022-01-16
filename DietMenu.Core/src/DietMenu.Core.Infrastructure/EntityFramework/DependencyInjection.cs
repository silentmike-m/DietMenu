namespace DietMenu.Core.Infrastructure.EntityFramework;

using DietMenu.Core.Domain.Repositories;
using DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using DietMenu.Core.Infrastructure.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static class DependencyInjection
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddScoped<ApplicationDbContext>();

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options => options.UseSqlServer(defaultConnectionString));

        services.AddScoped<IFamilyRepository, FamilyRepository>();
    }
}

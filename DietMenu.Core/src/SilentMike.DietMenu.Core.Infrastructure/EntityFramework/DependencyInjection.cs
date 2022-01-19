namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Domain.Repositories;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ApplicationDbContext>();

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options => options.UseSqlServer(defaultConnectionString));

        services.AddScoped<IFamilyRepository, FamilyRepository>();
    }
}

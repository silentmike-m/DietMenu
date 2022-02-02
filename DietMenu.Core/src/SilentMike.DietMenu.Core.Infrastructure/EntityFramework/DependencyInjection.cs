namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Domain.Repositories;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<DietMenuDbContext>();

        services.AddDbContext<IDietMenuDbContext, DietMenuDbContext>(options => options.UseSqlServer(defaultConnectionString));

        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IIngredientTypeRepository, IngredientTypeRepository>();
        services.AddScoped<IMealTypeRepository, MealTypeRepository>();
        services.AddScoped<IFamilyRepository, FamilyRepository>();

        services.AddScoped<IIngredientTypeReadService, IngredientTypeReadService>();
        services.AddScoped<IMealTypeReadService, MealTypeReadService>();
    }

    public static void UseEntityFramework(this IApplicationBuilder _, DietMenuDbContext context)
    {
        context.Database.Migrate();
    }
}

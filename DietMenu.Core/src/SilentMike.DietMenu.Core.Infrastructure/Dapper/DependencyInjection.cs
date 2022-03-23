namespace SilentMike.DietMenu.Core.Infrastructure.Dapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;
using SilentMike.DietMenu.Core.Infrastructure.Dapper.Services;

internal static class DependencyInjection
{
    public static void AddDapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DapperOptions>(configuration.GetSection(DapperOptions.SectionName));

        services.AddSingleton<IMealTypeReadService, MealTypeReadService>();
    }
}

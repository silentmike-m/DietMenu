namespace SilentMike.DietMenu.Auth.Infrastructure.AutoMapper;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

internal static class DependencyInjection
{
    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}

﻿namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Domain.Services;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.Infrastructure.Families.Interfaces;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDbContext<IDietMenuDbContext, DietMenuDbContext>(options => options.UseSqlServer(defaultConnectionString));

        services.AddScoped<IFamilyMigrationService, FamilyMigrationService>();
        services.AddScoped<IFamilyRepository, FamilyRepository>();
    }

    public static void UseEntityFramework(this IApplicationBuilder _, DietMenuDbContext context)
    {
        context.Database.Migrate();
    }
}

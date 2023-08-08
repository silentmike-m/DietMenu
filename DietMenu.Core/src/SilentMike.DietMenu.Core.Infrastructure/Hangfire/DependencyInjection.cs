namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire;

using System.Diagnostics.CodeAnalysis;
using global::Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.Filters;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    private const string INIT_DATABASE_QUERY = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = @DatabaseName) CREATE DATABASE [@DatabaseName]";

    public static void AddHangfire(this IServiceCollection services, IConfiguration configuration, string hangFireServerName)
    {
        var connectionString = configuration.GetConnectionString("HangfireConnection");
        connectionString ??= string.Empty;

        ProvideHangfireDatabase(connectionString);

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(connectionString);
        });

        services.AddHangfireServer(options =>
        {
            options.ServerName = hangFireServerName;
            options.WorkerCount = 1;
        });
    }

    public static void UseHangfire(this IApplicationBuilder app, string hangFireServerName)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() },
        });

        ClearHangfireServers(hangFireServerName);
    }

    private static void ClearHangfireServers(string hangFireServerName)
    {
        var serversToDelete = JobStorage.Current
            .GetMonitoringApi()
            .Servers()
            .Where(i => !i.Name.Contains(hangFireServerName, StringComparison.InvariantCultureIgnoreCase));

        foreach (var server in serversToDelete)
        {
            JobStorage.Current.GetConnection().RemoveServer(server.Name);
        }
    }

    private static void ProvideHangfireDatabase(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var catalog = connectionStringBuilder.InitialCatalog;
        connectionStringBuilder.InitialCatalog = string.Empty;

        using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{catalog}') CREATE DATABASE [{catalog}]";
        command.ExecuteNonQuery();
    }
}

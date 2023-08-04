namespace SilentMike.DietMenu.Core.IntegrationTests.Helpers;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

internal static class WebApplicationFactoryExtensions
{
    private const string DB_NAME = "InMemoryDbForTesting";
    private const string HANGFIRE_CONNECTION_STRING = "Server=localhost,1433;Database=hangfire-tests;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=true;TrustServerCertificate=true";

    public static WebApplicationFactory<T> WithFakeDbContext<T>(this WebApplicationFactory<T> factory, string dbName = DB_NAME)
        where T : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<DietMenuDbContext>));

                services.Remove(dbContextDescriptor);

                services.AddDbContext<DietMenuDbContext>((_, options) =>
                {
                    options.UseInMemoryDatabase(dbName);
                });
            });
        });
    }

    public static WebApplicationFactory<T> WithTestHangfire<T>(this WebApplicationFactory<T> factory)
        where T : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:HangfireConnection", HANGFIRE_CONNECTION_STRING);
        });
    }
}

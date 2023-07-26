namespace SilentMike.DietMenu.Auth.IntegrationTests.Helpers;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;

internal static class WebApplicationFactoryExtensions
{
    private const string DB_NAME = "InMemoryDbForTesting";

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
}

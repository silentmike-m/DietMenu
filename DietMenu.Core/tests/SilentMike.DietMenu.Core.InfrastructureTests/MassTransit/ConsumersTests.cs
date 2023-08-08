namespace SilentMike.DietMenu.Core.InfrastructureTests.MassTransit;

using System.Reflection;
using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit;

[TestClass]
public sealed class ConsumersTests
{
    private const string INFRASTRUCTURE_ASSEMBLY_NAME = "SilentMike.DietMenu.Core.Infrastructure";

    [TestMethod]
    public void Should_RegisterAllMassTransitConsumers()
    {
        //GIVEN
        var types = Assembly.Load(new AssemblyName(INFRASTRUCTURE_ASSEMBLY_NAME)).GetTypes().ToList();

        var consumerTypes = types
            .Where(i => typeof(IConsumer).IsAssignableFrom(i) || typeof(IConsumer<object>).IsAssignableFrom(i))
            .ToList();

        var configuration = new ConfigurationBuilder().Build();

        var services = new ServiceCollection()
            .AddMassTransit(configuration);

        //THEN
        foreach (var consumerType in consumerTypes)
        {
            services.Should()
                .ContainSingle(service => service.ServiceType == consumerType, $"Missing registration of consumer {consumerType}");
        }
    }
}

namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit;

using System.Reflection;
using FluentAssertions;
using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit;

[TestClass]
public sealed class ConsumersTests
{
    private const string INFRASTRUCTURE_ASSEMBLY_NAME = "SilentMike.DietMenu.Auth.Infrastructure";

    [TestMethod]
    public void Should_RegisterAllMassTransitConsumers()
    {
        //GIVEN
        var types = Assembly.Load(new AssemblyName(INFRASTRUCTURE_ASSEMBLY_NAME)).GetTypes().ToList();

        var consumerTypes = types
            .Where(i => typeof(IConsumer).IsAssignableFrom(i) || typeof(IConsumer<object>).IsAssignableFrom(i))
            .ToList();

        var configuration = new ConfigurationBuilder().Build();

        var services = new ServiceCollection().AddMassTransit(configuration);

        //THEN
        foreach (var consumerType in consumerTypes)
        {
            services.Should()
                .ContainSingle(service => service.ServiceType == consumerType, $"Missing registration of consumer {consumerType}")
                ;
        }
    }
}

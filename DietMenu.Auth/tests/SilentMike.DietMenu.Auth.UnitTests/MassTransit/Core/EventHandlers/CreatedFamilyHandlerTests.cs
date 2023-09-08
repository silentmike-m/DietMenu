namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit.Core.EventHandlers;

using global::MassTransit;
using global::MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Auth.Application.Families.Events;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Core.EventHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Core.Models;
using SilentMike.DietMenu.Shared.Core.Interfaces;

[TestClass]
public sealed class CreatedFamilyHandlerTests
{
    private readonly NullLogger<CreatedFamilyHandler> logger = new();

    [TestMethod]
    public async Task Should_Send_Created_Family_Message()
    {
        //GIVEN
        var notification = new CreatedFamily
        {
            Id = Guid.NewGuid(),
        };

        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness()
            .BuildServiceProvider(true);

        var harness = provider.GetTestHarness();
        harness.TestTimeout = TimeSpan.FromSeconds(1);
        await harness.Start();

        var handler = new CreatedFamilyHandler(harness.Bus, this.logger);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        (await harness.Published.Any<ICreatedFamilyMessage>()).Should()
            .BeTrue()
            ;

        var expectedMessage = new CreatedFamilyMessage
        {
            Id = notification.Id,
        };

        var messages = harness.Published
                .Select<ICreatedFamilyMessage>()
                .ToList()
            ;

        messages.Should()
            .HaveCount(1)
            ;

        messages[0].Context.Message.Should()
            .BeEquivalentTo(expectedMessage)
            ;
    }
}

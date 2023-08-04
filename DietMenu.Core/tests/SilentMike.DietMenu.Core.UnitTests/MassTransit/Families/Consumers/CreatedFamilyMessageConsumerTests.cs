namespace SilentMike.DietMenu.Core.UnitTests.MassTransit.Families.Consumers;

using global::MassTransit;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Families.Consumers;
using SilentMike.DietMenu.Shared.Core.Interfaces;

[TestClass]
public sealed class CreatedFamilyMessageConsumerTests
{
    private readonly NullLogger<CreatedFamilyMessageConsumer> logger = new();
    private readonly Mock<ISender> mediator = new();

    [TestMethod]
    public async Task Should_Send_Create_Family_Request()
    {
        //GIVEN
        var familyId = Guid.NewGuid();

        CreateFamily? createFamilyRequest = null;

        this.mediator
            .Setup(service => service.Send(It.IsAny<CreateFamily>(), It.IsAny<CancellationToken>()))
            .Callback<CreateFamily, CancellationToken>((request, _) => createFamilyRequest = request);

        var message = Mock.Of<ICreatedFamilyMessage>(message => message.Id == familyId);

        var context = new Mock<ConsumeContext<ICreatedFamilyMessage>> { };

        context
            .Setup(service => service.Message)
            .Returns(message);

        var consumer = new CreatedFamilyMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(context.Object);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<CreateFamily>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new CreateFamily
        {
            Id = familyId,
        };

        createFamilyRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }
}

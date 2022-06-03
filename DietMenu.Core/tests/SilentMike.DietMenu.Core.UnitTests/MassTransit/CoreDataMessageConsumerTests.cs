namespace SilentMike.DietMenu.Core.UnitTests.MassTransit;

using global::MassTransit;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Infrastructure.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Shared.Core.Interfaces;
using SilentMike.DietMenu.Shared.Core.Models;

[TestClass]
public sealed class CoreDataMessageConsumerTests
{
    private readonly Mock<ConsumeContext<ICoreDataMessage>> context = new();
    private readonly NullLogger<CoreDataMessageConsumer> logger = new();
    private readonly Mock<IMediator> mediator = new();

    [TestMethod]
    public async Task ShouldThrowFormatExceptionWhenInvalidPayloadType()
    {
        //GIVEN
        var message = Mock.Of<ICoreDataMessage>(dataMessage => dataMessage.PayloadType == typeof(string).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        var consumer = new CoreDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        Func<Task> action = async () => await consumer.Consume(this.context.Object);

        //THEN
        await action.Should()
                .ThrowAsync<FormatException>()
            ;
    }

    [TestMethod]
    public async Task ShouldSendCreateFamilyRequest()
    {
        CreateFamily? createFamilyRequest = null;

        //GIVEN
        this.mediator.Setup(i => i.Send(It.IsAny<CreateFamily>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((request, _) => createFamilyRequest = request as CreateFamily);

        var payload = new CreatedFamilyPayload
        {
            FamilyId = Guid.NewGuid(),
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<ICoreDataMessage>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(CreatedFamilyPayload).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        var consumer = new CoreDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        this.mediator.Verify(i => i.Send(It.IsAny<CreateFamily>(), It.IsAny<CancellationToken>()), Times.Once);

        createFamilyRequest.Should()
            .NotBeNull()
            ;
        createFamilyRequest!.Id.Should()
            .Be(payload.FamilyId)
            ;
    }
}

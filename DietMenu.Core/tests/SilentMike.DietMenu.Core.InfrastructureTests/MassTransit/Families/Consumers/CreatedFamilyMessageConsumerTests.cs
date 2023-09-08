namespace SilentMike.DietMenu.Core.InfrastructureTests.MassTransit.Families.Consumers;

using global::MassTransit;
using SilentMike.DietMenu.Core.Application.Families.Commands;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Families.Consumers;
using SilentMike.DietMenu.Shared.Core.Interfaces;

[TestClass]
public sealed class CreatedFamilyMessageConsumerTests
{
    private readonly NullLogger<CreatedFamilyMessageConsumer> logger = new();
    private readonly ISender mediator = Substitute.For<ISender>();

    [TestMethod]
    public async Task Should_Send_Create_Family_Request()
    {
        //GIVEN
        var familyId = Guid.NewGuid();

        ImportFamilyData? importFamilyDataRequest = null;

        await this.mediator
            .Send(Arg.Do<ImportFamilyData>(request => importFamilyDataRequest = request), Arg.Any<CancellationToken>());

        var message = Substitute.For<ICreatedFamilyMessage>();
        message.Id.Returns(familyId);

        var context = Substitute.For<ConsumeContext<ICreatedFamilyMessage>>();
        context.Message.Returns(message);

        var consumer = new CreatedFamilyMessageConsumer(this.logger, this.mediator);

        //WHEN
        await consumer.Consume(context);

        //THEN
        _ = this.mediator.Received(1).Send(Arg.Any<ImportFamilyData>(), Arg.Any<CancellationToken>());

        var expectedRequest = new ImportFamilyData(familyId);

        importFamilyDataRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }
}

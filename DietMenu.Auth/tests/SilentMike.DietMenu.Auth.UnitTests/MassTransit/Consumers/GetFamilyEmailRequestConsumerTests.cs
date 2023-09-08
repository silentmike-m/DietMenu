namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit.Consumers;

using global::MassTransit;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

[TestClass]
public sealed class GetFamilyEmailRequestConsumerTests
{
    private readonly NullLogger<GetFamilyEmailRequestConsumer> logger = new();
    private readonly ISender mediator = Substitute.For<ISender>();

    [TestMethod]
    public async Task Should_Return_Family_Email()
    {
        //GIVEN
        const string familyEmail = "family@domain.com";
        var familyId = Guid.NewGuid();

        GetFamilyEmail? getFamilyOwnerRequest = null;

        IGetFamilyEmailResponse? getFamilyOwnerEmailResponse = null;

        this.mediator
            .Send(Arg.Do<GetFamilyEmail>(request => getFamilyOwnerRequest = request), Arg.Any<CancellationToken>())
            .Returns(familyEmail);

        var message = Substitute.For<IGetFamilyEmailRequest>();
        message.FamilyId.Returns(familyId);

        var context = Substitute.For<ConsumeContext<IGetFamilyEmailRequest>>();

        context
            .Message
            .Returns(message);

        await context
            .RespondAsync(Arg.Do<IGetFamilyEmailResponse>(response => getFamilyOwnerEmailResponse = response));

        var consumer = new GetFamilyEmailRequestConsumer(this.logger, this.mediator);

        //WHEN
        await consumer.Consume(context);

        //THEN
        _ = this.mediator.Received(1).Send(Arg.Any<GetFamilyEmail>(), Arg.Any<CancellationToken>());

        var expectedRequest = new GetFamilyEmail
        {
            FamilyId = familyId,
        };

        getFamilyOwnerRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;

        var expectedResponse = new GetFamilyEmailResponse
        {
            Email = familyEmail,
            FamilyId = familyId,
        };

        getFamilyOwnerEmailResponse.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResponse)
            ;
    }
}

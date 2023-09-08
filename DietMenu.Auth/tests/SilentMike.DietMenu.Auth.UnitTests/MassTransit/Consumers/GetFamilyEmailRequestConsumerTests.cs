namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit.Consumers;

using FluentAssertions;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

[TestClass]
public sealed class GetFamilyEmailRequestConsumerTests
{
    private readonly NullLogger<GetFamilyEmailRequestConsumer> logger = new();
    private readonly Mock<ISender> mediator = new();

    [TestMethod]
    public async Task Should_Return_Family_Email()
    {
        //GIVEN
        const string familyEmail = "family@domain.com";
        var familyId = Guid.NewGuid();

        GetFamilyEmail? getFamilyOwnerRequest = null;

        IGetFamilyEmailResponse? getFamilyOwnerEmailResponse = null;

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetFamilyEmail>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(familyEmail)
            .Callback<IRequest<string>, CancellationToken>((request, _) => getFamilyOwnerRequest = request as GetFamilyEmail);

        var message = Mock.Of<IGetFamilyEmailRequest>(message => message.FamilyId == familyId);

        var context = new Mock<ConsumeContext<IGetFamilyEmailRequest>> { };

        context
            .Setup(service => service.Message)
            .Returns(message);

        context
            .Setup(service => service.RespondAsync(It.IsAny<IGetFamilyEmailResponse>()))
            .Callback<IGetFamilyEmailResponse>((response) => getFamilyOwnerEmailResponse = response);

        var consumer = new GetFamilyEmailRequestConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(context.Object);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<GetFamilyEmail>(), It.IsAny<CancellationToken>()), Times.Once);

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

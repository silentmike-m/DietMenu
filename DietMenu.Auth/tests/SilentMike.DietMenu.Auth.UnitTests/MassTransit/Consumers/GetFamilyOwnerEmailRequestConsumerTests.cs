namespace SilentMike.DietMenu.Auth.UnitTests.MassTransit.Consumers;

using FluentAssertions;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Application.Families.ViewModels;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.Identity.Interfaces;

[TestClass]
public sealed class GetFamilyOwnerEmailRequestConsumerTests
{
    private readonly NullLogger<GetFamilyOwnerEmailRequestConsumer> logger = new();
    private readonly Mock<ISender> mediator = new();

    [TestMethod]
    public async Task Should_Return_Family_Owner()
    {
        //GIVEN
        var familyId = Guid.NewGuid();

        var familyOwner = new FamilyOwner
        {
            Email = "owner@domain.com",
            UserId = Guid.NewGuid(),
        };

        GetFamilyOwner? getFamilyOwnerRequest = null;

        IGetFamilyOwnerEmailResponse? getFamilyOwnerEmailResponse = null;

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetFamilyOwner>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(familyOwner)
            .Callback<IRequest<FamilyOwner>, CancellationToken>((request, _) => getFamilyOwnerRequest = request as GetFamilyOwner);

        var message = Mock.Of<IGetFamilyOwnerEmailRequest>(message => message.FamilyId == familyId);

        var context = new Mock<ConsumeContext<IGetFamilyOwnerEmailRequest>> { };

        context
            .Setup(service => service.Message)
            .Returns(message);

        context
            .Setup(service => service.RespondAsync(It.IsAny<IGetFamilyOwnerEmailResponse>()))
            .Callback<IGetFamilyOwnerEmailResponse>((response) => getFamilyOwnerEmailResponse = response);

        var consumer = new GetFamilyOwnerEmailRequestConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(context.Object);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<GetFamilyOwner>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new GetFamilyOwner
        {
            FamilyId = familyId,
        };

        getFamilyOwnerRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;

        var expectedResponse = new GetFamilyOwnerEmailResponse
        {
            Email = familyOwner.Email,
            FamilyId = familyId,
            UserId = familyOwner.UserId,
        };

        getFamilyOwnerEmailResponse.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResponse)
            ;
    }
}

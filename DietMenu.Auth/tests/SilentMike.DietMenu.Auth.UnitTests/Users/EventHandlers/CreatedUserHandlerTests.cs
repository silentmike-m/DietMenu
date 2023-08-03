namespace SilentMike.DietMenu.Auth.UnitTests.Users.EventHandlers;

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.Users.EventHandlers;

[TestClass]
public sealed class CreatedUserHandlerTests
{
    private readonly NullLogger<CreatedUserHandler> logger = new();
    private readonly Mock<ISender> mediator = new();

    [TestMethod]
    public async Task Should_Not_Sent_Generate_Email_Confirmation_Token_When_Not_Require_Account_Confirmation()
    {
        //GIVEN
        var options = new IdentityOptions
        {
            RequireConfirmedAccount = false,
        };

        var identityOptions = Options.Create(options);

        var notification = new CreatedUser
        {
            Id = Guid.NewGuid(),
        };

        var handler = new CreatedUserHandler(identityOptions, this.logger, this.mediator.Object);

        //THEN
        await handler.Handle(notification, CancellationToken.None);

        this.mediator.Verify(service => service.Send(It.IsAny<GenerateEmailConfirmationToken>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task Should_Sent_Generate_Email_Confirmation_Token_When_Require_Account_Confirmation()
    {
        //GIVEN
        GenerateEmailConfirmationToken? generateEmailConfirmationTokenRequest = null;

        this.mediator
            .Setup(service => service.Send(It.IsAny<GenerateEmailConfirmationToken>(), It.IsAny<CancellationToken>()))
            .Callback<GenerateEmailConfirmationToken, CancellationToken>((request, _) => generateEmailConfirmationTokenRequest = request);

        var options = new IdentityOptions
        {
            RequireConfirmedAccount = true,
        };

        var identityOptions = Options.Create(options);

        var notification = new CreatedUser
        {
            Id = Guid.NewGuid(),
        };

        var handler = new CreatedUserHandler(identityOptions, this.logger, this.mediator.Object);

        //THEN
        await handler.Handle(notification, CancellationToken.None);

        this.mediator.Verify(service => service.Send(It.IsAny<GenerateEmailConfirmationToken>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new GenerateEmailConfirmationToken
        {
            Email = notification.Email,
        };

        generateEmailConfirmationTokenRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }
}

namespace SilentMike.DietMenu.Auth.UnitTests.Users;

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.EventHandlers;

[TestClass]
public sealed class CreatedUserHandlerTests
{
    [TestMethod]
    public async Task ShouldSendEmailConfirmationCommand()
    {
        SendUserConfirmation? sendUserConfirmation = null;

        //GIVEN
        var logger = new NullLogger<CreatedUserHandler>();
        var mediator = new Mock<IMediator>();

        mediator.Setup(i => i.Send(It.IsAny<SendUserConfirmation>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((command, _) => sendUserConfirmation = command as SendUserConfirmation);

        var notification = new CreatedUser
        {
            Email = "user@domain.com",
        };

        var notificationHandler = new CreatedUserHandler(logger, mediator.Object);

        //WHEN
        await notificationHandler.Handle(notification, CancellationToken.None);

        //THEN
        mediator.Verify(i => i.Send(It.IsAny<SendUserConfirmation>(), It.IsAny<CancellationToken>()), Times.Once);

        sendUserConfirmation.Should()
            .NotBeNull()
            ;
        sendUserConfirmation!.Email.Should()
            .Be(notification.Email)
            ;
    }
}

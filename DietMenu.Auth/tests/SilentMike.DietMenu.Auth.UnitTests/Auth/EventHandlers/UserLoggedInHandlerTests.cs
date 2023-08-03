namespace SilentMike.DietMenu.Auth.UnitTests.Auth.EventHandlers;

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Auth.Events;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Auth.EventHandlers;

[TestClass]
public sealed class UserLoggedInHandlerTests
{
    private readonly NullLogger<UserLoggedInHandler> logger = new();
    private readonly Mock<ISender> mediator = new();

    [TestMethod]
    public async Task Should_Sent_Update_User_Last_Login_Date()
    {
        //GIVEN
        UpdateUserLastLoginDate? updateUserLastLoginDateRequest = null;

        this.mediator
            .Setup(service => service.Send(It.IsAny<UpdateUserLastLoginDate>(), It.IsAny<CancellationToken>()))
            .Callback<UpdateUserLastLoginDate, CancellationToken>((request, _) => updateUserLastLoginDateRequest = request);

        var notification = new UserLoggedIn
        {
            UserId = Guid.NewGuid(),
        };

        var handler = new UserLoggedInHandler(this.logger, this.mediator.Object);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<UpdateUserLastLoginDate>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new UpdateUserLastLoginDate
        {
            UserId = notification.UserId,
        };

        updateUserLastLoginDateRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }
}

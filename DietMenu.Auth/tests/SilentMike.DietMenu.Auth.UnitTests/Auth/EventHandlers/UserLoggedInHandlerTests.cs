namespace SilentMike.DietMenu.Auth.UnitTests.Auth.EventHandlers;

using SilentMike.DietMenu.Auth.Application.Auth.Events;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Auth.EventHandlers;

[TestClass]
public sealed class UserLoggedInHandlerTests
{
    private readonly NullLogger<UserLoggedInHandler> logger = new();
    private readonly ISender mediator = Substitute.For<ISender>();

    [TestMethod]
    public async Task Should_Sent_Update_User_Last_Login_Date()
    {
        //GIVEN
        UpdateUserLastLoginDate? updateUserLastLoginDateRequest = null;

        await this.mediator
            .Send(Arg.Do<UpdateUserLastLoginDate>(request => updateUserLastLoginDateRequest = request), Arg.Any<CancellationToken>());

        var notification = new UserLoggedIn
        {
            UserId = Guid.NewGuid(),
        };

        var handler = new UserLoggedInHandler(this.logger, this.mediator);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        _ = this.mediator.Received(1).Send(Arg.Any<UpdateUserLastLoginDate>(), Arg.Any<CancellationToken>());

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

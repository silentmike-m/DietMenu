namespace SilentMike.DietMenu.Auth.UnitTests.Users.CommandHandlers;

using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class UpdateUserLastLoginDateHandlerTests
{
    private static readonly DateTime DATE = new(year: 2023, month: 8, day: 1, hour: 11, minute: 47, second: 15, millisecond: 16, DateTimeKind.Utc);

    private readonly IDateTimeService dateTimeService = Substitute.For<IDateTimeService>();
    private readonly NullLogger<UpdateUserLastLoginDateHandler> logger = new();

    public UpdateUserLastLoginDateHandlerTests()
    {
        this.dateTimeService
            .GetNow()
            .Returns(DATE);
    }

    [TestMethod]
    public async Task Should_Throw_User_Not_Found_Exception_When_Missing_User_With_Id()
    {
        //GIVEN
        var userId = Guid.NewGuid();

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .FindByIdAsync(userId.ToString())
                .Returns(new User()))
            .Build();

        var request = new UpdateUserLastLoginDate
        {
            UserId = Guid.NewGuid(),
        };

        var handler = new UpdateUserLastLoginDateHandler(this.dateTimeService, this.logger, userManager);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
                .Where(exception => exception.Id == request.UserId)
            ;
    }

    [TestMethod]
    public async Task Should_Update_User_Last_Login_Date()
    {
        //GIVEN
        User? updatedUser = null;

        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId.ToString(),
            LastLogin = new DateTime(year: 2023, month: 1, day: 1, hour: 10, minute: 00, second: 15),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .FindByIdAsync(userId.ToString())
                .Returns(user)
            )
            .With(
                userManager => userManager
                    .UpdateAsync(Arg.Do<User>(userToUpdate => updatedUser = userToUpdate))
            )
            .Build();

        var request = new UpdateUserLastLoginDate
        {
            UserId = userId,
        };

        var handler = new UpdateUserLastLoginDateHandler(this.dateTimeService, this.logger, userManager);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        _ = userManager.Received(1).UpdateAsync(Arg.Any<User>());

        updatedUser.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(user, options => options.Excluding(resultUser => resultUser.LastLogin))
            ;

        var expectedDate = new DateTime(DATE.Year, DATE.Month, DATE.Day, DATE.Hour, DATE.Minute, DATE.Second, DATE.Kind);

        updatedUser!.LastLogin.Should()
            .Be(expectedDate)
            ;
    }
}

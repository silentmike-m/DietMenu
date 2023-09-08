namespace SilentMike.DietMenu.Auth.UnitTests.Users.QueryHandlers;

using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Application.Users.ViewModels;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Users.QueryHandlers;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class GetUserStatusHandlerTests
{
    private readonly NullLogger<GetUserStatusHandler> logger = new();

    [TestMethod]
    public async Task Should_Return_User_Status_With_Confirmed_Email()
    {
        //GIVEN
        var identityOptions = Options.Create(new IdentityOptions
        {
            RequireConfirmedAccount = true,
        });

        var user = new User
        {
            Email = "user@domain.com",
            EmailConfirmed = true,
            LockoutEnabled = false,
            LockoutEnd = DateTime.Now,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .FindByEmailAsync(user.Email)
                .Returns(user)
            )
            .Build();

        var request = new GetUserStatus
        {
            Email = user.Email,
        };

        var handler = new GetUserStatusHandler(this.logger, identityOptions, userManager);

        //GIVEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        var expectedResult = new UserStatus
        {
            IsEmailConfirmed = true,
            IsLockedOut = false,
        };

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Return_User_Status_With_Confirmed_Email_When_Email_Confirmation_Is_Not_Required()
    {
        //GIVEN
        var identityOptions = Options.Create(new IdentityOptions
        {
            RequireConfirmedAccount = false,
        });

        var user = new User
        {
            Email = "user@domain.com",
            EmailConfirmed = false,
            LockoutEnabled = true,
            LockoutEnd = null,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .FindByEmailAsync(user.Email)
                .Returns(user)
            )
            .Build();

        var request = new GetUserStatus
        {
            Email = user.Email,
        };

        var handler = new GetUserStatusHandler(this.logger, identityOptions, userManager);

        //GIVEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        var expectedResult = new UserStatus
        {
            IsEmailConfirmed = true,
            IsLockedOut = false,
        };

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Return_User_Status_With_Not_Confirmed_Email()
    {
        //GIVEN
        var identityOptions = Options.Create(new IdentityOptions
        {
            RequireConfirmedAccount = true,
        });

        var user = new User
        {
            Email = "user@domain.com",
            EmailConfirmed = false,
            LockoutEnabled = true,
            LockoutEnd = DateTimeOffset.Now,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .FindByEmailAsync(user.Email)
                .Returns(user)
            )
            .Build();

        var request = new GetUserStatus
        {
            Email = user.Email,
        };

        var handler = new GetUserStatusHandler(this.logger, identityOptions, userManager);

        //GIVEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        var expectedResult = new UserStatus
        {
            IsEmailConfirmed = user.EmailConfirmed,
            IsLockedOut = user.LockoutEnabled,
        };

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_User_Not_Found_Exception_When_Missing_User()
    {
        //GIVEN
        var identityOptions = Options.Create(new IdentityOptions());

        var userManager = new FakeUserManagerBuilder()
            .Build();

        var request = new GetUserStatus
        {
            Email = "user@domain.com",
        };

        var handler = new GetUserStatusHandler(this.logger, identityOptions, userManager);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }
}

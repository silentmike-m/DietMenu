namespace SilentMike.DietMenu.Auth.UnitTests.Auth.CommandHandlers;

using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Auth.Commands;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Auth.CommandHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Auth;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class PasswordSignInUserHandlerTests
{
    private const string EMAIL = "user@domain.com";
    private const string PASSWORD = "pssword";
    private const bool REMEMBER = false;

    private readonly NullLogger<PasswordSignInUserHandler> logger = new();

    [TestMethod]
    public async Task Should_Sign_In_User()
    {
        //GIVEN
        var signInResult = SignInResult.Success;

        var user = new User
        {
            Email = EMAIL,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(EMAIL))
                .ReturnsAsync(user)
            )
            .Build();

        var signInManager = new FakeSignInManagerBuilder(userManager.Object)
            .With(manager => manager
                .Setup(service => service.PasswordSignInAsync(EMAIL, PASSWORD, REMEMBER, false))
                .ReturnsAsync(signInResult)
            )
            .Build();

        var request = new PasswordSignInUser
        {
            Email = EMAIL,
            Password = PASSWORD,
            Remember = REMEMBER,
        };

        var handler = new PasswordSignInUserHandler(this.logger, signInManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Invalid_Login_Attempt_When_Login_Is_Unsuccessful()
    {
        //GIVEN
        var signInResult = SignInResult.Failed;

        var user = new User
        {
            Email = EMAIL,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(EMAIL))
                .ReturnsAsync(user)
            )
            .Build();

        var signInManager = new FakeSignInManagerBuilder(userManager.Object)
            .With(manager => manager
                .Setup(service => service.PasswordSignInAsync(EMAIL, PASSWORD, REMEMBER, false))
                .ReturnsAsync(signInResult)
            )
            .Build();

        var request = new PasswordSignInUser
        {
            Email = EMAIL,
            Password = PASSWORD,
            Remember = REMEMBER,
        };

        var handler = new PasswordSignInUserHandler(this.logger, signInManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<InvalidLoginAttemptException>()
                .Where(exception => exception.Code == Infrastructure.Common.Constants.ErrorCodes.INVALID_LOGIN_ATTEMPT)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_User_Not_Found_Exception_When_Missing_User()
    {
        //GIVEN
        var signInResult = SignInResult.Failed;

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .Setup(service => service.FindByEmailAsync(EMAIL))
                .Returns(Task.FromResult((User?)null)!)
            )
            .Build();

        var signInManager = new FakeSignInManagerBuilder(userManager.Object)
            .With(manager => manager
                .Setup(service => service.PasswordSignInAsync(EMAIL, PASSWORD, REMEMBER, false))
                .ReturnsAsync(signInResult)
            )
            .Build();

        var request = new PasswordSignInUser
        {
            Email = EMAIL,
            Password = PASSWORD,
            Remember = REMEMBER,
        };

        var handler = new PasswordSignInUserHandler(this.logger, signInManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }
}

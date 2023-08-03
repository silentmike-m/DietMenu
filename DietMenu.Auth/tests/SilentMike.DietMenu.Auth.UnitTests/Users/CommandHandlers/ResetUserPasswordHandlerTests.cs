namespace SilentMike.DietMenu.Auth.UnitTests.Users.CommandHandlers;

using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class ResetUserPasswordHandlerTests
{
    private const string NEW_PASSWORD = "new_password";
    private const string RESET_PASSWORD_TOKEN = "reset_password_token";
    private const string USER_EMAIL = "user@domain.com";
    private const string USER_ID = "aa4dbe6f-9548-4e51-80d8-ebd933aa7c6c";

    private readonly NullLogger<ResetUserPasswordHandler> logger = new();

    [TestMethod]
    public async Task Should_Reset_User_Password()
    {
        var user = new User
        {
            Email = USER_EMAIL,
            Id = USER_ID,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(USER_EMAIL))
                .ReturnsAsync(user)
            )
            .With(manager => manager
                .Setup(service => service.ResetPasswordAsync(It.IsAny<User>(), RESET_PASSWORD_TOKEN, NEW_PASSWORD))
                .ReturnsAsync(IdentityResult.Success)
            )
            .Build();

        var request = new ResetUserPassword
        {
            Email = USER_EMAIL,
            Password = NEW_PASSWORD,
            Token = RESET_PASSWORD_TOKEN,
        };

        var handler = new ResetUserPasswordHandler(this.logger, userManager.Object);

//WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Reset_User_Password_Exception_When_Reset_Password_Fails()
    {
        //GIVEN
        var identityError = new IdentityError
        {
            Code = "test",
            Description = "test",
        };

        var identityResult = IdentityResult.Failed(identityError);

        var user = new User
        {
            Email = USER_EMAIL,
            Id = USER_ID,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(USER_EMAIL))
                .ReturnsAsync(user)
            )
            .With(manager => manager
                .Setup(service => service.ResetPasswordAsync(It.IsAny<User>(), RESET_PASSWORD_TOKEN, NEW_PASSWORD))
                .ReturnsAsync(identityResult)
            )
            .Build();

        var request = new ResetUserPassword
        {
            Email = USER_EMAIL,
            Password = NEW_PASSWORD,
            Token = RESET_PASSWORD_TOKEN,
        };

        var handler = new ResetUserPasswordHandler(this.logger, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ResetUserPasswordException>()
                .Where(exception => exception.Code == Infrastructure.Common.Constants.ErrorCodes.RESET_USER_PASSWORD_ERROR)
                .WithMessage($"*{identityError.Description}*")
            ;
    }

    [TestMethod]
    public async Task Should_Throw_User_Not_Found_Exception_When_Missing_User()
    {
        //GIVEN
        var user = new User
        {
            Email = USER_EMAIL,
            Id = USER_ID,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(USER_EMAIL))
                .ReturnsAsync(user)
            )
            .Build();

        var request = new ResetUserPassword
        {
            Email = "fake@domain.com",
        };

        var handler = new ResetUserPasswordHandler(this.logger, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(i => i.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }
}

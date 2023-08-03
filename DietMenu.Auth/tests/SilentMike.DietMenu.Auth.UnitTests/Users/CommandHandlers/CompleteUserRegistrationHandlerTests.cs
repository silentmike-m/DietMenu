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
public sealed class CompleteUserRegistrationHandlerTests
{
    private const string CONFIRM_EMAIL_TOKEN = "confirm_email_token";
    private const string NEW_PASSWORD = "new_password";
    private const string RESET_PASSWORD_TOKEN = "reset_password_token";
    private static readonly Guid USER_ID = Guid.NewGuid();

    private readonly NullLogger<CompleteUserRegistrationHandler> logger = new();

    [TestMethod]
    public async Task Should_Complete_User_Registration()
    {
        var user = new User
        {
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByIdAsync(user.Id))
                .ReturnsAsync(user)
            )
            .With(manager => manager
                .Setup(service => service.ConfirmEmailAsync(It.IsAny<User>(), CONFIRM_EMAIL_TOKEN))
                .ReturnsAsync(IdentityResult.Success)
            )
            .With(manager => manager
                .Setup(service => service.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(RESET_PASSWORD_TOKEN)
            )
            .With(manager => manager
                .Setup(service => service.ResetPasswordAsync(It.IsAny<User>(), RESET_PASSWORD_TOKEN, NEW_PASSWORD))
                .ReturnsAsync(IdentityResult.Success)
            )
            .Build();

        var request = new CompleteUserRegistration
        {
            Id = USER_ID,
            Password = NEW_PASSWORD,
            Token = CONFIRM_EMAIL_TOKEN,
        };

        var handler = new CompleteUserRegistrationHandler(this.logger, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Confirm_User_Email_Exception_When_Confirm_Email_Fails()
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
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByIdAsync(user.Id))
                .ReturnsAsync(user)
            )
            .With(manager => manager
                .Setup(service => service.ConfirmEmailAsync(It.IsAny<User>(), CONFIRM_EMAIL_TOKEN))
                .ReturnsAsync(identityResult)
            )
            .Build();

        var request = new CompleteUserRegistration
        {
            Id = USER_ID,
            Token = CONFIRM_EMAIL_TOKEN,
        };

        var handler = new CompleteUserRegistrationHandler(this.logger, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ConfirmUserEmailException>()
                .Where(exception => exception.Code == Infrastructure.Common.Constants.ErrorCodes.CONFIRM_USER_EMAIL_ERROR)
                .WithMessage($"*{identityError.Description}*")
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
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByIdAsync(user.Id))
                .ReturnsAsync(user)
            )
            .With(manager => manager
                .Setup(service => service.ConfirmEmailAsync(It.IsAny<User>(), CONFIRM_EMAIL_TOKEN))
                .ReturnsAsync(IdentityResult.Success)
            )
            .With(manager => manager
                .Setup(service => service.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(RESET_PASSWORD_TOKEN)
            )
            .With(manager => manager
                .Setup(service => service.ResetPasswordAsync(It.IsAny<User>(), RESET_PASSWORD_TOKEN, NEW_PASSWORD))
                .ReturnsAsync(identityResult)
            )
            .Build();

        var request = new CompleteUserRegistration
        {
            Id = USER_ID,
            Password = NEW_PASSWORD,
            Token = CONFIRM_EMAIL_TOKEN,
        };

        var handler = new CompleteUserRegistrationHandler(this.logger, userManager.Object);

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
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByIdAsync(user.Id))
                .ReturnsAsync(user)
            )
            .Build();

        var request = new CompleteUserRegistration
        {
            Id = Guid.NewGuid(),
        };

        var handler = new CompleteUserRegistrationHandler(this.logger, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(i => i.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }
}

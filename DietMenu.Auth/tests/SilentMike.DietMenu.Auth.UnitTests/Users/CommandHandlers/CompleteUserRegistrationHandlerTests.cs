namespace SilentMike.DietMenu.Auth.UnitTests.Users.CommandHandlers;

using Microsoft.AspNetCore.Identity;
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
                .FindByIdAsync(user.Id)
                .Returns(user)
            )
            .With(manager => manager
                .ConfirmEmailAsync(Arg.Any<User>(), CONFIRM_EMAIL_TOKEN)
                .Returns(IdentityResult.Success)
            )
            .With(manager => manager
                .GeneratePasswordResetTokenAsync(Arg.Any<User>())
                .Returns(RESET_PASSWORD_TOKEN)
            )
            .With(manager => manager
                .ResetPasswordAsync(Arg.Any<User>(), RESET_PASSWORD_TOKEN, NEW_PASSWORD)
                .Returns(IdentityResult.Success)
            )
            .Build();

        var request = new CompleteUserRegistration
        {
            Id = USER_ID,
            Password = NEW_PASSWORD,
            Token = CONFIRM_EMAIL_TOKEN,
        };

        var handler = new CompleteUserRegistrationHandler(this.logger, userManager);

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
                .FindByIdAsync(user.Id)
                .Returns(user)
            )
            .With(manager => manager
                .ConfirmEmailAsync(Arg.Any<User>(), CONFIRM_EMAIL_TOKEN)
                .Returns(identityResult)
            )
            .Build();

        var request = new CompleteUserRegistration
        {
            Id = USER_ID,
            Token = CONFIRM_EMAIL_TOKEN,
        };

        var handler = new CompleteUserRegistrationHandler(this.logger, userManager);

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
                .FindByIdAsync(user.Id)
                .Returns(user)
            )
            .With(manager => manager
                .ConfirmEmailAsync(Arg.Any<User>(), CONFIRM_EMAIL_TOKEN)
                .Returns(IdentityResult.Success)
            )
            .With(manager => manager
                .GeneratePasswordResetTokenAsync(Arg.Any<User>())
                .Returns(RESET_PASSWORD_TOKEN)
            )
            .With(manager => manager
                .ResetPasswordAsync(Arg.Any<User>(), RESET_PASSWORD_TOKEN, NEW_PASSWORD)
                .Returns(identityResult)
            )
            .Build();

        var request = new CompleteUserRegistration
        {
            Id = USER_ID,
            Password = NEW_PASSWORD,
            Token = CONFIRM_EMAIL_TOKEN,
        };

        var handler = new CompleteUserRegistrationHandler(this.logger, userManager);

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
                .FindByIdAsync(user.Id)
                .Returns(user)
            )
            .Build();

        var request = new CompleteUserRegistration
        {
            Id = Guid.NewGuid(),
        };

        var handler = new CompleteUserRegistrationHandler(this.logger, userManager);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(i => i.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }
}

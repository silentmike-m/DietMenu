namespace SilentMike.DietMenu.Auth.UnitTests.Users.CommandHandlers;

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Users.CommandHandlers;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class GenerateResetPasswordTokenHandlerTests
{
    private const string RESET_PASSWORD_TOKEN = "reset_password_token";
    private const string USER_EMAIL = "user@domain.com";
    private const string USER_ID = "aa4dbe6f-9548-4e51-80d8-ebd933aa7c6c";

    private readonly NullLogger<GenerateResetPasswordTokenHandler> logger = new();
    private readonly Mock<IPublisher> mediator = new();

    [TestMethod]
    public async Task Should_Generate_Reset_Password_Token()
    {
        //GIVEN
        GeneratedResetPasswordToken? generatedResetPasswordTokenNotification = null;

        this.mediator
            .Setup(service => service.Publish(It.IsAny<GeneratedResetPasswordToken>(), It.IsAny<CancellationToken>()))
            .Callback<GeneratedResetPasswordToken, CancellationToken>((notification, _) => generatedResetPasswordTokenNotification = notification);

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
                .Setup(service => service.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(RESET_PASSWORD_TOKEN)
            )
            .Build();

        var request = new GenerateResetPasswordToken
        {
            Email = USER_EMAIL,
        };

        var handler = new GenerateResetPasswordTokenHandler(this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<GeneratedResetPasswordToken>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new GeneratedResetPasswordToken
        {
            Email = user.Email,
            Id = new Guid(user.Id),
            Token = RESET_PASSWORD_TOKEN,
        };

        generatedResetPasswordTokenNotification.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
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

        var request = new GenerateResetPasswordToken
        {
            Email = "fake@domain.com",
        };

        var handler = new GenerateResetPasswordTokenHandler(this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(i => i.Code == ErrorCodes.USER_NOT_FOUND)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<GeneratedResetPasswordToken>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

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
public sealed class GenerateEmailConfirmationTokenHandlerTests
{
    private static readonly Guid USER_ID = Guid.NewGuid();

    private readonly NullLogger<GenerateEmailConfirmationTokenHandler> logger = new();
    private readonly Mock<IPublisher> mediator = new();

    [TestMethod]
    public async Task Should_Generate_Token()
    {
        //GIVEN
        GeneratedEmailConfirmationToken? generatedEmailConfirmationTokenNotification = null;

        this.mediator
            .Setup(service => service.Publish(It.IsAny<GeneratedEmailConfirmationToken>(), It.IsAny<CancellationToken>()))
            .Callback<GeneratedEmailConfirmationToken, CancellationToken>((notification, _) => generatedEmailConfirmationTokenNotification = notification);

        const string token = "email_confirmation_token";

        var user = new User
        {
            Email = "user@domain.com",
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(user.Email))
                .ReturnsAsync(user)
            )
            .With(manager => manager
                .Setup(service => service.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(token)
            )
            .Build();

        var request = new GenerateEmailConfirmationToken
        {
            Email = user.Email,
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Publish(It.IsAny<GeneratedEmailConfirmationToken>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new GeneratedEmailConfirmationToken
        {
            Email = user.Email,
            Id = USER_ID,
            Token = token,
        };

        generatedEmailConfirmationTokenNotification.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Not_Throw_Exception_When_Token_Is_Null()
    {
        //GIVEN
        var user = new User
        {
            Email = "user@domain.com",
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(user.Email))
                .ReturnsAsync(user)
            )
            .Build();

        var request = new GenerateEmailConfirmationToken
        {
            Email = user.Email,
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task Should_Throw_User_Not_Found_Exception_When_Missing_User()
    {
        //GIVEN
        var user = new User
        {
            Email = "user@domain.com",
            Id = USER_ID.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(user.Email))
                .ReturnsAsync(user)
            )
            .Build();

        var request = new GenerateEmailConfirmationToken
        {
            Email = "fake@domain.com",
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

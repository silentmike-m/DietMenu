namespace SilentMike.DietMenu.Auth.UnitTests.Users.CommandHandlers;

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.CommandHandlers;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Services;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class GenerateEmailConfirmationTokenHandlerTests
{
    private readonly NullLogger<GenerateEmailConfirmationTokenHandler> logger = new();
    private readonly Mock<IPublisher> mediator = new();
    private readonly Mock<IUserService> userService = new();

    [TestMethod]
    public async Task Should_Generate_Token()
    {
        //GIVEN
        GeneratedEmailConfirmationToken? generatedEmailConfirmationTokenNotification = null;

        this.mediator
            .Setup(service => service.Publish(It.IsAny<GeneratedEmailConfirmationToken>(), It.IsAny<CancellationToken>()))
            .Callback<GeneratedEmailConfirmationToken, CancellationToken>((notification, _) => generatedEmailConfirmationTokenNotification = notification);

        const string token = "email_confirmation_token";
        var user = UserEntityFactory.Create(Guid.NewGuid(), Guid.NewGuid());

        this.userService
            .Setup(service => service.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        this.userService
            .Setup(service => service.GenerateEmailConfirmationTokenAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(token);

        var request = new GenerateEmailConfirmationToken
        {
            Id = user.Id,
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator.Object, this.userService.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Publish(It.IsAny<GeneratedEmailConfirmationToken>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new GeneratedEmailConfirmationToken
        {
            Email = user.Email,
            Id = user.Id,
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
        var user = new UserEntity("user@domain.com", Guid.NewGuid(), "John", "Wick", Guid.NewGuid());

        this.userService
            .Setup(service => service.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        this.userService
            .Setup(service => service.GenerateEmailConfirmationTokenAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        var request = new GenerateEmailConfirmationToken
        {
            Id = Guid.NewGuid(),
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator.Object, this.userService.Object);

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
        this.userService
            .Setup(service => service.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserEntity?)null);

        var request = new GenerateEmailConfirmationToken
        {
            Id = Guid.NewGuid(),
        };

        var handler = new GenerateEmailConfirmationTokenHandler(this.logger, this.mediator.Object, this.userService.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Id == request.Id)
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

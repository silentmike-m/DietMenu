namespace SilentMike.DietMenu.Auth.UnitTests.Users;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.UnitTests.Services;

[TestClass]
public sealed class ResetPasswordRequestHandlerTests
{
    private readonly Mock<IMediator> mediator = new();

    [TestMethod]
    public async Task ShouldNotThrowExceptionWhenUserNotFound()
    {
        //GIVEN
        var logger = new Mock<ILogger<ResetPasswordRequestHandler>>();

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .Build();

        var command = new ResetPasswordRequest();

        var commandHandler = new ResetPasswordRequestHandler(logger.Object, this.mediator.Object, userManager.Object);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        logger.Verify(m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), Times.Once)
            ;

        this.mediator.Verify(i => i.Publish(It.IsAny<GeneratedResetPasswordToken>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task ShouldNotThrowExceptionWhenEmailIsNotConfirmed()
    {
        //GIVEN
        var logger = new Mock<ILogger<ResetPasswordRequestHandler>>();

        var user = new DietMenuUser
        {
            Email = "user@domain.com",
            EmailConfirmed = false,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user)))
            .With(i => i.Setup(m => m.IsEmailConfirmedAsync(It.IsAny<DietMenuUser>()))
                .Returns(Task.FromResult(false)))
            .Build();

        var command = new ResetPasswordRequest();

        var commandHandler = new ResetPasswordRequestHandler(logger.Object, this.mediator.Object, userManager.Object);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        logger.Verify(m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), Times.Once)
            ;

        this.mediator.Verify(i => i.Publish(It.IsAny<GeneratedResetPasswordToken>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task ShouldGenerateResetPasswordToken()
    {
        GeneratedResetPasswordToken? generatedResetPasswordTokenNotification = null;

        //GIVEN
        var logger = new NullLogger<ResetPasswordRequestHandler>();

        var user = new DietMenuUser
        {
            Email = "user@domain.com",
            EmailConfirmed = true,
        };

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user)))
            .With(i => i.Setup(m => m.IsEmailConfirmedAsync(It.IsAny<DietMenuUser>()))
                .Returns(Task.FromResult(true)))
            .Build();

        this.mediator.Setup(i => i.Publish(It.IsAny<GeneratedResetPasswordToken>(), It.IsAny<CancellationToken>()))
            .Callback<GeneratedResetPasswordToken, CancellationToken>((notification, _) =>
                generatedResetPasswordTokenNotification = notification);

        var command = new ResetPasswordRequest();

        var commandHandler = new ResetPasswordRequestHandler(logger, this.mediator.Object, userManager.Object);

        //WHEN
        _ = await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<GeneratedResetPasswordToken>(), It.IsAny<CancellationToken>()), Times.Once);

        generatedResetPasswordTokenNotification.Should()
            .NotBeNull()
            ;
        generatedResetPasswordTokenNotification!.Email.Should()
            .Be(user.Email)
            ;
    }
}


namespace SilentMike.DietMenu.Auth.UnitTests.Users;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.UnitTests.Services;

[TestClass]
public sealed class ResetPasswordHandlerTests
{
    [TestMethod]
    public async Task ShouldNotThrowExceptionWhenUserNotFound()
    {
        //GIVEN
        var logger = new Mock<ILogger<ResetPasswordHandler>>();

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                .Build();

        var command = new ResetPassword();
        var commandHandler = new ResetPasswordHandler(logger.Object, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;

        logger.Verify(m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), Times.Once)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowResetPasswordExceptionWhenIdentityError()
    {
        //GIVEN
        var identityError = new IdentityError()
        {
            Code = "test",
            Description = "test",
        };
        var identityResult = IdentityResult.Failed(identityError);

        var logger = new NullLogger<ResetPasswordHandler>();

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new DietMenuUser())))
            .With(i => i.Setup(m => m.ResetPasswordAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(identityResult)))
            .Build();

        var command = new ResetPassword();
        var commandHandler = new ResetPasswordHandler(logger, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ResetPasswordException>()
                .Where(i => i.Code == ErrorCodes.RESET_PASSWORD)
            ;
    }
}

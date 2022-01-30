namespace SilentMike.DietMenu.Auth.UnitTests.Users;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
public sealed class ConfirmUserHandlerTests
{
    private readonly Guid userId = Guid.NewGuid();
    private readonly NullLogger<ConfirmUserHandler> logger = new();

    [TestMethod]
    public async Task ShouldThrowUserNotFoundWhenInvalidId()
    {
        //GIVEN
        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .Build();

        var command = new ConfirmUser();

        var commandHandler = new ConfirmUserHandler(this.logger, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(i => i.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowArgumentExceptionWhenInvalidToken()
    {
        //GIVEN
        var user = new DietMenuUser
        {
            Id = this.userId.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user)))
            .With(i => i.Setup(m => m.ConfirmEmailAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed())))
            .Build();

        var command = new ConfirmUser
        {
            Id = this.userId,
        };

        var commandHandler = new ConfirmUserHandler(this.logger, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ArgumentException>()
            ;
    }

    [TestMethod]
    public async Task ShouldCompleteUserRegistration()
    {
        //GIVEN
        var user = new DietMenuUser
        {
            Id = this.userId.ToString(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user)))
            .With(i => i.Setup(m => m.ConfirmEmailAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success)))
            .With(i => i.Setup(m => m.UpdateAsync(It.IsAny<DietMenuUser>()))
                .Callback<DietMenuUser>(sterlingUser => user = sterlingUser)
                .Returns(Task.FromResult(IdentityResult.Success)))
            .With(i => i.Setup(m => m.ResetPasswordAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success)))
            .Build();

        var command = new ConfirmUser
        {
            Id = this.userId,
            Token = "token",
        };

        var commandHandler = new ConfirmUserHandler(this.logger, userManager.Object);

        //WHEN
        _ = await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        user.Should()
            .BeEquivalentTo(command, options => options
                .ExcludingMissingMembers()
                .Excluding(i => i.Id))
            ;

        userManager.Verify(i => i.ConfirmEmailAsync(It.IsAny<DietMenuUser>(), command.Token), Times.Once);
    }
}


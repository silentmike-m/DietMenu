namespace SilentMike.DietMenu.Auth.UnitTests.Users;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.CommandHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.UnitTests.Services;
using IdentityOptions = SilentMike.DietMenu.Auth.Infrastructure.Identity.IdentityOptions;

[TestClass]
public sealed class CreateUserHandlerTests
{
    private const string REGISTER_CODE = "create_user_code";

    private readonly NullLogger<CreateUserHandler> logger;
    private readonly Mock<IMediator> mediator;
    private readonly IOptions<IdentityOptions> options;

    public CreateUserHandlerTests()
    {
        this.logger = new NullLogger<CreateUserHandler>();
        this.mediator = new Mock<IMediator>();

        this.options = Options.Create<IdentityOptions>(new IdentityOptions
        {
            RegisterCode = REGISTER_CODE,
        });
    }

    [TestMethod]
    public async Task ShouldThrowArgumentExceptionWhenInvalidRegisterCode()
    {
        //GIVEN
        var userManager = new FakeUserManagerBuilder().Build();

        var command = new CreateUser();

        var commandHandler = new CreateUserHandler(this.logger, this.mediator.Object, this.options, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ArgumentException>()
            ;
    }

    [TestMethod]
    public async Task ShouldThrowExceptionWhenErrorOnCreate()
    {
        //GIVEN
        var identityError = new IdentityError()
        {
            Code = "test",
            Description = "test",
        };
        var identityResult = IdentityResult.Failed(identityError);

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .With(i => i.Setup(m => m.CreateAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(identityResult)))
            .Build();

        var command = new CreateUser
        {
            Email = "test@rapid.com",
            RegisterCode = REGISTER_CODE,
        };

        var commandHandler = new CreateUserHandler(this.logger, this.mediator.Object, this.options, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CreateUserException>()
                .Where(i => i.Code == ErrorCodes.CREATE_USER_ERROR)
            ;
    }

    [TestMethod]
    public async Task ShouldCreateUser()
    {
        DietMenuUser? result = null;

        //GIVEN
        var identityResult = IdentityResult.Success;

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                .With(i => i.Setup(m => m.CreateAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>()))
                    .Callback<DietMenuUser, string>((user, _) => result = user)
                    .Returns(Task.FromResult(identityResult)))
            .Build();

        var command = new CreateUser
        {
            Email = "user@domain.com",
            Family = "Smiths",
            FirstName = "John",
            LastName = "Smith",
            Password = "P@ssw0rd",
            RegisterCode = REGISTER_CODE,
        };

        var commandHandler = new CreateUserHandler(this.logger, this.mediator.Object, this.options, userManager.Object);

        //WHEN
        _ = await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        mediator.Verify(i => i.Publish(It.IsAny<CreatedUser>(), CancellationToken.None), Times.Once);

        result.Should()
            .NotBeNull()
            ;
        result!.Email.Should()
            .Be(command.Email)
            ;
        result!.FirstName.Should()
            .Be(command.FirstName)
            ;
        result!.LastName.Should()
            .Be(command.LastName)
            ;
        result!.UserName.Should()
            .Be(command.Email);
    }
}

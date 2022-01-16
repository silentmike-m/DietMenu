namespace DietMenu.Core.UnitTests.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DietMenu.Core.Application.Common.Constants;
using DietMenu.Core.Application.Exceptions;
using DietMenu.Core.Application.Exceptions.Families;
using DietMenu.Core.Application.Exceptions.Users;
using DietMenu.Core.Application.Users.Commands;
using DietMenu.Core.Application.Users.Events;
using DietMenu.Core.Application.Users.ViewModels;
using DietMenu.Core.Domain.Entities;
using DietMenu.Core.Infrastructure.EntityFramework.Interfaces;
using DietMenu.Core.Infrastructure.Identity.CommandHandlers;
using DietMenu.Core.Infrastructure.Identity.Models;
using DietMenu.Core.UnitTests.Services;
using DietMenu.Core.UnitTests.Services.DbSetMoq;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public sealed class CreateUserHandlerTests
{
    private const string CREATE_USER_CODE = "create_user_code";

    private readonly string existingFamilyName = "FamilyName";

    private readonly IConfiguration configuration;

    private readonly Mock<IApplicationDbContext> context = new();
    private readonly NullLogger<CreateUserHandler> logger = new();
    private readonly Mock<IMediator> mediator = new();

    public CreateUserHandlerTests()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Identity:CreateUserCode", CREATE_USER_CODE},
        };

        this.configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();


        var families = new List<FamilyEntity>
        {
            new()
            {
                Name = this.existingFamilyName,
            },
        };

        this.context.Setup(i => i.Families).ReturnsDbSet(families);
    }

    [TestMethod]
    public async Task ShouldThrowInvalidCreateUserTokenExceptionWhenMissingToken()
    {
        //GIVEN
        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new DietMenuUser())))
            .Build();

        var command = new CreateUser();

        var commandHandler = new CreateUserHandler(this.configuration, this.context.Object, this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.CREATE_USER_INCORRECT_CREATE_CODE)
                    && i.Errors[ValidationErrorCodes.CREATE_USER_INCORRECT_CREATE_CODE]
                        .Contains(ValidationErrorCodes.CREATE_USER_INCORRECT_CREATE_CODE_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowInvalidCreateUserTokenExceptionWhenInvalidToken()
    {
        //GIVEN
        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new DietMenuUser())))
            .Build();

        var command = new CreateUser
        {
            CreateCode = "test code",
        };

        var commandHandler = new CreateUserHandler(this.configuration, this.context.Object, this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ValidationException>()
                .Where(i =>
                    i.Errors.Count == 1
                    && i.Errors.ContainsKey(ValidationErrorCodes.CREATE_USER_INCORRECT_CREATE_CODE)
                    && i.Errors[ValidationErrorCodes.CREATE_USER_INCORRECT_CREATE_CODE]
                        .Contains(ValidationErrorCodes.CREATE_USER_INCORRECT_CREATE_CODE_MESSAGE))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowCreateUserExceptionWhenUserAlreadyExists()
    {
        //GIVEN
        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new DietMenuUser())))
            .Build();

        var command = new CreateUser
        {
            CreateCode = CREATE_USER_CODE,
            User = new UserToCreate(),
        };

        var commandHandler = new CreateUserHandler(this.configuration, this.context.Object, this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CreateUserException>()
                .Where(i => i.Code == ErrorCodes.CREATE_USER
                            && i.Id == Guid.Empty)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowCreateUserExceptionOnIdentityFail()
    {
        //GIVEN
        var identityError = new IdentityError()
        {
            Code = "test",
            Description = "test",
        };
        var identityResult = IdentityResult.Failed(identityError);

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .With(i => i.Setup(m => m.CreateAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(identityResult)))
            .Build();

        var command = new CreateUser
        {
            CreateCode = CREATE_USER_CODE,
            User = new UserToCreate(),
        };

        var commandHandler = new CreateUserHandler(this.configuration, this.context.Object, this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CreateUserException>()
                .Where(i => i.Code == ErrorCodes.CREATE_USER
                            && i.Id == Guid.Empty
                            && i.Message.Contains(identityError.Description))
            ;
    }

    [TestMethod]
    public async Task ShouldThrowFamilyExistsWhenSameFamilyName()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            FamilyName = this.existingFamilyName,
            Id = Guid.NewGuid(),
        };

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .With(i => i.Setup(m => m.CreateAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success)))
            .Build();

        var command = new CreateUser
        {
            CreateCode = CREATE_USER_CODE,
            User = userToCreate,
        };

        var commandHandler = new CreateUserHandler(this.configuration, this.context.Object, this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyAlreadyExistsException>()
                .Where(i => i.Code == ErrorCodes.FAMILY_ALREADY_EXISTS
                            && i.Id == Guid.Empty
                            && i.Message.Contains(this.existingFamilyName))
            ;
    }

    [TestMethod]
    public async Task ShouldCreateUser()
    {
        string? createdPassword = null;
        DietMenuUser? createdUser = null;
        CreatedUser? createdUserNotification = null;

        //GIVEN
        this.mediator.Setup(i => i.Publish(It.IsAny<CreatedUser>(), It.IsAny<CancellationToken>()))
            .Callback<CreatedUser, CancellationToken>((notification, _) => createdUserNotification = notification)
            ;

        var userToCreate = new UserToCreate
        {
            Email = "test@test.pl",
            FamilyName = "Family Name",
            FirstName = "First Name",
            Id = Guid.NewGuid(),
            LastName = "Last Name",
            Password = "Password",
            UserName = "User Name",
        };

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .With(i => i.Setup(m => m.CreateAsync(It.IsAny<DietMenuUser>(), It.IsAny<string>()))

                .Callback<DietMenuUser, string>((user, password) =>
                {
                    createdPassword = password;
                    createdUser = user;
                })
                .Returns(Task.FromResult(IdentityResult.Success)))
            .Build();

        var command = new CreateUser
        {
            CreateCode = CREATE_USER_CODE,
            User = userToCreate,
        };

        var commandHandler = new CreateUserHandler(this.configuration, this.context.Object, this.logger, this.mediator.Object, userManager.Object);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<CreatedUser>(), It.IsAny<CancellationToken>()), Times.Once);

        createdUserNotification.Should()
            .NotBeNull()
            ;
        createdUserNotification!.FamilyId.Should()
            .NotBeEmpty()
            ;
        createdUserNotification.FamilyName.Should()
            .Be(userToCreate.FamilyName)
            ;
        createdUserNotification.UserId.Should()
            .Be(userToCreate.Id)
            ;

        createdPassword.Should()
            .Be(userToCreate.Password)
            ;

        createdUser.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(userToCreate, opt => opt
                .ExcludingMissingMembers())
            ;
    }
}

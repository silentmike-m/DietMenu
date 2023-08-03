namespace SilentMike.DietMenu.Auth.UnitTests.Users.CommandHandlers;

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Families;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.CommandHandlers;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Events;
using SilentMike.DietMenu.Auth.Application.Users.ValueModels;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Services;
using SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;

[TestClass]
public sealed class CreateUserHandlerTests
{
    private static readonly FamilyEntity EXISTING_FAMILY = new(Guid.NewGuid(), "existing family");
    private static readonly UserEntity EXISTING_USER = new("user@domain.com", Guid.NewGuid(), "first name", "last name", Guid.NewGuid());

    private readonly Mock<IFamilyRepository> familyRepository = new();
    private readonly NullLogger<CreateUserHandler> logger = new();
    private readonly Mock<IPublisher> mediator = new();
    private readonly Mock<IUserRepository> userService = new();

    public CreateUserHandlerTests()
    {
        this.familyRepository
            .Setup(service => service.GetByIdAsync(EXISTING_FAMILY.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(EXISTING_FAMILY)
            ;

        this.userService
            .Setup(service => service.GetByIdAsync(EXISTING_USER.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(EXISTING_USER)
            ;

        this.userService
            .Setup(service => service.GetByEmailAsync(EXISTING_USER.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(EXISTING_USER)
            ;
    }

    [TestMethod]
    public async Task Should_Create_User()
    {
        //GIVEN
        string? createdPassword = null;
        CreatedUser? createdUserNotification = null;
        UserEntity? createdUser = null;

        this.mediator
            .Setup(service => service.Publish(It.IsAny<CreatedUser>(), It.IsAny<CancellationToken>()))
            .Callback<CreatedUser, CancellationToken>((notification, _) => createdUserNotification = notification);

        this.userService
            .Setup(service => service.CreateUserAsync(It.IsAny<string>(), It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .Callback<string, UserEntity, CancellationToken>((password, user, _) =>
            {
                createdPassword = password;
                createdUser = user;
            });

        var userToCreate = new UserToCreate
        {
            Email = "newUser@domain.com",
            FamilyId = EXISTING_FAMILY.Id,
            FirstName = "John",
            Id = Guid.NewGuid(),
            LastName = "Wick",
            Password = "P@ssw0rd",
        };

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var handler = new CreateUserHandler(this.familyRepository.Object, this.logger, this.mediator.Object, this.userService.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        createdPassword.Should()
            .Be(userToCreate.Password)
            ;

        var expectedUser = new UserEntity(userToCreate.Email, userToCreate.FamilyId, userToCreate.FirstName, userToCreate.LastName, userToCreate.Id);

        createdUser.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedUser)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<CreatedUser>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new CreatedUser
        {
            Email = userToCreate.Email,
            Id = userToCreate.Id,
        };

        createdUserNotification.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Not_Publish_Notification_On_Create_User_Errors()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = "newUser@domain.com",
            FamilyId = EXISTING_FAMILY.Id,
            FirstName = "John",
            Id = Guid.NewGuid(),
            LastName = "Wick",
            Password = "P@ssw0rd",
        };

        var exception = new CreateUserException(userToCreate.Email, "error");

        this.userService
            .Setup(service => service.CreateUserAsync(It.IsAny<string>(), It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .Throws(exception);

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var handler = new CreateUserHandler(this.familyRepository.Object, this.logger, this.mediator.Object, this.userService.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CreateUserException>()
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<CreatedUser>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task Should_Throw_Family_Not_Found_Exception_When_Missing_Family()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = "newUser@domain.com",
            FamilyId = Guid.NewGuid(),
            FirstName = "John",
            Id = Guid.NewGuid(),
            LastName = "Wick",
            Password = "P@ssw0rd",
        };

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var handler = new CreateUserHandler(this.familyRepository.Object, this.logger, this.mediator.Object, this.userService.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_NOT_FOUND)
                .Where(exception => exception.Id == userToCreate.FamilyId)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<CreatedUser>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task Should_Throw_User_Already_Exists_When_User_With_Same_Email_Exists()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = EXISTING_USER.Email,
            FamilyId = EXISTING_FAMILY.Id,
            FirstName = "John",
            Id = Guid.NewGuid(),
            LastName = "Wick",
            Password = "P@ssw0rd",
        };

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var handler = new CreateUserHandler(this.familyRepository.Object, this.logger, this.mediator.Object, this.userService.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserAlreadyExistsException>()
                .Where(exception => exception.Code == ErrorCodes.USER_ALREADY_EXISTS)
                .WithMessage($"*{userToCreate.Email}*")
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<CreatedUser>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task Should_Throw_User_Already_Exists_When_User_With_Same_Id_Exists()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = "newUser@domain.com",
            FamilyId = EXISTING_FAMILY.Id,
            FirstName = "John",
            Id = EXISTING_USER.Id,
            LastName = "Wick",
            Password = "P@ssw0rd",
        };

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var handler = new CreateUserHandler(this.familyRepository.Object, this.logger, this.mediator.Object, this.userService.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserAlreadyExistsException>()
                .Where(exception => exception.Code == ErrorCodes.USER_ALREADY_EXISTS)
                .Where(exception => exception.Id == userToCreate.Id)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<CreatedUser>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

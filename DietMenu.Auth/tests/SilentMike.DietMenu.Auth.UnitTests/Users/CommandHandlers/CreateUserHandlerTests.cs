namespace SilentMike.DietMenu.Auth.UnitTests.Users.CommandHandlers;

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
    private static readonly FamilyEntity EXISTING_FAMILY = new(Guid.NewGuid(), "family@domain.com", "existing family");
    private static readonly UserEntity EXISTING_USER = new("user@domain.com", Guid.NewGuid(), "first name", "last name", Guid.NewGuid());

    private readonly IFamilyRepository familyRepository = Substitute.For<IFamilyRepository>();
    private readonly NullLogger<CreateUserHandler> logger = new();
    private readonly IPublisher mediator = Substitute.For<IPublisher>();
    private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();

    public CreateUserHandlerTests()
    {
        this.familyRepository
            .GetByIdAsync(EXISTING_FAMILY.Id, Arg.Any<CancellationToken>())
            .Returns(EXISTING_FAMILY)
            ;

        this.userRepository
            .GetByIdAsync(EXISTING_USER.Id, Arg.Any<CancellationToken>())
            .Returns(EXISTING_USER)
            ;

        this.userRepository
            .GetByEmailAsync(EXISTING_USER.Email, Arg.Any<CancellationToken>())
            .Returns(EXISTING_USER)
            ;
    }

    [TestMethod]
    public async Task Should_Create_User()
    {
        //GIVEN
        string? createdPassword = null;
        CreatedUser? createdUserNotification = null;
        UserEntity? createdUser = null;

        await this.mediator
            .Publish(Arg.Do<CreatedUser>(notification => createdUserNotification = notification), Arg.Any<CancellationToken>());

        await this.userRepository
            .CreateUserAsync(Arg.Do<string>(password => createdPassword = password), Arg.Do<UserEntity>(user => createdUser = user), Arg.Any<CancellationToken>());

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

        var handler = new CreateUserHandler(this.familyRepository, this.logger, this.mediator, this.userRepository);

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

        _ = this.mediator.Received(1).Publish(Arg.Any<CreatedUser>(), Arg.Any<CancellationToken>());

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

        this.userRepository
            .CreateUserAsync(Arg.Any<string>(), Arg.Any<UserEntity>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(exception));

        var request = new CreateUser
        {
            User = userToCreate,
        };

        var handler = new CreateUserHandler(this.familyRepository, this.logger, this.mediator, this.userRepository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CreateUserException>()
            ;

        _ = this.mediator.Received(0).Publish(Arg.Any<CreatedUser>(), Arg.Any<CancellationToken>());
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

        var handler = new CreateUserHandler(this.familyRepository, this.logger, this.mediator, this.userRepository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_NOT_FOUND)
                .Where(exception => exception.Id == userToCreate.FamilyId)
            ;

        _ = this.mediator.Received(0).Publish(Arg.Any<CreatedUser>(), Arg.Any<CancellationToken>());
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

        var handler = new CreateUserHandler(this.familyRepository, this.logger, this.mediator, this.userRepository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserAlreadyExistsException>()
                .Where(exception => exception.Code == ErrorCodes.USER_ALREADY_EXISTS)
                .WithMessage($"*{userToCreate.Email}*")
            ;

        _ = this.mediator.Received(0).Publish(Arg.Any<CreatedUser>(), Arg.Any<CancellationToken>());
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

        var handler = new CreateUserHandler(this.familyRepository, this.logger, this.mediator, this.userRepository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserAlreadyExistsException>()
                .Where(exception => exception.Code == ErrorCodes.USER_ALREADY_EXISTS)
                .Where(exception => exception.Id == userToCreate.Id)
            ;

        _ = this.mediator.Received(0).Publish(Arg.Any<CreatedUser>(), Arg.Any<CancellationToken>());
    }
}

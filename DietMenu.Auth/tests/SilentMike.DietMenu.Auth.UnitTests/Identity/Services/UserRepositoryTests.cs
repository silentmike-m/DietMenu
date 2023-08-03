namespace SilentMike.DietMenu.Auth.UnitTests.Identity.Services;

using System.Reflection;
using FluentAssertions;
using global::AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Infrastructure;
using SilentMike.DietMenu.Auth.Infrastructure.Common.Constants;
using SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Services;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class UserRepositoryTests : FakeDietMenuDbContext
{
    private static readonly Family EXISTING_FAMILY = new()
    {
        Id = Guid.NewGuid(),
        Key = 2,
        Name = "family name",
    };

    private readonly IMapper mapper;

    public UserRepositoryTests()
        : base(EXISTING_FAMILY)
    {
        var config = new MapperConfiguration(config => config.AddMaps(Assembly.GetAssembly(typeof(DependencyInjection))));
        this.mapper = config.CreateMapper();
    }

    [TestMethod]
    public async Task Should_Create_User()
    {
        //GIVEN
        User? createdUser = null;

        const string password = "P@ssw0rd";

        var userToCreate = new UserEntity("user@domain.com", EXISTING_FAMILY.Id, "John", "Wick", Guid.NewGuid());

        var identityResult = IdentityResult.Success;

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .Setup(service => service.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Callback<User, string>((user, _) => createdUser = user)
                .ReturnsAsync(identityResult))
            .Build();

        var service = new UserRepository(this.Context!, this.mapper, userManager.Object);

        //WHEN
        await service.CreateUserAsync(password, userToCreate, CancellationToken.None);

        //THEN
        var expectedUser = new User
        {
            Email = userToCreate.Email,
            Family = EXISTING_FAMILY,
            FamilyId = EXISTING_FAMILY.Id,
            FamilyKey = EXISTING_FAMILY.Key,
            FirstName = userToCreate.FirstName,
            Id = userToCreate.Id.ToString(),
            LastName = userToCreate.LastName,
            UserName = userToCreate.Email,
        };

        createdUser.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedUser, options => options
                .Excluding(user => user.ConcurrencyStamp)
                .Excluding(user => user.SecurityStamp)
            )
            ;
    }

    [TestMethod]
    public async Task Should_Return_Null_On_Get_By_Email_When_Missing_User()
    {
        //GIVEN
        var user = new User
        {
            Email = "user@domain.com",
            Family = new Family
            {
                Id = Guid.NewGuid(),
            },
            FirstName = "John",
            Id = "18ff1b8b-0f85-4b37-b5d3-073f82f52431",
            LastLogin = DateTime.Now,
            LastName = "Wick",
        };

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .Setup(service => service.FindByEmailAsync(user.Email))
                .ReturnsAsync(user)
            )
            .Build();

        var service = new UserRepository(this.Context!, this.mapper, userManager.Object);

        //WHEN
        var result = await service.GetByEmailAsync("email@domain.com", CancellationToken.None);

        //THEN
        result.Should()
            .BeNull()
            ;
    }

    [TestMethod]
    public async Task Should_Return_Null_On_Get_By_Id_When_Missing_User()
    {
        //GIVEN
        var userId = Guid.NewGuid();

        var user = new User
        {
            Email = "user@domain.com",
            Family = new Family
            {
                Id = Guid.NewGuid(),
            },
            FirstName = "John",
            Id = userId.ToString(),
            LastLogin = DateTime.Now,
            LastName = "Wick",
        };

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .Setup(service => service.FindByIdAsync(user.Id))
                .ReturnsAsync(user)
            )
            .Build();

        var service = new UserRepository(this.Context!, this.mapper, userManager.Object);

        //WHEN
        var result = await service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        //THEN
        result.Should()
            .BeNull()
            ;
    }

    [TestMethod]
    public async Task Should_Return_User_On_Get_By_Email()
    {
        //GIVEN
        var user = new User
        {
            Email = "user@domain.com",
            Family = new Family
            {
                Id = Guid.NewGuid(),
            },
            FamilyId = Guid.NewGuid(),
            FirstName = "John",
            Id = "18ff1b8b-0f85-4b37-b5d3-073f82f52431",
            LastLogin = DateTime.Now,
            LastName = "Wick",
        };

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .Setup(service => service.FindByEmailAsync(user.Email))
                .ReturnsAsync(user)
            )
            .Build();

        var service = new UserRepository(this.Context!, this.mapper, userManager.Object);

        //WHEN
        var result = await service.GetByEmailAsync(user.Email, CancellationToken.None);

        //THEN
        var expectedResult = new UserEntity(user.Email, user.FamilyId, user.FirstName, user.LastName, new Guid(user.Id), user.Role);

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Return_User_On_Get_By_Id()
    {
        //GIVEN
        var userId = Guid.NewGuid();

        var user = new User
        {
            Email = "user@domain.com",
            Family = new Family
            {
                Id = Guid.NewGuid(),
            },
            FamilyId = Guid.NewGuid(),
            FirstName = "John",
            Id = userId.ToString(),
            LastLogin = DateTime.Now,
            LastName = "Wick",
        };

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .Setup(service => service.FindByIdAsync(user.Id))
                .ReturnsAsync(user)
            )
            .Build();

        var service = new UserRepository(this.Context!, this.mapper, userManager.Object);

        //WHEN
        var result = await service.GetByIdAsync(userId, CancellationToken.None);

        //THEN
        var expectedResult = new UserEntity(user.Email, user.FamilyId, user.FirstName, user.LastName, new Guid(user.Id), user.Role);

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Create_User_Exception_When_Error_On_Create()
    {
        //GIVEN
        var identityError = new IdentityError
        {
            Code = "test",
            Description = "test",
        };

        var identityResult = IdentityResult.Failed(identityError);

        const string password = "P@ssw0rd";

        var userToCreate = new UserEntity("user@domain.com", EXISTING_FAMILY.Id, "John", "Wick", Guid.NewGuid());

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager
                .Setup(service => service.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult))
            .Build();

        var service = new UserRepository(this.Context!, this.mapper, userManager.Object);

        //WHEN
        var action = async () => await service.CreateUserAsync(password, userToCreate, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CreateUserException>()
                .Where(exception => exception.Code == ErrorCodes.CREATE_USER_ERROR)
                .WithMessage($"*{userToCreate.Email}*")
            ;
    }
}

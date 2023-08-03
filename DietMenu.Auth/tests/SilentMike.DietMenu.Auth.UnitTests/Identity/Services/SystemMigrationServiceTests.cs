namespace SilentMike.DietMenu.Auth.UnitTests.Identity.Services;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;
using SilentMike.DietMenu.Auth.Domain.Enums;
using SilentMike.DietMenu.Auth.Infrastructure.Identity;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Services;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class SystemMigrationServiceTests : FakeDietMenuDbContext
{
    private const string DEFAULT_SYSTEM_USER_EMAIL = "system@domain.com";
    private const string DEFAULT_SYSTEM_USER_FIRST_NAME = "John";
    private const string DEFAULT_SYSTEM_USER_LAST_NAME = "Smith";
    private const string DEFAULT_SYSTEM_USER_PASSWORD = "P@ssw0rd";

    private readonly NullLogger<SystemMigrationService> logger = new();

    [TestMethod]
    public async Task Should_Create_User_With_Default_Values_When_Missing_Config()
    {
        //GIVEN
        User? createdUser = null;

        var identityOptions = Options.Create(new IdentityOptions());

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.CreateAsync(It.IsAny<User>(), DEFAULT_SYSTEM_USER_PASSWORD))
                .Callback<User, string>((user, _) => createdUser = user)
            )
            .Build();

        var service = new SystemMigrationService(this.Context!, identityOptions, this.logger, userManager.Object);

        //WHEN
        await service.MigrateSystemAsync(CancellationToken.None);

        //THEN
        var systemFamily = await this.Context!.Families.SingleAsync(family => family.Id == FamilyIds.SYSTEM_FAMILY_ID);

        var expectedUser = new User
        {
            Email = DEFAULT_SYSTEM_USER_EMAIL,
            EmailConfirmed = true,
            Family = systemFamily,
            FamilyId = systemFamily.Id,
            FamilyKey = systemFamily.Key,
            FirstName = DEFAULT_SYSTEM_USER_FIRST_NAME,
            Role = UserRole.System,
            LastName = DEFAULT_SYSTEM_USER_LAST_NAME,
            UserName = DEFAULT_SYSTEM_USER_EMAIL,
        };

        createdUser.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedUser, options => options
                .Excluding(user => user.ConcurrencyStamp)
                .Excluding(user => user.Id)
                .Excluding(user => user.SecurityStamp)
            )
            ;
    }

    [TestMethod]
    public async Task Should_Create_User_With_Values_From_Config()
    {
        //GIVEN
        User? createdUser = null;

        var identityOptions = Options.Create(new IdentityOptions
        {
            SystemUserEmail = "newsystem@user.com",
            SystemUserPassword = "newPassword",
        });

        var userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.CreateAsync(It.IsAny<User>(), identityOptions.Value.SystemUserPassword))
                .Callback<User, string>((user, _) => createdUser = user)
            )
            .Build();

        var service = new SystemMigrationService(this.Context!, identityOptions, this.logger, userManager.Object);

        //WHEN
        await service.MigrateSystemAsync(CancellationToken.None);

        //THEN
        var systemFamily = await this.Context!.Families.SingleAsync(family => family.Id == FamilyIds.SYSTEM_FAMILY_ID);

        var expectedUser = new User
        {
            Email = identityOptions.Value.SystemUserEmail,
            EmailConfirmed = true,
            Family = systemFamily,
            FamilyId = systemFamily.Id,
            FamilyKey = systemFamily.Key,
            FirstName = DEFAULT_SYSTEM_USER_FIRST_NAME,
            Role = UserRole.System,
            LastName = DEFAULT_SYSTEM_USER_LAST_NAME,
            UserName = identityOptions.Value.SystemUserEmail,
        };

        createdUser.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedUser, options => options
                .Excluding(user => user.ConcurrencyStamp)
                .Excluding(user => user.Id)
                .Excluding(user => user.SecurityStamp)
            )
            ;
    }
}

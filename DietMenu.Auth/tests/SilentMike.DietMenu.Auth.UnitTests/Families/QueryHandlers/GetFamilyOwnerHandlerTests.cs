namespace SilentMike.DietMenu.Auth.UnitTests.Families.QueryHandlers;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Families;
using SilentMike.DietMenu.Auth.Application.Families.Queries;
using SilentMike.DietMenu.Auth.Application.Families.ViewModels;
using SilentMike.DietMenu.Auth.Infrastructure.Families.QueryHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class GetFamilyOwnerHandlerTests : FakeDietMenuDbContext
{
    private static readonly Family EXISTING_FAMILY = new()
    {
        Id = Guid.NewGuid(),
        Key = 1,
        Name = "family name",
    };

    private readonly NullLogger<GetFamilyOwnerHandler> logger = new();

    public GetFamilyOwnerHandlerTests()
        : base(EXISTING_FAMILY)
    {
    }

    [TestMethod]
    public async Task Should_Return_Family_Owner()
    {
        //GIVEN
        var user = new User
        {
            Email = "user@domain.com",
            FamilyKey = EXISTING_FAMILY.Key,
            Id = Guid.NewGuid().ToString(),
        };

        var users = new List<User>
        {
            user,
        }.AsQueryable();

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager.Setup(
                    service => service.Users)
                .Returns(users)
            )
            .Build();

        var request = new GetFamilyOwner
        {
            FamilyId = EXISTING_FAMILY.Id,
        };

        var handler = new GetFamilyOwnerHandler(this.Context!, this.logger, userManager.Object);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        var expectedResult = new FamilyOwner
        {
            Email = user.Email,
            UserId = new Guid(user.Id),
        };

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Family_Not_Found_Exception_When_Missing_Family()
    {
        //GIVEN
        var userManager = new FakeUserManagerBuilder()
            .Build();

        var request = new GetFamilyOwner
        {
            FamilyId = Guid.NewGuid(),
        };

        var handler = new GetFamilyOwnerHandler(this.Context!, this.logger, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_NOT_FOUND)
                .Where(exception => exception.Id == request.FamilyId)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Family_Owner_Not_Found_Exception_When_Missing_User()
    {
        //GIVEN
        var user = new User
        {
            FamilyKey = 2,
        };

        var users = new List<User>
        {
            user,
        }.AsQueryable();

        var userManager = new FakeUserManagerBuilder()
            .With(userManager => userManager.Setup(
                    service => service.Users)
                .Returns(users)
            )
            .Build();

        var request = new GetFamilyOwner
        {
            FamilyId = EXISTING_FAMILY.Id,
        };

        var handler = new GetFamilyOwnerHandler(this.Context!, this.logger, userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyOwnerNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_OWNER_NOT_FOUND)
                .Where(exception => exception.Id == request.FamilyId)
            ;
    }
}

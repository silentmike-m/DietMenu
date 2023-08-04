namespace SilentMike.DietMenu.Core.UnitTests.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.Common.Constants;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.Families;
using SilentMike.DietMenu.Core.UnitTests.Helpers;

[TestClass]
public sealed class FamilyRepositoryTests : FakeDietMenuDbContext
{
    private static readonly Family EXISTING_FAMILY = new()
    {
        Id = 1,
        InternalId = Guid.NewGuid(),
    };

    public FamilyRepositoryTests()
        : base(EXISTING_FAMILY)
    {
    }

    [TestMethod]
    public async Task Should_Add_Family()
    {
        //GIVEN
        var family = new FamilyEntity(Guid.NewGuid());

        var repository = new FamilyRepository(this.Context!);

        //WHEN
        await repository.AddFamilyAsync(family, CancellationToken.None);

        //THEN
        var result = await this.Context!.Families.SingleOrDefaultAsync(result => result.InternalId == family.Id, CancellationToken.None);

        result.Should()
            .NotBeNull()
            ;
    }

    [TestMethod]
    public async Task Should_Return_False_When_Family_Not_Exists()
    {
        //GIVEN
        var id = Guid.NewGuid();

        var repository = new FamilyRepository(this.Context!);

        //WHEN
        var result = await repository.ExistsAsync(id, CancellationToken.None);

        //THEN
        result.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Return_True_When_Family_Exists()
    {
        //GIVEN
        var id = EXISTING_FAMILY.InternalId;

        var repository = new FamilyRepository(this.Context!);

        //WHEN
        var result = await repository.ExistsAsync(id, CancellationToken.None);

        //THEN
        result.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Family_Already_Exists_When_Family_With_Same_Internal_Id_Exists_On_Add()
    {
        //GIVEN
        var family = new FamilyEntity(EXISTING_FAMILY.InternalId);

        var repository = new FamilyRepository(this.Context!);

        //WHEN
        var action = async () => await repository.AddFamilyAsync(family, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyAlreadyExistsException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_ALREADY_EXISTS)
                .Where(exception => exception.Id == family.Id)
            ;
    }
}

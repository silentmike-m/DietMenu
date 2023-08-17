namespace SilentMike.DietMenu.Core.InfrastructureTests.EntityFramework.Services;

using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.InfrastructureTests.Helpers;

[TestClass]
public sealed class FamilyRepositoryTests : FakeDietMenuDbContext
{
    private static readonly FamilyEntity EXISTING_FAMILY = new()
    {
        Id = 1,
        FamilyId = Guid.NewGuid(),
    };

    public FamilyRepositoryTests()
        : base(EXISTING_FAMILY)
    {
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
        AssertionExtensions.Should((bool)result)
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Return_True_When_Family_Exists()
    {
        //GIVEN
        var id = EXISTING_FAMILY.FamilyId;

        var repository = new FamilyRepository(this.Context!);

        //WHEN
        var result = await repository.ExistsAsync(id, CancellationToken.None);

        //THEN
        AssertionExtensions.Should((bool)result)
            .BeTrue()
            ;
    }
}

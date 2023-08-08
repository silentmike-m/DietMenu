namespace SilentMike.DietMenu.Core.InfrastructureTests.Families.QueryHandlers;

using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;
using SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;
using SilentMike.DietMenu.Core.InfrastructureTests.Helpers;

[TestClass]
public sealed class GetFamilyDataToImportHandlerTests : FakeDietMenuDbContext
{
    private static readonly FamilyEntity EXISTING_FAMILY = new()
    {
        Id = 1,
        IngredientsVersion = "version 2.2",
        InternalId = Guid.NewGuid(),
    };

    private readonly NullLogger<GetFamilyDataToImportHandler> logger = new();

    public GetFamilyDataToImportHandlerTests()
        : base(EXISTING_FAMILY)
    {
    }

    [TestMethod]
    public async Task Should_Return_Empty_String_As_Ingredients_Version_When_Missing_Family()
    {
        //GIVEN
        var request = new GetFamilyDataToImport
        {
            FamilyId = Guid.NewGuid(),
        };

        var handler = new GetFamilyDataToImportHandler(this.Context!, this.logger);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        result.IngredientsVersion.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public async Task Should_Return_Ingredients_Version()
    {
        //GIVEN
        var request = new GetFamilyDataToImport
        {
            FamilyId = EXISTING_FAMILY.InternalId,
        };

        var handler = new GetFamilyDataToImportHandler(this.Context!, this.logger);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        result.IngredientsVersion.Should()
            .Be(EXISTING_FAMILY.IngredientsVersion)
            ;
    }
}

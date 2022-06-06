namespace SilentMike.DietMenu.Core.UnitTests.Core;

using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.Core.QueryHandlers;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class GetCoreDataToImportHandlerTests : IDisposable
{
    private DietMenuDbContextFactory factory = null!;

    private readonly NullLogger<GetCoreDataToImportHandler> logger = new();

    [TestMethod]
    public async Task ShouldNotCreateCoreWhenNoExistingOne()
    {
        //GIVEN
        this.factory = new DietMenuDbContextFactory();

        var request = new GetCoreDataToImport
        {
            IngredientsPayload = new byte[10],
        };

        var requestHandler = new GetCoreDataToImportHandler(this.factory.Context, this.logger);

        //WHEN
        var result = await requestHandler.Handle(request, CancellationToken.None);

        //TEST
        factory.Context.Core.Should()
            .BeEmpty()
            ;
        result.IngredientTypes.Should()
            .BeEmpty()
            ;
        result.Ingredients.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public async Task ShouldNotCreateCoreWhenExistingOne()
    {
        //GIVEN
        var core = new CoreEntity(Guid.NewGuid());
        this.factory = new DietMenuDbContextFactory(core);

        var request = new GetCoreDataToImport
        {
            IngredientsPayload = new byte[10],
        };

        var requestHandler = new GetCoreDataToImportHandler(factory.Context, this.logger);

        //WHEN
        var result = await requestHandler.Handle(request, CancellationToken.None);

        //TEST
        factory.Context.Core.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.Id == core.Id)
            ;

        result.Core.Should()
            .Be(factory.Context.Core.Single())
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}

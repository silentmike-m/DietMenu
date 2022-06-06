namespace SilentMike.DietMenu.Core.UnitTests.Core;

using Microsoft.Extensions.FileProviders;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Core.Models;
using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Infrastructure.Core.QueryHandlers;

[TestClass]
public sealed class GetCoreDataPayloadsToImportHandlerTests
{
    private readonly NullLogger<GetCoreDataPayloadsToImportHandler> logger = new();

    [TestMethod]
    public async Task ShouldReturnIngredientsPayloadOnGetCoreDataPayloadsToImport()
    {
        //GIVEN
        var fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

        var request = new GetCoreDataPayloadsToImport();

        var requestHandler = new GetCoreDataPayloadsToImportHandler(fileProvider, this.logger);

        //WHEN
        var result = await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        result.IngredientsPayload.Should()
            .NotBeNull()
            ;
    }

    [TestMethod]
    public async Task ShouldThrowResourceNotFoundWhenMissingFileOnGetCoreDataPayloadsToImport()
    {
        //GIVEN
        var fileProvider = new NullFileProvider();

        var request = new GetCoreDataPayloadsToImport();

        var requestHandler = new GetCoreDataPayloadsToImportHandler(fileProvider, this.logger);

        //WHEN
        Func<Task<CoreDataPayloadsToImport>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ResourceNotFoundException>()
                .Where(i => i.Code == ErrorCodes.RESOURCE_NOT_FOUND)
            ;
    }
}

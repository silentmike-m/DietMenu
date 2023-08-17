namespace SilentMike.DietMenu.Core.InfrastructureTests.Families.QueryHandlers;

using System.Reflection;
using Microsoft.Extensions.FileProviders;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Infrastructure.Common.Constants;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.Families;
using SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;

[TestClass]
public sealed class GetFamilyIngredientsPayloadHandlerTests
{
    private readonly NullLogger<GetFamilyIngredientsPayloadHandler> logger = new();

    [TestMethod]
    public async Task Should_Return_File_Payload()
    {
        //GIVEN
        var fileProvider = new EmbeddedFileProvider(Assembly.GetAssembly(typeof(GetFamilyIngredientsPayloadHandler))!);

        var request = new GetFamilyIngredientsPayload
        {
            FamilyId = Guid.NewGuid(),
        };

        var handler = new GetFamilyIngredientsPayloadHandler(fileProvider, this.logger);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        result.Should()
            .NotBeEmpty()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Family_File_Not_Found_When_Missing_File()
    {
        //GIVEN
        var fileProvider = new PhysicalFileProvider(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()));

        var request = new GetFamilyIngredientsPayload
        {
            FamilyId = Guid.NewGuid(),
        };

        var handler = new GetFamilyIngredientsPayloadHandler(fileProvider, this.logger);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyFileNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_FILE_NOT_FOUND)
                .Where(exception => exception.Id == request.FamilyId)
            ;
    }
}

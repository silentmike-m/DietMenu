namespace SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;

using Microsoft.Extensions.FileProviders;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Infrastructure.Common.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.Families;

internal sealed class GetFamilyIngredientsPayloadHandler : IRequestHandler<GetFamilyIngredientsPayload, byte[]>
{
    private const string INGREDIENTS_RESOURCE_NAME = "Ingredients.xlsx";

    private readonly IFileProvider fileProvider;
    private readonly ILogger<GetFamilyIngredientsPayloadHandler> logger;

    public GetFamilyIngredientsPayloadHandler(IFileProvider fileProvider, ILogger<GetFamilyIngredientsPayloadHandler> logger)
    {
        this.fileProvider = fileProvider;
        this.logger = logger;
    }

    public async Task<byte[]> Handle(GetFamilyIngredientsPayload request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get family ingredients payload");

        var fileInfo = this.fileProvider.GetFileInfo(INGREDIENTS_RESOURCE_NAME);

        if (!fileInfo.Exists)
        {
            throw new FamilyFileNotFoundException(request.FamilyId, INGREDIENTS_RESOURCE_NAME);
        }

        var payload = this.fileProvider
            .GetFileInfo(INGREDIENTS_RESOURCE_NAME)
            .CreateReadStream()
            .ReadAllBytes();

        return await Task.FromResult(payload);
    }
}

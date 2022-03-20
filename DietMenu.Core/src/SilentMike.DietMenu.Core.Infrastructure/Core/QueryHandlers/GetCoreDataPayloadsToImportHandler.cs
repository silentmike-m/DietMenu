namespace SilentMike.DietMenu.Core.Infrastructure.Core.QueryHandlers;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Core.Models;
using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Application.Exceptions;

internal sealed class GetCoreDataPayloadsToImportHandler : IRequestHandler<GetCoreDataPayloadsToImport, CoreDataPayloadsToImport>
{
    private const string INGREDIENTS_RESOURCE_NAME = "Ingredients.xlsx";

    private readonly IFileProvider fileProvider;
    private readonly ILogger<GetCoreDataPayloadsToImportHandler> logger;

    public GetCoreDataPayloadsToImportHandler(IFileProvider fileProvider, ILogger<GetCoreDataPayloadsToImportHandler> logger)
        => (this.fileProvider, this.logger) = (fileProvider, logger);

    public async Task<CoreDataPayloadsToImport> Handle(GetCoreDataPayloadsToImport request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get core payloads to import");

        var ingredientsPayload = await this.GetPayload(INGREDIENTS_RESOURCE_NAME, cancellationToken);

        var result = new CoreDataPayloadsToImport
        {
            IngredientsPayload = ingredientsPayload,
        };

        return result;
    }

    private async Task<byte[]> GetPayload(string resourceName, CancellationToken cancellationToken)
    {
        var fileInfo = this.fileProvider.GetFileInfo(resourceName);

        if (!fileInfo.Exists)
        {
            throw new ResourceNotFoundException(resourceName);
        }

        await using var resourceStream = fileInfo.CreateReadStream();
        await using var memoryStream = new MemoryStream();
        await resourceStream.CopyToAsync(memoryStream, cancellationToken);

        return memoryStream.ToArray();
    }
}

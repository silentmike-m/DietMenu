namespace SilentMike.DietMenu.Core.Infrastructure.Families.QueryHandlers;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Application.Common.Extensions;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

internal sealed class GetFamilyDataToImportHandler : IRequestHandler<GetFamilyDataToImport, FamilyDataToImport>
{
    private readonly IDietMenuDbContext context;
    private readonly ILogger<GetFamilyDataToImportHandler> logger;

    public GetFamilyDataToImportHandler(IDietMenuDbContext context, ILogger<GetFamilyDataToImportHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<FamilyDataToImport> Handle(GetFamilyDataToImport request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope("FamilyId", request.FamilyId);

        var family = await this.context.Families.SingleOrDefaultAsync(family => family.InternalId == request.FamilyId, cancellationToken);

        if (family is null)
        {
            this.logger.LogInformation("Family has not been found");
        }

        var result = new FamilyDataToImport
        {
            FamilyId = request.FamilyId,
            IngredientsVersion = family?.IngredientsVersion ?? string.Empty,
        };

        return result;
    }
}

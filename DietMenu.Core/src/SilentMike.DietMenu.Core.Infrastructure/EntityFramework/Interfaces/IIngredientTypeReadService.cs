namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

internal interface IIngredientTypeReadService
{
    Task<IngredientTypes> GetIngredientTypesAsync(Guid familyId, CancellationToken cancellationToken = default);
    Task<IngredientTypesGrid> GetIngredientTypesGridAsync(Guid familyId, GridRequest gridRequest, CancellationToken cancellationToken = default);
}

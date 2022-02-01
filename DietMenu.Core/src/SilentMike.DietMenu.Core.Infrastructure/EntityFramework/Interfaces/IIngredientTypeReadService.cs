namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

internal interface IIngredientTypeReadService
{
    Task<IngredientTypesGrid> GetIngredientTypesGrid(Guid familyId, GridRequest gridRequest);
}

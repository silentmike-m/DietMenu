namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Interfaces;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.ViewModels;

internal interface IMealTypeReadService
{
    Task<MealTypesGrid> GetMealTypesGridAsync(Guid familyId, GridRequest gridRequest, CancellationToken cancellationToken = default);
}

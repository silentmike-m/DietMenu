namespace SilentMike.DietMenu.Core.Application.Families.Queries;

using SilentMike.DietMenu.Core.Application.Families.Interfaces;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Domain.Entities;

public sealed record GetFamilyDataToImport : IRequest<FamilyDataToImport>, IGetFamilyDataToImport
{
    public FamilyEntity Family { get; init; } = new(Guid.NewGuid());
}

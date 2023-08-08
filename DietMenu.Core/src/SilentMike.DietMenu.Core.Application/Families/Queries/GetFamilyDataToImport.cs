namespace SilentMike.DietMenu.Core.Application.Families.Queries;

using SilentMike.DietMenu.Core.Application.Families.Interfaces;
using SilentMike.DietMenu.Core.Application.Families.Models;

public sealed record GetFamilyDataToImport : IRequest<FamilyDataToImport>, IGetFamilyDataToImport
{
    public Guid FamilyId { get; init; } = Guid.Empty;
}

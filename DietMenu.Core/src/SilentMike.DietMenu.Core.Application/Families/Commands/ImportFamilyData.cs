namespace SilentMike.DietMenu.Core.Application.Families.Commands;

public sealed record ImportFamilyData : IRequest
{
    public Guid FamilyId { get; init; } = Guid.Empty;
}

namespace SilentMike.DietMenu.Auth.Application.Families.Queries;

using SilentMike.DietMenu.Auth.Application.Families.ViewModels;

public sealed record GetFamilyOwner : IRequest<FamilyOwner>
{
    public Guid FamilyId { get; init; } = Guid.Empty;
}

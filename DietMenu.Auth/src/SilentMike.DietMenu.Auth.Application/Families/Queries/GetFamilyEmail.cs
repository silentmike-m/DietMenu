namespace SilentMike.DietMenu.Auth.Application.Families.Queries;

public sealed record GetFamilyEmail : IRequest<string>
{
    public Guid FamilyId { get; init; } = Guid.Empty;
}

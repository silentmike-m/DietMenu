namespace SilentMike.DietMenu.Core.Application.Families.Queries;

public sealed record GetFamilyIngredientsPayload : IRequest<byte[]>
{
    public Guid FamilyId { get; init; } = Guid.Empty;
}

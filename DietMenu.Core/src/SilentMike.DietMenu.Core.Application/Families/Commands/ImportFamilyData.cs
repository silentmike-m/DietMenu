namespace SilentMike.DietMenu.Core.Application.Families.Commands;

using SilentMike.DietMenu.Core.Application.Common;

public sealed record ImportFamilyData : IRequest, IAuthRequest, IFamilyRequest
{
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}

namespace SilentMike.DietMenu.Core.Application.Families.Commands;

using SilentMike.DietMenu.Core.Application.Common;

public sealed record ImportFamilyData : IRequest, IAuthRequest, IFamilyRequest
{
    [JsonIgnore] public IAuthData AuthData { get; set; }
    public Guid FamilyId { get; }
}

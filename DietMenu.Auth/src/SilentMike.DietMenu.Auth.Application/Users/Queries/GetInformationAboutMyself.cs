namespace SilentMike.DietMenu.Auth.Application.Users.Queries;

using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Users.ViewModels;

public sealed record GetInformationAboutMyself : IRequest<User>, IAuthRequest
{
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}

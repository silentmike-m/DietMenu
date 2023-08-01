namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Users.ValueModels;

public sealed record CreateUser : IRequest, IAuthRequest, ISystemRequest
{
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonPropertyName("user")] public UserToCreate User { get; init; } = new();
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}

namespace SilentMike.DietMenu.Auth.Application.Users.Commands;

using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Common.Models;
using SilentMike.DietMenu.Auth.Application.Users.ValueModels;

public sealed record CreateUser : IRequest, IAuthRequest, ISystemRequest
{
    [JsonIgnore] public IAuthData AuthData { get; set; } = new AuthData();
    [JsonPropertyName("user")] public UserToCreate User { get; init; } = new();
}

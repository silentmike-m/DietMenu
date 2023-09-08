namespace SilentMike.DietMenu.Auth.Application.Families.Commands;

using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Common.Models;

public sealed class CreateFamily : IRequest, IAuthRequest, ISystemRequest
{
    [JsonIgnore] public IAuthData AuthData { get; set; } = new AuthData();
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.NewGuid();
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
}

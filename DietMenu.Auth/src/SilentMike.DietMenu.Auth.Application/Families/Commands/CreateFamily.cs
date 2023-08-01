namespace SilentMike.DietMenu.Auth.Application.Families.Commands;

using SilentMike.DietMenu.Auth.Application.Common;

public sealed class CreateFamily : IRequest, IAuthRequest, ISystemRequest
{
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.NewGuid();
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}

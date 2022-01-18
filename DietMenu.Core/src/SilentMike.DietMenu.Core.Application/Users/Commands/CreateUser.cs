namespace SilentMike.DietMenu.Core.Application.Users.Commands;

using System.Text.Json.Serialization;
using MediatR;
using SilentMike.DietMenu.Core.Application.Users.ViewModels;

public sealed record CreateUser : IRequest
{
    [JsonPropertyName("create_code")] public string CreateCode { get; init; } = string.Empty;
    [JsonPropertyName("user")] public UserToCreate User { get; init; } = new();
}

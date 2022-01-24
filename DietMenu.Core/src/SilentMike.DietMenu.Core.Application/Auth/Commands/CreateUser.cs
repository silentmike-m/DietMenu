namespace SilentMike.DietMenu.Core.Application.Auth.Commands;

using System.Text.Json.Serialization;
using MediatR;
using SilentMike.DietMenu.Core.Application.Auth.ViewModels;

public sealed record CreateUser : IRequest
{
    [JsonPropertyName("create_code")] public string CreateCode { get; init; } = string.Empty;
    [JsonPropertyName("user")] public UserToCreate User { get; init; } = new();
}

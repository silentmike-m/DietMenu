namespace DietMenu.Core.Application.Users.Commands;

using System.Text.Json.Serialization;
using DietMenu.Core.Application.Users.ViewModels;
using MediatR;

public sealed record CreateUser : IRequest
{
    [JsonPropertyName("create_code")] public string CreateCode { get; init; } = string.Empty;
    [JsonPropertyName("user")] public UserToCreate User { get; init; } = new();
}

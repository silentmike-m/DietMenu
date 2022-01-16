﻿namespace DietMenu.Core.Application.Families.Commands;

using System.Text.Json.Serialization;
using MediatR;

public sealed record CreateFamilyIfNotExists : IRequest
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
}

﻿namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.Email.Interfaces;

internal sealed record EmailDataMessage : IEmailDataMessage
{
    public string Payload { get; init; } = string.Empty;
    public string PayloadType { get; init; } = string.Empty;
}

﻿namespace SilentMike.DietMenu.Mailing.Application.Family.Models;

public sealed record ImportedFamilyDataError
{
    public string Code { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}

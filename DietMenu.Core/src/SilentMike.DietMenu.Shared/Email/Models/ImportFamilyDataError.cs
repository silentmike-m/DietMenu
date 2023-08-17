namespace SilentMike.DietMenu.Shared.Email.Models;

public sealed record ImportFamilyDataError
{
    public string Code { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}

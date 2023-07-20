namespace SilentMike.DietMenu.Auth.Application.Families.Commands;

using SilentMike.DietMenu.Auth.Application.Common;

public sealed class CreateFamily : IRequest, ISystemRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
}

namespace SilentMike.DietMenu.Core.Application.Core.Events;

using SilentMike.DietMenu.Core.Application.Core.Models;

public sealed record ImportedCoreData : INotification
{
    public IReadOnlyList<CoreDataImportError> Errors { get; init; } = new List<CoreDataImportError>();
    public bool IsSuccess { get; init; } = default;
}

namespace SilentMike.DietMenu.Core.Domain.Services;

public interface IFamilyRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

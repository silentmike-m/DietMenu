namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IFamilyRepository
{
    Task<FamilyEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(FamilyEntity family, CancellationToken cancellationToken = default);
}

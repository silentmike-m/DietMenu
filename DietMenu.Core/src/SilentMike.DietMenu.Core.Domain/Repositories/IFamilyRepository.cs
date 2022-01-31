namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IFamilyRepository
{
    Task<FamilyEntity?> Get(Guid id, CancellationToken cancellationToken = default);
    Task Save(FamilyEntity family, CancellationToken cancellationToken = default);
}

namespace DietMenu.Core.Domain.Repositories;

using DietMenu.Core.Domain.Entities;

public interface IFamilyRepository
{
    Task<FamilyEntity?> Get(Guid id, CancellationToken cancellationToken = default);
    Task<FamilyEntity?> Get(string name, CancellationToken cancellationToken = default);
    Task Save(FamilyEntity family, CancellationToken cancellationToken = default);
}

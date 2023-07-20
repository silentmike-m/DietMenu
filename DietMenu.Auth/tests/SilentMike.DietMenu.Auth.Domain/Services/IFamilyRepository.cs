namespace SilentMike.DietMenu.Auth.Domain.Services;

using SilentMike.DietMenu.Auth.Domain.Entities;

public interface IFamilyRepository
{
    Task<FamilyEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(FamilyEntity family, CancellationToken cancellationToken = default);
}

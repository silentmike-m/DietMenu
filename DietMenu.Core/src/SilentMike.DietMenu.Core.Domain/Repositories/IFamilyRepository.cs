namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IFamilyRepository : IRepository
{
    bool Exists(Guid id);
    FamilyEntity? Get(Guid id);
    void Save(FamilyEntity family);
}

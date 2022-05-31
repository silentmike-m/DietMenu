namespace SilentMike.DietMenu.Core.Domain.Repositories;

using SilentMike.DietMenu.Core.Domain.Entities;

public interface IFamilyRepository
{
    FamilyEntity? Get(Guid id);
    void Save(FamilyEntity family);
}

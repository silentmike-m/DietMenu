namespace SilentMike.DietMenu.Core.Application.Families.Interfaces;

using SilentMike.DietMenu.Core.Domain.Entities;

internal interface IGetFamilyDataToImport
{
    FamilyEntity Family { get; }
}

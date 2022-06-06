namespace SilentMike.DietMenu.Core.Infrastructure.AutoMapper;

using global::AutoMapper;
using SourceIngredientType = SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models.IngredientTypeRow;
using TargetIngredientType = SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.IngredientType;

internal sealed class IngredientTypeRowProfile : Profile
{
    public IngredientTypeRowProfile()
    {
        this.CreateMap<SourceIngredientType, TargetIngredientType>()
            .ForMember(target => target.Id,
                opt => opt.MapFrom(source => source.Id))
            .ForMember(target => target.Name,
                opt => opt.MapFrom(source => source.Name))
            ;
    }
}

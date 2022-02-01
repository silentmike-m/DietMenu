namespace SilentMike.DietMenu.Core.Infrastructure.AutoMapper;

using global::AutoMapper;
using SourceIngredientType = SilentMike.DietMenu.Core.Domain.Entities.IngredientTypeEntity;
using TargetIngredientType = SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels.IngredientType;

internal sealed class IngredientTypeProfile : Profile
{
    public IngredientTypeProfile()
    {
        this.CreateMap<SourceIngredientType, TargetIngredientType>()
            .ForMember(target => target.Id, options => options.MapFrom(source => source.Id))
            .ForMember(target => target.Name, options => options.MapFrom(source => source.Name))
            ;
    }
}

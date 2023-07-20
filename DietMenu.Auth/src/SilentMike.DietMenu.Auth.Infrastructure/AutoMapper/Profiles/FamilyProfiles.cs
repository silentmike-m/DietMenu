namespace SilentMike.DietMenu.Auth.Infrastructure.AutoMapper.Profiles;

using global::AutoMapper;
using Dto = SilentMike.DietMenu.Auth.Infrastructure.Identity.Models.Family;
using Entity = SilentMike.DietMenu.Auth.Domain.Entities.FamilyEntity;

internal sealed class FamilyProfiles : Profile
{
    public FamilyProfiles()
    {
        this.CreateMap<Dto, Entity>()
            .ForMember(target => target.Id, options => options.MapFrom(source => source.Id))
            .ForMember(target => target.Name, options => options.MapFrom(source => source.Name))
            ;
    }
}

namespace SilentMike.DietMenu.Auth.Infrastructure.AutoMapper.Profiles;

using global::AutoMapper;
using Dto = SilentMike.DietMenu.Auth.Infrastructure.Identity.Models.User;
using Entity = SilentMike.DietMenu.Auth.Domain.Entities.UserEntity;

internal sealed class UserProfiles : Profile
{
    public UserProfiles()
    {
        this.CreateMap<Dto, Entity>()
            .ForMember(target => target.Email, options => options.MapFrom(source => source.Email))
            .ForMember(target => target.FamilyId, options => options.MapFrom(source => source.FamilyId))
            .ForMember(target => target.FirstName, options => options.MapFrom(source => source.FirstName))
            .ForMember(target => target.Id, options => options.MapFrom(source => source.Id))
            .ForMember(target => target.LastName, options => options.MapFrom(source => source.LastName))
            ;
    }
}

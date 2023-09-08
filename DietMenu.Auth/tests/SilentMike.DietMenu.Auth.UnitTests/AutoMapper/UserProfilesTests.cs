namespace SilentMike.DietMenu.Auth.UnitTests.AutoMapper;

using global::AutoMapper;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Enums;
using SilentMike.DietMenu.Auth.Infrastructure.AutoMapper.Profiles;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

[TestClass]
public sealed class UserProfilesTests
{
    private static readonly User DTO_USER = new()
    {
        Email = "johnwick@domain.com",
        Family = new Family
        {
            Id = Guid.NewGuid(),
        },
        FamilyId = Guid.NewGuid(),
        FirstName = "John",
        Id = "18ff1b8b-0f85-4b37-b5d3-073f82f52431",
        LastLogin = DateTime.Now,
        LastName = "Wick",
        Role = UserRole.System,
    };

    private readonly IMapper mapper;

    public UserProfilesTests()
    {
        var config = new MapperConfiguration(config => config.AddProfile<UserProfiles>());
        this.mapper = config.CreateMapper();
    }

    [TestMethod]
    public void Should_Map_Dto_To_Entity()
    {
        //GIVEN

        //WHEN
        var result = this.mapper.Map<UserEntity>(DTO_USER);

        //THEN
        var expectedResult = new UserEntity(DTO_USER.Email, DTO_USER.FamilyId, DTO_USER.FirstName, DTO_USER.LastName, new Guid(DTO_USER.Id), DTO_USER.Role);

        result.Should()
            .BeEquivalentTo(expectedResult)
            ;
    }
}

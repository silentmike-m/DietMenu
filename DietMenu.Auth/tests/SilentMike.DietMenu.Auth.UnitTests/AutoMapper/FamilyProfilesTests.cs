﻿namespace SilentMike.DietMenu.Auth.UnitTests.AutoMapper;

using global::AutoMapper;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Infrastructure.AutoMapper.Profiles;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

[TestClass]
public sealed class FamilyProfilesTests
{
    private static Family DTO_FAMILY = new()
    {
        Email = "family@domain.com",
        Id = Guid.NewGuid(),
        Key = 2,
        Name = "dto family name",
    };

    private readonly IMapper mapper;

    public FamilyProfilesTests()
    {
        var config = new MapperConfiguration(config => config.AddProfile<FamilyProfiles>());
        this.mapper = config.CreateMapper();
    }

    [TestMethod]
    public void Should_Map_Dto_To_Entity()
    {
        //GIVEN

        //WHEN
        var result = this.mapper.Map<FamilyEntity>(DTO_FAMILY);

        //THEN
        var expectedResult = new FamilyEntity(DTO_FAMILY.Id, DTO_FAMILY.Email, DTO_FAMILY.Name);

        result.Should()
            .BeEquivalentTo(expectedResult)
            ;
    }
}

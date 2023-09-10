namespace SilentMike.DietMenu.Core.Application.Tests.Families.Queries;

using SilentMike.DietMenu.Core.Application.Common.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;

[TestClass]
public sealed class GetIngredientTypesTests
{
    [TestMethod]
    public void FamilyId_Should_Return_Id_From_AuthData()
    {
        //GIVEN
        var request = new GetIngredientTypes
        {
            AuthData = new AuthData
            {
                FamilyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            },
        };

        //WHEN
        var familyId = request.FamilyId;

        //THEN
        familyId.Should()
            .Be(request.AuthData.FamilyId);
    }
}

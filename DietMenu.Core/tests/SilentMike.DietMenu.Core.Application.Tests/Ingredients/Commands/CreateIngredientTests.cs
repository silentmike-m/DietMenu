namespace SilentMike.DietMenu.Core.Application.Tests.Ingredients.Commands;

using SilentMike.DietMenu.Core.Application.Common.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;

[TestClass]
public sealed class CreateIngredientTests
{
    [TestMethod]
    public void Should_Return_Family_Id_From_Auth_Data()
    {
        //GIVEN
        var request = new CreateIngredient
        {
            AuthData = new AuthData
            {
                FamilyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            },
        };

        //WHEN
        var result = request.FamilyId;

        //THEN
        result.Should()
            .Be(request.AuthData.FamilyId)
            ;
    }
}

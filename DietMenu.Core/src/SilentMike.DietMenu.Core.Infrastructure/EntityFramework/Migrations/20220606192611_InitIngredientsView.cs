using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Migrations
{
    public partial class InitIngredientsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW IngredientRows AS
                SELECT Ingredient.Id
                    , Ingredient.Exchanger
                    , Ingredient.FamilyId
                    , Ingredient.IsActive
                    , Ingredient.Name
                    , Ingredient.TypeId
                    , Type.Name TypeName
                    , Ingredient.UnitSymbol
                FROM SilentMike.Ingredients Ingredient
                JOIN SilentMike.IngredientTypes Type ON Type.Id = Ingredient.TypeId
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IngredientRows");
        }
    }
}

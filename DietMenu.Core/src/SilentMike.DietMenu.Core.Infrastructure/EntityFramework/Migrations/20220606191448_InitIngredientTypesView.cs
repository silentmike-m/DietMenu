using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Migrations
{
    public partial class InitIngredientTypesView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW IngredientTypeRows AS
                SELECT Id
                    , FamilyId
                    , IsActive
                    , Name
                FROM SilentMike.IngredientTypes
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IngredientTypeRows");
        }
    }
}

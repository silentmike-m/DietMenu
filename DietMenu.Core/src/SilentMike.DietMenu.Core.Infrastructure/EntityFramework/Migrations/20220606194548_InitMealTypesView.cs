using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Migrations
{
    public partial class InitMealTypesView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW MealTypeRows AS
                SELECT Id
                    , FamilyId
                    , IsActive
                    , Name
                    , Order
                FROM SilentMike.MealTypes
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW MealTypeRows");
        }
    }
}

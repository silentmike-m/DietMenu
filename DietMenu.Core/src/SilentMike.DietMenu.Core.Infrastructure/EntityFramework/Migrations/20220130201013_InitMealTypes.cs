using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Migrations
{
    public partial class InitMealTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MealTypes",
                schema: "SilentMike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternalName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealTypes_Families_FamilyEntityId",
                        column: x => x.MealTypeId,
                        principalSchema: "SilentMike",
                        principalTable: "Families",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_FamilyEntityId",
                schema: "SilentMike",
                table: "MealTypes",
                column: "FamilyEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_InternalName",
                schema: "SilentMike",
                table: "MealTypes",
                column: "InternalName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_MealTypeId",
                schema: "SilentMike",
                table: "MealTypes",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_Name",
                schema: "SilentMike",
                table: "MealTypes",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealTypes",
                schema: "SilentMike");
        }
    }
}

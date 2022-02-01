using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SilentMike");

            migrationBuilder.CreateTable(
                name: "Families",
                schema: "SilentMike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealTypes",
                schema: "SilentMike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternalName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealTypes_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalSchema: "SilentMike",
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_FamilyId",
                schema: "SilentMike",
                table: "MealTypes",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_Id_InternalName",
                schema: "SilentMike",
                table: "MealTypes",
                columns: new[] { "Id", "InternalName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_Id_Name",
                schema: "SilentMike",
                table: "MealTypes",
                columns: new[] { "Id", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealTypes",
                schema: "SilentMike");

            migrationBuilder.DropTable(
                name: "Families",
                schema: "SilentMike");
        }
    }
}

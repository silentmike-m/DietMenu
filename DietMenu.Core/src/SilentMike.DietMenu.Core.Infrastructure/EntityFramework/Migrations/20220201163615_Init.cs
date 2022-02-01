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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IngredientTypes",
                schema: "SilentMike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternalName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngredientTypes_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalSchema: "SilentMike",
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Ingredients",
                schema: "SilentMike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Exchanger = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternalName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalSchema: "SilentMike",
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingredients_IngredientTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "SilentMike",
                        principalTable: "IngredientTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_FamilyId_InternalName",
                schema: "SilentMike",
                table: "Ingredients",
                columns: new[] { "FamilyId", "InternalName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_FamilyId_Name",
                schema: "SilentMike",
                table: "Ingredients",
                columns: new[] { "FamilyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_TypeId",
                schema: "SilentMike",
                table: "Ingredients",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientTypes_FamilyId_InternalName",
                schema: "SilentMike",
                table: "IngredientTypes",
                columns: new[] { "FamilyId", "InternalName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngredientTypes_FamilyId_Name",
                schema: "SilentMike",
                table: "IngredientTypes",
                columns: new[] { "FamilyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_FamilyId_InternalName",
                schema: "SilentMike",
                table: "MealTypes",
                columns: new[] { "FamilyId", "InternalName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_FamilyId_Name",
                schema: "SilentMike",
                table: "MealTypes",
                columns: new[] { "FamilyId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients",
                schema: "SilentMike");

            migrationBuilder.DropTable(
                name: "MealTypes",
                schema: "SilentMike");

            migrationBuilder.DropTable(
                name: "IngredientTypes",
                schema: "SilentMike");

            migrationBuilder.DropTable(
                name: "Families",
                schema: "SilentMike");
        }
    }
}

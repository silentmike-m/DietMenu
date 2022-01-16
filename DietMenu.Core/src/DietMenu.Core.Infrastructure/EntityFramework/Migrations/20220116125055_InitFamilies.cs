using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietMenu.Core.Infrastructure.EntityFramework.Migrations
{
    public partial class InitFamilies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                schema: "SilentMike",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Identifier);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Families_Id",
                schema: "SilentMike",
                table: "Families",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_Name",
                schema: "SilentMike",
                table: "Families",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Families",
                schema: "SilentMike");
        }
    }
}

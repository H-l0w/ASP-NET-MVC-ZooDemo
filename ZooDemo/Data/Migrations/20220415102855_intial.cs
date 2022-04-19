using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZooDemo.Data.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbAnimalType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbAnimalType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbPavilon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbPavilon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbAnimal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdPavilon = table.Column<int>(type: "int", nullable: false),
                    IdAnimalType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbAnimal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbAnimal_tbAnimalType_IdAnimalType",
                        column: x => x.IdAnimalType,
                        principalTable: "tbAnimalType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbAnimal_tbPavilon_IdPavilon",
                        column: x => x.IdPavilon,
                        principalTable: "tbPavilon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbAnimal_IdAnimalType",
                table: "tbAnimal",
                column: "IdAnimalType");

            migrationBuilder.CreateIndex(
                name: "IX_tbAnimal_IdPavilon",
                table: "tbAnimal",
                column: "IdPavilon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbAnimal");

            migrationBuilder.DropTable(
                name: "tbAnimalType");

            migrationBuilder.DropTable(
                name: "tbPavilon");
        }
    }
}

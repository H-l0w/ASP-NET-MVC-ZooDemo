using Microsoft.EntityFrameworkCore.Migrations;

namespace ZooDemo.Data.Migrations
{
    public partial class animalnameadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnimalName",
                table: "tbAnimal",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalName",
                table: "tbAnimal");
        }
    }
}

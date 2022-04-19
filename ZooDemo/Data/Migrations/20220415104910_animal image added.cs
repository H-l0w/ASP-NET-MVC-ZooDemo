using Microsoft.EntityFrameworkCore.Migrations;

namespace ZooDemo.Data.Migrations
{
    public partial class animalimageadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "tbAnimal",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "tbAnimal");
        }
    }
}

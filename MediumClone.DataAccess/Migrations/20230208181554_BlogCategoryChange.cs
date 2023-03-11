using Microsoft.EntityFrameworkCore.Migrations;

namespace MediumClone.DataAccess.Migrations
{
    public partial class BlogCategoryChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BlogCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "BlogCategories");
        }
    }
}

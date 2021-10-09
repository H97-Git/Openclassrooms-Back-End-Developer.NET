using Microsoft.EntityFrameworkCore.Migrations;

namespace The_Car_Hub.Data.Migrations
{
    public partial class InventoryDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Inventory",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Inventory");
        }
    }
}

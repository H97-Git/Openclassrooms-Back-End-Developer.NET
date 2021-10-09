using Microsoft.EntityFrameworkCore.Migrations;

namespace The_Car_Hub.Data.Migrations
{
    public partial class InventoryTypeFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Inventory");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Inventory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Inventory");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Inventory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

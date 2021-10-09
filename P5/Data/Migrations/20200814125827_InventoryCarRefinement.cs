using Microsoft.EntityFrameworkCore.Migrations;

namespace The_Car_Hub.Data.Migrations
{
    public partial class InventoryCarRefinement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Inventory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Car",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EngineSize",
                table: "Car",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Fuel",
                table: "Car",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gearbox",
                table: "Car",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Seats",
                table: "Car",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "EngineSize",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Fuel",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Gearbox",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Seats",
                table: "Car");
        }
    }
}

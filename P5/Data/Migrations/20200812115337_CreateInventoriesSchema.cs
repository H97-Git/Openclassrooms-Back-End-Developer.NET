using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace The_Car_Hub.Data.Migrations
{
    public partial class CreateInventoriesSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Make = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Trim = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VIN = table.Column<string>(nullable: true),
                    Kilometer = table.Column<int>(nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "Date", nullable: false),
                    PurchasePrice = table.Column<int>(nullable: false),
                    Repairs = table.Column<string>(nullable: true),
                    RepairsCost = table.Column<int>(nullable: true),
                    LotDate = table.Column<DateTime>(type: "Date", nullable: true),
                    SellingPrice = table.Column<int>(nullable: true),
                    SaleDate = table.Column<DateTime>(type: "Date", nullable: true),
                    CarId = table.Column<int>(nullable: false),
                    InventoryStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventory_InventoryStatus_InventoryStatusId",
                        column: x => x.InventoryStatusId,
                        principalTable: "InventoryStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    InventoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Media_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_CarId",
                table: "Inventory",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_InventoryStatusId",
                table: "Inventory",
                column: "InventoryStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Media_InventoryId",
                table: "Media",
                column: "InventoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "InventoryStatus");
        }
    }
}

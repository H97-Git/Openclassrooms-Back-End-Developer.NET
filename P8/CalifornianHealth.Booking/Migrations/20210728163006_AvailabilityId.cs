using Microsoft.EntityFrameworkCore.Migrations;

namespace CalifornianHealth.Booking.Migrations
{
    public partial class AvailabilityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Bookings");
        }
    }
}

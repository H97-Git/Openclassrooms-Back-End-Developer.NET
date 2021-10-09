using Microsoft.EntityFrameworkCore.Migrations;

namespace CalifornianHealth.Demographics.Migrations
{
    public partial class TypoFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Speciality",
                table: "Consultants",
                newName: "Specialty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialty",
                table: "Consultants",
                newName: "Speciality");
        }
    }
}

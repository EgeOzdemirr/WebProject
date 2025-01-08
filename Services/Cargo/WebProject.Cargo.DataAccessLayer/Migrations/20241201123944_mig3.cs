using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProject.Cargo.DataAccessLayer.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "CargoCustomers",
                newName: "SurName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "CargoCustomers",
                newName: "Surname");
        }
    }
}

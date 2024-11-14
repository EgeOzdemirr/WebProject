using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProject.Order.Persistence.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detail1",
                table: "Adresses");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Adresses",
                newName: "AddressLine2");

            migrationBuilder.RenameColumn(
                name: "Detail2",
                table: "Adresses",
                newName: "AddressLine1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressLine2",
                table: "Adresses",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                table: "Adresses",
                newName: "Detail2");

            migrationBuilder.AddColumn<string>(
                name: "Detail1",
                table: "Adresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

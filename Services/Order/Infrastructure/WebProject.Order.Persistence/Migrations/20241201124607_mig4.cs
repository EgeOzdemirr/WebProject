using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProject.Order.Persistence.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressLine2",
                table: "Adresses",
                newName: "Detail2");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                table: "Adresses",
                newName: "Detail1");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Adresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsInvoice",
                table: "Adresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Isdefault",
                table: "Adresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "IsInvoice",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "Isdefault",
                table: "Adresses");

            migrationBuilder.RenameColumn(
                name: "Detail2",
                table: "Adresses",
                newName: "AddressLine2");

            migrationBuilder.RenameColumn(
                name: "Detail1",
                table: "Adresses",
                newName: "AddressLine1");
        }
    }
}

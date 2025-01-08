using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProject.Comment.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Review",
                table: "UserComments",
                newName: "CommentDetail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentDetail",
                table: "UserComments",
                newName: "Review");
        }
    }
}

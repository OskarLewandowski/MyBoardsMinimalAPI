using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoardsMinimalAPI.Migrations
{
    public partial class FixNamesModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Comments",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "PostaCode",
                table: "Addresses",
                newName: "PostalCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Comments",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Addresses",
                newName: "PostaCode");
        }
    }
}

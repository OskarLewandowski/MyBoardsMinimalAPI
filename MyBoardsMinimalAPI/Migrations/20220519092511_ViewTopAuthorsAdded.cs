using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoardsMinimalAPI.Migrations
{
    public partial class ViewTopAuthorsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW View_TopAuthors AS 
            SELECT TOP 5 u.FullName, COUNT(*) AS [WorkItemsCreated]
            FROM Users AS u
            JOIN WorkItems AS wi ON wi.AuthorId = u.Id
            GROUP BY u.FullName
            ORDER BY [WorkItemsCreated] DESC
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW View_TopAuthors 
            ");
        }
    }
}

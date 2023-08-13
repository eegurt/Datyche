using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datyche.Migrations
{
    /// <inheritdoc />
    public partial class RenameFilePathToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Files",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "Path");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameOwnerColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Projects",
                newName: "ProjectOwnerId");

            migrationBuilder.RenameColumn(
                name: "OwnerName",
                table: "Projects",
                newName: "ProjectOwnerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectOwnerId",
                table: "Projects",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "ProjectOwnerName",
                table: "Projects",
                newName: "OwnerName");
        }
    }
}

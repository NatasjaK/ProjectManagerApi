using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_20250906 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectOwnerName",
                table: "Projects",
                newName: "OwnerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerName",
                table: "Projects",
                newName: "ProjectOwnerName");
        }
    }
}

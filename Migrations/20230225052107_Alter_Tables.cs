using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDocs.API.Migrations
{
    public partial class Alter_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServerStorageBasePath",
                table: "Settings",
                newName: "StorageBasePath");

            migrationBuilder.RenameColumn(
                name: "DocumentLifetime",
                table: "Settings",
                newName: "RetentionDays");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorageBasePath",
                table: "Settings",
                newName: "ServerStorageBasePath");

            migrationBuilder.RenameColumn(
                name: "RetentionDays",
                table: "Settings",
                newName: "DocumentLifetime");
        }
    }
}

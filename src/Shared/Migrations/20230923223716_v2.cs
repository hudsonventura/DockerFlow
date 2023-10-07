using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "logs",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "System",
                table: "logs",
                newName: "system");

            migrationBuilder.RenameColumn(
                name: "Log",
                table: "logs",
                newName: "info");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "logs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "system",
                table: "logs",
                newName: "System");

            migrationBuilder.RenameColumn(
                name: "info",
                table: "logs",
                newName: "Log");
        }
    }
}

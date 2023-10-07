using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "execution_id",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "task_id",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "task_name",
                table: "logs");

            migrationBuilder.CreateTable(
                name: "system_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    execution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_name = table.Column<string>(type: "text", nullable: false),
                    container_id = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    info = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_logs", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "system_logs");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "logs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "execution_id",
                table: "logs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "task_id",
                table: "logs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "task_name",
                table: "logs",
                type: "text",
                nullable: true);
        }
    }
}

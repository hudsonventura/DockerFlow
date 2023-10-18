using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "system_logs");

            migrationBuilder.DropColumn(
                name: "system",
                table: "logs");

            migrationBuilder.AlterColumn<Guid>(
                name: "execution_id",
                table: "logs",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "logs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "task_id",
                table: "logs",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "task_id",
                table: "logs");

            migrationBuilder.AlterColumn<Guid>(
                name: "execution_id",
                table: "logs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "system",
                table: "logs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "system_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    container_id = table.Column<string>(type: "text", nullable: false),
                    execution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    info = table.Column<string>(type: "text", nullable: false),
                    system = table.Column<bool>(type: "boolean", nullable: false),
                    task_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_logs", x => x.id);
                });
        }
    }
}

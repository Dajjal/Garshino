using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGraphApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auto_graph_drivers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    driver_string_id = table.Column<string>(type: "text", nullable: false),
                    driver_full_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auto_graph_drivers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "auto_graph_machines",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    machine_id = table.Column<Guid>(type: "uuid", nullable: false),
                    machine_name = table.Column<string>(type: "text", nullable: false),
                    machine_reg_number = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auto_graph_machines", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "auto_graph_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    telegram_chat_id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auto_graph_users", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auto_graph_drivers");

            migrationBuilder.DropTable(
                name: "auto_graph_machines");

            migrationBuilder.DropTable(
                name: "auto_graph_users");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGraphApi.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auto_graph_schemas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    schema_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auto_graph_schemas", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auto_graph_schemas");
        }
    }
}

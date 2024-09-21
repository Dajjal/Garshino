using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGraphApi.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "schema_name",
                table: "auto_graph_schemas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "schema_name",
                table: "auto_graph_schemas");
        }
    }
}

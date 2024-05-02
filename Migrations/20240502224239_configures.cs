using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndexCardBackendApi.Migrations
{
    /// <inheritdoc />
    public partial class configures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "length",
                table: "Decks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "length",
                table: "Decks");
        }
    }
}

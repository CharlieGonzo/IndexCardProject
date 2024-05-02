using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndexCardBackendApi.Migrations
{
    /// <inheritdoc />
    public partial class configure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardDeck_Users_UserId",
                table: "CardDeck");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardDeck",
                table: "CardDeck");

            migrationBuilder.RenameTable(
                name: "CardDeck",
                newName: "Decks");

            migrationBuilder.RenameIndex(
                name: "IX_CardDeck_UserId",
                table: "Decks",
                newName: "IX_Decks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Decks",
                table: "Decks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_UserId",
                table: "Decks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_UserId",
                table: "Decks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decks",
                table: "Decks");

            migrationBuilder.RenameTable(
                name: "Decks",
                newName: "CardDeck");

            migrationBuilder.RenameIndex(
                name: "IX_Decks_UserId",
                table: "CardDeck",
                newName: "IX_CardDeck_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardDeck",
                table: "CardDeck",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardDeck_Users_UserId",
                table: "CardDeck",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

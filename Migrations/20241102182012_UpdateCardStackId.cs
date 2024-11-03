using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashcards.TwilightSaw.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardStackId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_CardStacks_StackId",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "StackId",
                table: "Flashcards",
                newName: "CardStackId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_StackId",
                table: "Flashcards",
                newName: "IX_Flashcards_CardStackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_CardStacks_CardStackId",
                table: "Flashcards",
                column: "CardStackId",
                principalTable: "CardStacks",
                principalColumn: "CardStackId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_CardStacks_CardStackId",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "CardStackId",
                table: "Flashcards",
                newName: "StackId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_CardStackId",
                table: "Flashcards",
                newName: "IX_Flashcards_StackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_CardStacks_StackId",
                table: "Flashcards",
                column: "StackId",
                principalTable: "CardStacks",
                principalColumn: "CardStackId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

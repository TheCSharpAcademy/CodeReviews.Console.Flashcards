using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterAlternateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Flashcards_Question",
                table: "Flashcards");

            migrationBuilder.AlterColumn<int>(
                name: "StackID",
                table: "FlashcardsDTO",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_Question",
                table: "Flashcards",
                column: "Question",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Flashcards_Question",
                table: "Flashcards");

            migrationBuilder.AlterColumn<int>(
                name: "StackID",
                table: "FlashcardsDTO",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Flashcards_Question",
                table: "Flashcards",
                column: "Question");
        }
    }
}

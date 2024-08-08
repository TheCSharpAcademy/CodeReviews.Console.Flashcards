using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashcards.Migrations {
    /// <inheritdoc />
    public partial class FlashcardNameUnique : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "ScorePercentage",
                table: "StudySessions");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "Flashcards",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_Question",
                table: "Flashcards",
                column: "Question",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropIndex(
                name: "IX_Flashcards_Question",
                table: "Flashcards");

            migrationBuilder.AddColumn<double>(
                name: "ScorePercentage",
                table: "StudySessions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "Flashcards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

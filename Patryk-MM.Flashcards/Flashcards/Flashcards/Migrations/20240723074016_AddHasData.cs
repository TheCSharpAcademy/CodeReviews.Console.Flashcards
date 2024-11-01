using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Flashcards.Migrations {
    /// <inheritdoc />
    public partial class AddHasData : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.InsertData(
                table: "Stacks",
                columns: new[] { "StackId", "Name" },
                values: new object[] { 1, "Questions" });

            migrationBuilder.InsertData(
                table: "Flashcards",
                columns: new[] { "Id", "Answer", "Question", "StackId" },
                values: new object[,]
                {
                    { 1, "Test 1", "Test 1", 1 },
                    { 2, "Test 2", "Test 2", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stacks",
                keyColumn: "StackId",
                keyValue: 1);
        }
    }
}

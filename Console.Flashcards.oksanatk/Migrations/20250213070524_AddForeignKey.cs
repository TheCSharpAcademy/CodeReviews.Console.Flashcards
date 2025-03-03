using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetMAUI.Flashcards.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_StackId",
                table: "StudySessions",
                column: "StackId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudySessions_Stacks_StackId",
                table: "StudySessions",
                column: "StackId",
                principalTable: "Stacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_Stacks_StackId",
                table: "StudySessions");

            migrationBuilder.DropIndex(
                name: "IX_StudySessions_StackId",
                table: "StudySessions");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashcards.TwilightSaw.Migrations
{
    /// <inheritdoc />
    public partial class UniqueName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CardStacks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CardStacks_Name",
                table: "CardStacks",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CardStacks_Name",
                table: "CardStacks");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CardStacks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

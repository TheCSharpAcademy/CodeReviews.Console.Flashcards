using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetMAUI.Flashcards.Migrations
{
    /// <inheritdoc />
    public partial class DisplayName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayNum",
                table: "Flashcards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayNum",
                table: "Flashcards");
        }
    }
}

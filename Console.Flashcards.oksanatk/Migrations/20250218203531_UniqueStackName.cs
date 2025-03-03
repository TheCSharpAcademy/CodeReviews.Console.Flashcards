using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetMAUI.Flashcards.Migrations
{
    /// <inheritdoc />
    public partial class UniqueStackName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stacks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Stacks_Name",
                table: "Stacks",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stacks_Name",
                table: "Stacks");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stacks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterOnDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studies_Stacks_StackID",
                table: "Studies");

            migrationBuilder.AddForeignKey(
                name: "FK_Studies_Stacks_StackID",
                table: "Studies",
                column: "StackID",
                principalTable: "Stacks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studies_Stacks_StackID",
                table: "Studies");

            migrationBuilder.AddForeignKey(
                name: "FK_Studies_Stacks_StackID",
                table: "Studies",
                column: "StackID",
                principalTable: "Stacks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

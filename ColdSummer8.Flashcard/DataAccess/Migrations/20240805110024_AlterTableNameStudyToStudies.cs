using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableNameStudyToStudies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Study_Stacks_StackID",
                table: "Study");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Study",
                table: "Study");

            migrationBuilder.RenameTable(
                name: "Study",
                newName: "Studies");

            migrationBuilder.RenameIndex(
                name: "IX_Study_StackID",
                table: "Studies",
                newName: "IX_Studies_StackID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Studies",
                table: "Studies",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Studies_Stacks_StackID",
                table: "Studies",
                column: "StackID",
                principalTable: "Stacks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studies_Stacks_StackID",
                table: "Studies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Studies",
                table: "Studies");

            migrationBuilder.RenameTable(
                name: "Studies",
                newName: "Study");

            migrationBuilder.RenameIndex(
                name: "IX_Studies_StackID",
                table: "Study",
                newName: "IX_Study_StackID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Study",
                table: "Study",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Study_Stacks_StackID",
                table: "Study",
                column: "StackID",
                principalTable: "Stacks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

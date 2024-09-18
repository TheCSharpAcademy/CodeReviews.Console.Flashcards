using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashcards.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stack",
                columns: table => new
                {
                    StackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StackName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stack", x => x.StackId);
                });

            migrationBuilder.CreateTable(
                name: "Flashcard",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackId = table.Column<int>(type: "int", nullable: false),
                    StackCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcard", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Flashcard_Stack_StackId",
                        column: x => x.StackId,
                        principalTable: "Stack",
                        principalColumn: "StackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudySession",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StackName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySession", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_StudySession_Stack_StackId",
                        column: x => x.StackId,
                        principalTable: "Stack",
                        principalColumn: "StackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_Question",
                table: "Flashcard",
                column: "Question",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_StackId",
                table: "Flashcard",
                column: "StackId");

            migrationBuilder.CreateIndex(
                name: "IX_Stack_StackName",
                table: "Stack",
                column: "StackName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudySession_StackId",
                table: "StudySession",
                column: "StackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcard");

            migrationBuilder.DropTable(
                name: "StudySession");

            migrationBuilder.DropTable(
                name: "Stack");
        }
    }
}

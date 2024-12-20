using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashCards.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "creationTime",
                table: "Flashcards",
                newName: "CreationTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "StudySessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeReviewed",
                table: "Flashcards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextTimeToReview",
                table: "Flashcards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewBreakInSeconds",
                table: "Flashcards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "StudySessions");

            migrationBuilder.DropColumn(
                name: "LastTimeReviewed",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "NextTimeToReview",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "ReviewBreakInSeconds",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Flashcards",
                newName: "creationTime");
        }
    }
}

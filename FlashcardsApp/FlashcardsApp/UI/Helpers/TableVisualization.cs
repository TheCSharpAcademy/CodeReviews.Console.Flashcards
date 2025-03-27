using Spectre.Console;
using FlashcardsApp.Models;
using FlashcardsApp.DTOs;
using FlashcardsApp.Services;

namespace FlashcardsApp.UI.Helpers
{
    internal class TableVisualization
    {
        internal static void ShowStacksTable(List<Stack> stacks)
        {
            Console.WriteLine("\n");

            var table = new Table();

            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Description");
            table.AddColumn("Date Created");

            foreach (var stack in stacks)
            {
                table.AddRow(
                    stack.StackId.ToString(),
                    stack.Name,
                    stack.Description,
                    stack.CreatedDate.ToString()
                    );
            }

            table.Border(TableBorder.Square);
            table.BorderColor(Color.Blue);

            AnsiConsole.Write(table);
            Console.WriteLine("\n");
        }

        internal static void ShowFlashcardsTable(List<FlashcardDTO> flashcards)
        {
            Console.WriteLine("\n");

            var table = new Table();

            table.AddColumn("ID");
            table.AddColumn("Front");
            table.AddColumn("Back");
            table.AddColumn("Date Created");

            foreach (var card in flashcards)
            {
                var idPoperty = typeof(FlashcardDTO).GetProperty("DisplayNumber");
                var frontProperty = typeof(FlashcardDTO).GetProperty("Front");
                var backProperty = typeof(FlashcardDTO).GetProperty("Back");
                var createdDateProperty = typeof(FlashcardDTO).GetProperty("CreatedDate");

                table.AddRow(
                    idPoperty?.GetValue(card)?.ToString() ?? "NULL",
                    frontProperty?.GetValue(card)?.ToString() ?? "NULL",
                    backProperty?.GetValue(card)?.ToString() ?? "NULL",
                    createdDateProperty?.GetValue(card)?.ToString() ?? "NULL"
                    );
            }

            table.Border(TableBorder.Square);
            table.BorderColor(Color.Green);

            AnsiConsole.Write(table);
            Console.WriteLine("\n");
        }

        internal static void ShowStudySessionsTable(List<StudySession> sessions, List<Stack> stacks)
        {
            Console.Write("\n");

            Dictionary<int, string> stackDictionary = stacks.ToDictionary(s => s.StackId, s => s.Name);

            var table = new Table();

            table.AddColumn("Stack Studied");
            table.AddColumn("Score");
            table.AddColumn("Study Date");

            foreach (var session in sessions)
            {
                string stackName = stackDictionary.GetValueOrDefault(session.StackId, "NULL");
                
                table.AddRow(
                    stackName,
                    session.Score?.ToString() ?? "NULL",
                    session.StudyDate.ToString()
                    );
            }

            table.Border(TableBorder.Square);
            table.BorderColor(Color.Orange1);

            AnsiConsole.Write(table);
            Console.WriteLine("\n");
        }
    }
}

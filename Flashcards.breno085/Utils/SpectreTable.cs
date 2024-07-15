using Spectre.Console;
using flashcards.Models;

namespace flashcards.Utils
{
    public class SpectreTable
    {
        public static void StackTable(IEnumerable<Stacks> stackData)
        {
            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Name");

            // int displayId = 1;
            foreach (var stack in stackData)
            {
                table.AddRow(
                    // displayId.ToString(),
                    stack.Id.ToString(),
                    stack.LanguageName
                );
                // displayId++;
            }
            AnsiConsole.Write(table);
        }

        public static void FlashcardsTable(IEnumerable<Flashcards> flashcards)
        {
            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Front");
            table.AddColumn("Back");
            
            int displayId = 1;
            foreach (var flashcard in flashcards)
            {
                table.AddRow(
                    // flashcard.Id.ToString(),
                    displayId.ToString(),
                    flashcard.Front,
                    flashcard.Back
                );
                displayId++;
            }
            AnsiConsole.Write(table);
        }


        public static void FlashcardsFrontTable(IEnumerable<Flashcards> flashcards)
        {
            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Front");

            int displayId = 1;
            foreach (var flashcard in flashcards)
            {
                table.AddRow(
                    // flashcard.Id.ToString(),
                    displayId.ToString(),
                    flashcard.Front
                );
                displayId++;
            }
            AnsiConsole.Write(table);
        }

        public static void StudyTable(IEnumerable<Study> study)
        {
            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Date");
            table.AddColumn("Score");
            table.AddColumn("StackId");

            foreach (var data in study)
            {
                table.AddRow(
                    data.Id.ToString(),
                    data.Date.ToString("dd/MM/yyyy HH:mm"),
                    data.Score.ToString(),
                    data.StackId.ToString()
                );
            }
            AnsiConsole.Write(table);
        }
    }
}
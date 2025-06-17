using Flashcards.DTOs;
using Spectre.Console;
using static Flashcards.Enums;

namespace Flashcards
{
    internal static class UI
    {

        internal static string StackSelector(List<stackDto> dtoResults)
        {

            ShowStacks(dtoResults);

            string stackName = AnsiConsole.Prompt(new TextPrompt<string>("Enter the name of the stack you want to select: "));

            while (!Validator.StackValidator(stackName, dtoResults))
            {
                stackName = AnsiConsole.Prompt(new TextPrompt<string>("Invalid Stack Name! Enter again: "));
            }

            return stackName;
        }

        internal static (string, EditType) AskForUpdateValue()
        {

            return (AnsiConsole.Prompt(new TextPrompt<string>("Enter the new value: ")), AnsiConsole.Prompt(new SelectionPrompt<EditType>()
                        .Title("Update Type")
                        .AddChoices(Enum.GetValues<EditType>())));

        }

        internal static int FlashcardSelector(List<FlashcardDto> dtoResults)
        {
            ShowFlashcards(dtoResults);

            int id = AnsiConsole.Prompt(new TextPrompt<int>("Enter the id of the flashcard you want to select: "));

            while (!Validator.FlashcardValidator(id, dtoResults))
            {
                id = AnsiConsole.Prompt(new TextPrompt<int>("Wrong id entered! Enter the id of the flashcard you want to select: "));
            }

            return id;
        }

        internal static void ShowStacks(List<stackDto> stacks)
        {
            Table table = new Table();
            table.ShowRowSeparators();
            table.Title("Stacks");
            table.AddColumn("Name");

            foreach (var result in stacks)
            {
                table.AddRow($"{result.Name}");
            }

            AnsiConsole.Write(table);
        }

        internal static string GetUserAnswer()
        {
            string userAnswer = AnsiConsole.Prompt(new TextPrompt<string>("Enter the answer: "));

            return userAnswer;

        }
        internal static void ShowStudyFlashcard(FlashcardDto flashcard)
        {
            Table table = new();
            table.Title("Flashcard");

            table.ShowRowSeparators();
            table.AddColumn("Front");
            table.AddRow($"{flashcard.Front}");

            AnsiConsole.Write(table);
        }
        internal static void ShowStudySessions(List<StudySessionDto> studySessions)
        {
            Table table = new();
            table.ShowRowSeparators();
            table.Title("Study Sessions");
            table.AddColumn("ID");
            table.AddColumn("Date");
            table.AddColumn("Stack Name");
            table.AddColumn("Percentage");

            foreach (var session in studySessions)
            {
                table.AddRow(session.ID.ToString(), session.SessionDate.ToShortDateString(), session.StackName, session.Score.ToString());
            }

            AnsiConsole.Write(table);
        }
        internal static void ShowFlashcards(List<FlashcardDto> flashcards, int numberOfFlashcards = -1)
        {
            Table table = new Table();
            table.ShowRowSeparators();
            table.Title("Flashcards");
            table.AddColumn("ID");
            table.AddColumn("Front");
            table.AddColumn("Back");

            foreach (var result in flashcards)
            {
                table.AddRow(result.ID.ToString(), result.Front, result.Back);
                if (numberOfFlashcards == result.ID) break;
            }

            AnsiConsole.Write(table);
        }


        internal static (string, string) CreateFlashcard()
        {
            string front = AnsiConsole.Prompt(new TextPrompt<string>("Enter the content for front of the flashcard: "));
            string back = AnsiConsole.Prompt(new TextPrompt<string>("Enter the content for back of the flashcard: "));

            return (front, back);
        }

        internal static string AddStack()
        {
            string stackName = AnsiConsole.Prompt(new TextPrompt<string>("Enter the name of the stack: "));
            return stackName;
        }
    }
}

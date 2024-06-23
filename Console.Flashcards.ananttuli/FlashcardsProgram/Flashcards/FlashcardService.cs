using FlashcardsProgram.Flashcards;
using Spectre.Console;

namespace Flashcards;

public class FlashcardService
{
    public static void Create()
    {

    }

    public static void Display(string stackName, FlashcardDTO flashcard, int order, bool showBack = false)
    {
        string backContents = showBack ? $"[bold][green]{flashcard.Back}[/][/]" : "";
        string frontContents = !showBack ? $"[blue]{flashcard.Front}[/]" : "";
        string frontOrBack = showBack ? "[green]Answer[/]" : "Question";
        string contents = $"{frontContents}{backContents}";

        var table = new Table();

        table.AddColumn(new TableColumn($"{stackName} {frontOrBack} #{order}").Centered().Width(30));
        table.AddRow(contents);

        AnsiConsole.Write(table);
    }
}
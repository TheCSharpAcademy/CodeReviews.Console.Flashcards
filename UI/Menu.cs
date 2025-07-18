namespace DotNETConsole.Flashcards.UI;

using Spectre.Console;
using Enums;

public class Menu
{
    public MainUI GetChoice()
    {
        var appName = new FigletText("Memomry")
            .Color(Color.Cyan1)
            .Centered();
        AnsiConsole.Write(appName);
        AnsiConsole.Write(new Align(new Markup("-[orange3 italic] retain your memory using flashcard...[/]"), HorizontalAlignment.Center, VerticalAlignment.Top));

        MainUI choice = AnsiConsole.Prompt(new SelectionPrompt<MainUI>()
            .Title("Select [green]options from the menu.[/]?").AddChoices(new List<MainUI>((MainUI[])Enum.GetValues(typeof(MainUI)))));
        return choice;
    }
}

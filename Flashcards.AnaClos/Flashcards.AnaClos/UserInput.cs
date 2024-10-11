using Spectre.Console;
using Flashcards.AnaClos.Models;

namespace Flashcards.AnaClos;

public class UserInput
{
    public string MainMenu(List<string> mainOptions)
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select a [green]function[/]")
            .PageSize(10)
            .AddChoices(mainOptions));

        //return options.IndexOf(selection);
        return selection;
    }
    
    public string GetString(string message)
    {
        return AnsiConsole.Prompt(new TextPrompt<string>($@"[bold blue]{message}: [/]"));
    }
}
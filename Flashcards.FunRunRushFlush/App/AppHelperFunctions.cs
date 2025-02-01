using Spectre.Console;

namespace Flashcards.FunRunRushFlush.App;

public enum SessionMode
{
    All,
    Only_Unsolved,
    Only_Solved
}

public static class AppHelperFunctions
{
    public static void AppHeader(string headerTitel, bool link = false)
    {
        AnsiConsole.Write(new FigletText(headerTitel).Centered().Color(Color.Blue));
        
        if (link)
        {
            AnsiConsole.Write(
             new Markup("[blue]Inspired by the [link=https://thecsharpacademy.com/project/14/flashcards]C#Academy[/][/]")
             .Centered());
        }
        AnsiConsole.MarkupLine("");
    }

    public static bool ReturnMenu()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<bool>($"[yellow]You want to go back?[/]")
            .AddChoice(true)
            .AddChoice(false)
            .DefaultValue(false)
            .WithConverter(choice => choice ? "y" : "n"));
    }
    public static bool CloseApp()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<bool>($"[yellow]Are you sure you want to Close the App[/]")
            .AddChoice(true)
            .AddChoice(false)
            .DefaultValue(false)
            .WithConverter(choice => choice ? "y" : "n"));
    }




}

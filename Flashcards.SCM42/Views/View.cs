using Spectre.Console;

namespace Flashcards;

public class Views
{    
    static Color foregroundColor = ViewStyles.foregroundColor;
    
    internal static void ShowHeader()
    {        
        var font = FigletFont.Load("fonts/ogre.flf.txt");

        AnsiConsole.Write(
            new FigletText(font, "Flashcards")
                .Centered()
                .Color(foregroundColor));

        var rule = new Rule();
        AnsiConsole.Write(rule);
    }

    internal static void ShowWorkingStack(string? stackName)
    {
        AnsiConsole.MarkupInterpolated($"Current working stack: [orange3]{stackName}[/]");
        Console.WriteLine();

        var rule = new Rule();
        AnsiConsole.Write(rule);
    }

    internal static void EmptyTable(string? stackName)
    {
        var panel = new Panel($"[orange3]Stack is empty.[/]");
        panel.Border = ViewStyles.panelBorder;

        AnsiConsole.Write(panel);
    }

    internal static string MainMenu()
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(foregroundColor)
                .Title($"Select an [{foregroundColor}]option[/] from the menu:")
                .PageSize(5)
                .AddChoices(new[] {
                    "Manage Stacks", "Manage Flashcards", "Start Study Session", "View Session Data", "Exit"
                }));

        return selected;
    }

    internal static string SelectYesOrNo()
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(foregroundColor)
                .PageSize(3)
                .AddChoices(new[] {
                    "Yes", "No"
                }));

        return selected;
    }

    // Message display methods
    internal static void ShowErrorMessage(string? message)
    {
        AnsiConsole.MarkupInterpolated($"\n[red]{message}[/]");
    }

    internal static void ShowMessage(string? message)
    {
        AnsiConsole.MarkupInterpolated($"\n[orange3]{message}[/]");
    }
}
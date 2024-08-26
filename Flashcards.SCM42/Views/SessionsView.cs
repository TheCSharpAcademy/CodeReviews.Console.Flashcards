using Spectre.Console;

namespace Flashcards;

public class SessionsView
{
    static Color foregroundColor = ViewStyles.foregroundColor;

    internal static string SessionsMenu()
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(foregroundColor)
                .Title($"Select an [{foregroundColor}]option[/] from the menu:")
                .PageSize(3)
                .AddChoices(new[] {
                    "View All Sessions", "Start Study Session", "Exit"
                }));

        return selected;
    }

    internal static string ReportMenu()
    {
        var selected = AnsiConsole.Prompt(
           new SelectionPrompt<string>()
               .HighlightStyle(foregroundColor)
               .Title($"Select an [{foregroundColor}]option[/] from the menu:")
               .PageSize(3)
               .AddChoices(new[] {
                    "View Amount of Sessions Per Month", "View Average Score Per Session", "Exit"
               }));

        return selected;
    }
}
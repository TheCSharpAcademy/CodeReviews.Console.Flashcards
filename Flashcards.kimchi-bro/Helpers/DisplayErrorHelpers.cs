using Microsoft.Data.SqlClient;
using Spectre.Console;

internal class DisplayErrorHelpers
{
    internal static void SqlError(SqlException ex)
    {
        AnsiConsole.MarkupLine("[red]A database error occurred.[/]");
        AnsiConsole.MarkupLine($"Error number: [yellow]{ex.Number}[/]");
        AnsiConsole.MarkupLine($"Error state: [yellow]{ex.State}[/]");
        AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    internal static void GeneralError(Exception ex)
    {
        AnsiConsole.MarkupLine("[red]An error occurred.[/]");
        AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }
}

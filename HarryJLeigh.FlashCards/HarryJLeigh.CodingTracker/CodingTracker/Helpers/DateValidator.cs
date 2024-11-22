using Spectre.Console;

namespace CodingTracker.Helpers;

public static class DateValidator
{
    public static bool IsValidEndDate(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            AnsiConsole.MarkupLine("[red]Invalid end date. Must be after start date.[/]");
            return false;
        }
        return true;
    }
}
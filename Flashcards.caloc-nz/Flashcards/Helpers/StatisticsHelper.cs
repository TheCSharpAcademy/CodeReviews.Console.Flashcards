using Flashcards.Data;
using Spectre.Console;

namespace Flashcards.Helpers;

public static class StatisticsHelper
{
    // Method to display stack statistics with customization options
    public static void DisplayStackStatistics(
        AppDbContext dbContext,
        int stackId,
        string label,
        int sessionCount,
        double averageScore,
        string stackNameColor = "green",
        string labelColor = "yellow",
        string scoreFormat = "F1")
    {
        var stack = dbContext.Stacks.Find(stackId);

        if (stack == null)
            AnsiConsole.MarkupLine($"[red]Warning: Stack with ID {stackId} not found. Displaying as 'Unknown'.[/]");

        AnsiConsole.MarkupLine($"[{stackNameColor}]Stack:[/] {stack?.Name ?? "Unknown"} " +
                               $"[{labelColor}]| {label} Sessions:[/] {sessionCount} " +
                               $"[{labelColor}]| Avg. Score:[/] {averageScore.ToString(scoreFormat)}%");
    }
}
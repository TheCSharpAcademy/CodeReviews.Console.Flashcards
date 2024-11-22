using Spectre.Console;

namespace FlashCards.Utilities;

public static class ReportExtensions
{
    internal static string GetYear()
    {
        string year = AnsiConsole.Prompt(
            new TextPrompt<string>("Input a year in format [blue]YYYY[/]:")
                .PromptStyle("green") // Style for the input prompt
                .ValidationErrorMessage("[red]Invalid year! Please enter a year in YYYY format.[/]")
                .Validate(year =>
                {
                    // Validate the input
                    return IsYearValid(year)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Year must be in YYYY format and between 1900 and 2100![/]");
                }));
        return year;
    }
    
    private static bool IsYearValid(string year)
    {
        return int.TryParse(year, out int parsedYear) && parsedYear >= 1900 && parsedYear <= 2100;
    }
}
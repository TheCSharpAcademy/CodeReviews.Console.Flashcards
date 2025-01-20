using Spectre.Console;

namespace Flashcards.Helpers;

public static class ValidationHelper
{
    // Validate that an ID is positive
    public static bool ValidateId(int id, string entityName, string errorColor = "red")
    {
        if (id <= 0)
        {
            AnsiConsole.MarkupLine($"[{errorColor}]Error: Invalid {entityName} ID. Please enter a positive number.[/]");
            return false;
        }

        return true;
    }

    // Validate that a text field is non-empty and within max length
    public static bool ValidateString(string text, string fieldName, int maxLength = int.MaxValue,
        string errorColor = "red")
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            AnsiConsole.MarkupLine($"[{errorColor}]Error: {fieldName} cannot be empty or whitespace.[/]");
            return false;
        }

        if (text.Length > maxLength)
        {
            AnsiConsole.MarkupLine($"[{errorColor}]Error: {fieldName} must not exceed {maxLength} characters.[/]");
            return false;
        }

        return true;
    }

    // Validate that a number falls within a specified range
    public static bool ValidateNumberInRange(int number, int minValue, int maxValue, string fieldName,
        string errorColor = "red")
    {
        if (number < minValue || number > maxValue)
        {
            AnsiConsole.MarkupLine($"[{errorColor}]Error: {fieldName} must be between {minValue} and {maxValue}.[/]");
            return false;
        }

        return true;
    }
}
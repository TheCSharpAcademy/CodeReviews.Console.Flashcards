using Flashcards.DTO;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace Flashcards.Services;

internal class Utilities
{
    public static void PrintNewLines(int numOfNewLines)
    {
        for (int i = 0; i < numOfNewLines; i++) { Console.WriteLine(); }
    }

    public static void DisplayPageHeader(string header)
    {
        PrintNewLines(1);
        string headerMessage = $"[bold][mediumpurple2]{header}[/][/]";
        AnsiConsole.MarkupLine(headerMessage);
        PrintNewLines(1);
    }

    public static void DisplayExceptionErrorMessage(string message, string exception)
    {
        PrintNewLines(1);
        string errorMessage = $"[red]{message}[/]\n[bold]{exception}[/]";
        AnsiConsole.MarkupLine(errorMessage);
        PrintNewLines(1);
    }

    public static void DisplayInformationConsoleMessage(string message)
    {
        PrintNewLines(1);
        string errorMessage = $"[dim][olive]{message}[/][/]\n";
        AnsiConsole.MarkupLine(errorMessage);
        PrintNewLines(1);
    }

    public static string SplitCamelCase(string input)
    {
        return Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
    }

    public static void DisplaySuccessMessage(string message)
    {
        PrintNewLines(1);
        string successMessage = $"[chartreuse1]{message}[/]";
        AnsiConsole.MarkupLine(successMessage);
        PrintNewLines(1);
    }

    public static void DisplayWarningMessage(string message)
    {
        PrintNewLines(1);
        string warningMessage = $"[lightgoldenrod2_2]{message}[/]";
        AnsiConsole.MarkupLine(warningMessage);
        PrintNewLines(1);
    }

    public static void DisplayCancellationMessage(string message)
    {
        PrintNewLines(1);
        string successMessage = $"[blueviolet]{message}[/]";
        AnsiConsole.MarkupLine(successMessage);
        PrintNewLines(1);
    }

    public static string StringTrimLower(string input)
    {
        return input.Trim().ToLower();
    }

    public static void DisplayFlashCardFront(FlashCardDto flashCard)
    {
        PrintNewLines(1);
        string flashCardFront = $"[bold cyan]Front:[/] {flashCard.Front}";
        AnsiConsole.MarkupLine(flashCardFront);
        PrintNewLines(1);
    }
}

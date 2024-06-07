
using System.Globalization;
using Flashcards.Models;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace Flashcards;

internal static class UserInputs
{
    static string BackMessage = $"or Enter [maroon]0[/] to go back to [bold green]main menu[/]: ";
    static string InvalidInputMessage = $"[maroon]Invalid Input.[/]";

    internal static (string? front, string? back) GetFlashCardData(FlashcardsDbContext dbContext, Stack stack)
    {
        string? front = GetStringInput("Please enter the [bold blue]front[/] of the Flash Card".ToUpper());
        if (front == null) return (null, null);
        while (dbContext.GetFlashCardByStackIdAndFront(stack.Id, front) != null)
        {
            AnsiConsole.Markup($"The [blue]{stack.StackName}[/] already had the [maroon]{front}[/]\n".ToUpper());
            front = GetStringInput("Please enter the [bold blue]front[/] of the Flash Card".ToUpper());
            if (front == null) return (null, null);
        }
        string? back = GetStringInput("Please enter the [bold blue]back[/] of the Flash Card".ToUpper());
        if (back == null) return (null, null);
        return (front.Trim().ToLower(), back.Trim().ToLower());
    }

    internal static string? GetStringInput(string message)
    {
        string? input = AnsiConsole.Ask<string>($"{message} {BackMessage}".ToUpper());
        if (Validations.IsStringEqualsZero(input)) return null;

        while (string.IsNullOrWhiteSpace(input))
        {
            input = AnsiConsole.Ask<string>($"{InvalidInputMessage} {message} {BackMessage}".ToUpper());
            if (Validations.IsStringEqualsZero(input)) return null;
        }
        return input.Trim();
    }

    internal static string? GetYearInput(string message)
    {
        string? input = AnsiConsole.Ask<string>($"{message} {BackMessage}".ToUpper());
        if (Validations.IsStringEqualsZero(input)) return null;

        while (string.IsNullOrWhiteSpace(input) || !DateTime.TryParseExact(input, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            input = AnsiConsole.Ask<string>($"{InvalidInputMessage} {message} {BackMessage}".ToUpper());
            if (Validations.IsStringEqualsZero(input)) return null;
        }
        return input.Trim().ToLower();
    }
}
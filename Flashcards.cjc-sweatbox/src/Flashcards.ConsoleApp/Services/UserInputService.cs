using System.Globalization;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Services;

/// <summary>
/// Helper service for getting valid user inputs of different types.
/// </summary>
internal static class UserInputService
{
    #region Methods
    
    internal static DateTime? GetDateTime(string prompt, string format, Func<string, UserInputValidationResult> validate)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>(prompt);
            if (input == "0")
            {
                return null;
            }

            var validationResult = validate(input);
            if (validationResult.IsValid)
            {
                return DateTime.ParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }

            AnsiConsole.WriteLine(validationResult.Message);
        }
    }

    internal static string GetString(string prompt)
    {
        return AnsiConsole.Ask<string>(prompt);
    }

    #endregion
}

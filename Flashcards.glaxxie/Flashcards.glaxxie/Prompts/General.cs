using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace Flashcards.glaxxie.Prompts;

internal class General
{
    private static readonly string basicPrompt = "Date input ( yyyy-MM-dd HH-mm or leave blank for NOW) :";

    internal static T SelectionInput<T>(string title = "") where T : struct, Enum
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<T>()
                .Title(title) // this could be remove to save space
                .AddChoices(Enum.GetValues<T>())
                .WrapAround()
        );
    }

    internal static DateTime DatetimeInput(string prompt, DateTime? fallback = null)
    {
        var fallbackValue = fallback ?? DateTime.Now;
        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>(basicPrompt)
            .Validate(input =>
            {
                if (DateTime.TryParse(input, out DateTime date) || string.IsNullOrWhiteSpace(input))
                {
                    return ValidationResult.Success();
                }
                return ValidationResult.Error("Invalid format. Accept: yyyy-MM-dd HH-mm");
            }
            ).AllowEmpty());

        return string.IsNullOrWhiteSpace(dateInput) ? fallbackValue : DateTime.Parse(dateInput);
    }
}

using Spectre.Console;

namespace Flashcards.glaxxie.Prompts;

internal class General
{
    internal static T SelectionInput<T>(string title = "") where T : struct, Enum
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<T>()
                .Title(title)
                .AddChoices(Enum.GetValues<T>())
                .WrapAround()
        );
    }

    internal static int SelectionInputInt(string title, IEnumerable<int> options)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title(title)
                .AddChoices(options)
                .WrapAround()
        );
    }

    internal static DateTime DatetimeInput(string prompt, DateTime? fallback = null)
    {
        var fallbackValue = fallback ?? DateTime.Now;
        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
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

    internal static bool ConfirmationInput(string prompt)
    {
        var confirmation = AnsiConsole.Prompt(
            new TextPrompt<bool>(prompt)
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "y" : "n"));
        return confirmation;
    }
}
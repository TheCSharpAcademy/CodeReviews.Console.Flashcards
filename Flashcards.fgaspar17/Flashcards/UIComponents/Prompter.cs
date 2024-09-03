using CodingTracker;
using Spectre.Console;

namespace Flashcards;
public static class Prompter
{
    public static T EnumPrompt<T>() where T : struct, Enum
    {
        return AnsiConsole.Prompt<T>(
            new SelectionPrompt<T>()
                .Title("Choose an option: ")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(Enum.GetValues<T>()).UseConverter<T>(EnumHelper.GetTitle)
                );
    }

    public static string ValidatedTextPrompt(string message, string defaultValue = null, params IValidator[] validations)
    {
        var textPrompt = new TextPrompt<string>($"{message}{CancelSetup.CancelPrompt}:")
                .PromptStyle("bold yellow");

        if (!string.IsNullOrEmpty(defaultValue))
        {
            textPrompt.DefaultValue(defaultValue);
        }

        foreach (var validationResult in validations)
        {
            textPrompt.Validate(input => {
                // Checking first user cancel
                if (input == CancelSetup.CancelString)
                    return ValidationResult.Success();

                return validationResult.Validate(input);
            });
        }

        return AnsiConsole.Prompt<string>(textPrompt);
    }

    public static void PressKeyToContinuePrompt()
    {
        AnsiConsole.Write("Press any key to continue...");
        Console.ReadKey();
    }
}
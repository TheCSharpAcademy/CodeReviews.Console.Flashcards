using System.Globalization;
using Spectre.Console;

namespace vcesario.Flashcards;

public class UserInputValidator
{
    public ValidationResult ValidateDateTimeOrReturn(string input)
    {
        if (input.ToLower().Equals("return"))
        {
            return ValidationResult.Success();
        }

        if (!DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_DATETIMEERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidateDateOrReturn(string input)
    {
        if (input.ToLower().Equals("return"))
        {
            return ValidationResult.Success();
        }

        if (!DateOnly.TryParseExact(input, "dd/MM/yyyy", out DateOnly result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_DATEERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidateFutureDateOrReturn(string input)
    {
        return ValidationResult.Error("Implement");
        // if (input.ToLower().Equals("return"))
        // {
        //     return ValidationResult.Success();
        // }

        // if (!DateOnly.TryParseExact(input, "dd/MM/yyyy", out DateOnly result))
        // {
        //     return ValidationResult.Error(ApplicationTexts.USERINPUT_DATEERROR);
        // }

        // if (result < DateUtils.Today)
        // {
        //     return ValidationResult.Error(ApplicationTexts.USERINPUT_OLDERDATEERROR);
        // }

        // return ValidationResult.Success();
    }

    public ValidationResult ValidateLongReturn(string input)
    {
        if (input.ToLower().Equals("return"))
        {
            return ValidationResult.Success();
        }

        if (!long.TryParse(input, out long result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_LONGERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidatePositiveIntOrReturn(string input)
    {
        if (input.ToLower().Equals("return"))
        {
            return ValidationResult.Success();
        }

        if (!uint.TryParse(input, out uint result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_LONGERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ConfirmUniqueStackName(string input)
    {
        AnsiConsole.MarkupLine("[green]TO-DO: Implement Unique Stack Name validation[/]");
        return ValidationResult.Success();
    }
}
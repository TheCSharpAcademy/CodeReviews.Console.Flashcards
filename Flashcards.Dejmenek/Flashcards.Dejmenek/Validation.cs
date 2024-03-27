using Spectre.Console;
using System.Globalization;

namespace Flashcards.Dejmenek;

public class Validation
{
    public static ValidationResult IsPositiveNumber(int userNumber)
    {
        return userNumber switch
        {
            <= 0 => ValidationResult.Error("[red]You must enter a positive number.[/]"),
            _ => ValidationResult.Success(),
        };
    }

    public static ValidationResult IsValidYear(string? year)
    {
        if (DateTime.TryParseExact(year, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return ValidationResult.Success();
        }
        else
        {
            return ValidationResult.Error("[red]You must enter a valid year: yyyy[/]");
        }
    }
}

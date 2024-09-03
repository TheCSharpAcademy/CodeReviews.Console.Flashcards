using System.Globalization;
using Spectre.Console;

namespace Flashcards;
public class DateTimeValidator : IValidator
{
    public string errorMsg { get; set; } = "Invalid format";
    public required string Format { get; set; }

    public ValidationResult Validate(string? input)
    {
        if (!DateTime.TryParseExact(input, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return ValidationResult.Error($"[red]{errorMsg}[/]");
        }
        return ValidationResult.Success();
    }
}
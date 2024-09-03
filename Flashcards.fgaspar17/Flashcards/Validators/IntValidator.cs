using Spectre.Console;

namespace Flashcards;
public class IntValidator : IValidator
{
    public string errorMsg { get; set; } = "The input must be an integer.";
    public ValidationResult Validate(string input)
    {
        if (!int.TryParse(input, out _))
        {
            return ValidationResult.Error($"[red]{errorMsg}[/]");
        }
        return ValidationResult.Success();
    }
}
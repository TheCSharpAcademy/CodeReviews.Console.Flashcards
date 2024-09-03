using Spectre.Console;

namespace Flashcards;

public interface IValidator
{
    string errorMsg { get; set; }
    ValidationResult Validate(string? input);
}
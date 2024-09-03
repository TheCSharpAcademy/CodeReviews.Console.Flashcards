using Spectre.Console;

namespace Flashcards;

public interface IValidator
{
    string ErrorMsg { get; set; }
    ValidationResult Validate(string? input);
}
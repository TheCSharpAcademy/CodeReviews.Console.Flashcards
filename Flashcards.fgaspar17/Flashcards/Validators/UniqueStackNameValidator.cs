using FlashcardsLibrary;
using Spectre.Console;

namespace Flashcards;
public class UniqueStackNameValidator : IValidator
{
    public string ErrorMsg { get; set; } = "The Stack Name must be unique.";
    public ValidationResult Validate(string input)
    {
        if (StackController.GetStackByName(input) != null)
        {
            return ValidationResult.Error($"[red]{ErrorMsg}[/]");
        }
        return ValidationResult.Success();
    }
}
using FlashcardsLibrary;
using Spectre.Console;

namespace Flashcards;
public class UniqueStackNameValidator : IValidator
{
    public string errorMsg { get; set; } = "The Stack Name must be unique.";
    public ValidationResult Validate(string input)
    {
        if (StackController.GetStackByName(input) != null)
        {
            return ValidationResult.Error($"[red]{errorMsg}[/]");
        }
        return ValidationResult.Success();
    }
}
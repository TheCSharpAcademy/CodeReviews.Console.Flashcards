
namespace Flashcards;

internal class Validations
{
    internal static bool IsStringEqualsZero(string? input) => input != null && input.Trim() == "0";

}
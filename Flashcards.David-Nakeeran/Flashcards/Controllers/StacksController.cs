using Flashcards.Utilities;
using Spectre.Console;

namespace Flashcards.Controllers;

class StacksControllers
{
    private readonly Validation _validation;

    public StacksControllers(Validation validation)
    {
        _validation = validation;
    }
    internal string GetStackName(string message)
    {
        var stackName = AnsiConsole.Ask<string>(message);
        stackName = _validation.CheckInputNullOrWhitespace(message, stackName);
        return stackName.Trim();
    }
}
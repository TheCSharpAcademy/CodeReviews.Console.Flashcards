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
    // internal string? GetAvailableStackName()
    // {
    //     var stackName = AnsiConsole.Ask<string>("Please enter name of available stack or enter 0 to return to main menu");
    //     stackName = _validation.CheckInputNullOrWhitespace("Please enter name of available stack or enter 0 to return to main menu", stackName);
    //     return stackName;
    // }
}
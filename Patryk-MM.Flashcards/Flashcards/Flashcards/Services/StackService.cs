using Flashcards.Models;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Services;
public class StackService {
    private readonly IStackRepository _repository;

    public StackService(IStackRepository repository) {
        _repository = repository;
    }

    public async Task ViewStacks() {
        var stackList = await _repository.GetStackNamesAsync();
        foreach (var stackName in stackList) {
            AnsiConsole.WriteLine(stackName);
        }
    }

    public async Task ManageStack() {
        // Fetch the list of stack names from the repository
        var stackNames = await _repository.GetStackNamesAsync();

        // Add the "Cancel" option to the list
        var choices = stackNames.Concat(new[] { "Cancel" });

        // Display the prompt to the user with the combined list of choices
        var stackName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose a stack to manage:")
            .AddChoices(choices)
        );

        // Handle the user's choice
        if (stackName == "Cancel") {
            AnsiConsole.WriteLine("Operation cancelled.");
            return; // Exit the method or perform any other cancellation logic
        }

        // Proceed with managing the selected stack
        AnsiConsole.WriteLine($"Managing stack: {stackName}");


        Stack stack = await _repository.GetStackByNameAsync(stackName);

        AnsiConsole.WriteLine(stack.Flashcards.Count);
    }
}


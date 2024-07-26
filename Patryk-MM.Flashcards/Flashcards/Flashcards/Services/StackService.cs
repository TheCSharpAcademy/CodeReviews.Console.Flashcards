using Flashcards.Handlers.Stack;
using Flashcards.Models;
using Flashcards.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Services;
public class StackService {
    private readonly IStackRepository _repository;

    public StackService(IStackRepository repository) {
        _repository = repository;
    }

    public async Task ViewStacks() {
        var stackList = await _repository.GetStackNamesAsync();
        var table = new Table();

        table.AddColumn("Stack");

        foreach (var stackName in stackList) {
            table.AddRow(stackName);
        }
        AnsiConsole.Write(table);
    }

    public async Task AddStack() {
        string name = AnsiConsole.Ask<string>("Provide a stack name or input [red]cancel[/]:");
        if (name == "cancel") {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }

        Stack stack = new Stack { Name = name };

        try {
            await _repository.AddAsync(stack);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601) {
            AnsiConsole.MarkupLine("[red]A stack with the same name already exists. Pick a different name.[/]");
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

        Stack stack = await _repository.GetStackByNameAsync(stackName);


        Utilities.ClearConsole();
        while (true) {
            // Proceed with managing the selected stack
            AnsiConsole.MarkupLine($"Managing stack: [aqua]{stack.Name}[/]");
            AnsiConsole.MarkupLine($"Cards in stack: [aqua]{stack.Flashcards.Count}[/]");
            var actionPrompt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nChoose an action to perform:")
                    .AddChoices(new[] { "Change stack's name", "Delete stack", "Cancel" })
            );
            Utilities.ClearConsole();

            IStackActionHandler? handler = actionPrompt switch {
                "Change stack's name" => new ChangeStackNameHandler(_repository),
                "Delete stack" => new DeleteStackHandler(_repository),
                _ => null
            };

            if (handler == null) {
                AnsiConsole.WriteLine("Operation cancelled.");
                return;
            }

            await handler.HandleAsync(stack);
        }
    }
}


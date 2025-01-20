using Flashcards.Config;
using Flashcards.Data;
using Flashcards.Helpers;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.UI;

public class StackMenu
{
    public static void Show(DatabaseConfig config)
    {
        using var dbContext = new AppDbContext(config);
        var stackService = new StackService(dbContext);
        var isRunning = true;

        while (isRunning)
            try
            {
                var stackAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose a stack operation")
                        .AddChoices("Create Stack", "View All Stacks", "Update Stack", "Delete Stack", "Back"));

                switch (stackAction)
                {
                    case "Create Stack":
                        var stackName = AnsiConsole.Ask<string>("Enter stack name:");
                        if (!ValidationHelper.ValidateString(stackName, "Stack name", 100)) break;

                        stackService.CreateStack(stackName);
                        AnsiConsole.MarkupLine("[green]Stack created successfully![/]");
                        break;

                    case "View All Stacks":
                        var stacks = stackService.GetAllStacks();
                        if (stacks.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[yellow]No stacks found.[/]");
                        }
                        else
                        {
                            var table = new Table().Centered();
                            table.AddColumn("Id");
                            table.AddColumn("Name");

                            foreach (var stack in stacks) table.AddRow(stack.Id.ToString(), stack.Name);

                            AnsiConsole.Write(table);
                        }

                        break;

                    case "Update Stack":
                        var stackIdToUpdate = AnsiConsole.Ask<int>("Enter stack ID to update:");
                        if (!ValidationHelper.ValidateId(stackIdToUpdate, "stack")) break;

                        var newStackName = AnsiConsole.Ask<string>("Enter new stack name:");
                        if (!ValidationHelper.ValidateString(newStackName, "New stack name", 100)) break;

                        var updated = stackService.UpdateStack(stackIdToUpdate, newStackName);
                        if (updated)
                            AnsiConsole.MarkupLine("[green]Stack updated successfully![/]");
                        else
                            AnsiConsole.MarkupLine("[red]Error: Stack not found.[/]");
                        break;

                    case "Delete Stack":
                        var stackIdToDelete = AnsiConsole.Ask<int>("Enter stack ID to delete:");
                        if (!ValidationHelper.ValidateId(stackIdToDelete, "stack")) break;

                        var deleted = stackService.DeleteStack(stackIdToDelete);
                        if (deleted)
                            AnsiConsole.MarkupLine("[green]Stack deleted successfully![/]");
                        else
                            AnsiConsole.MarkupLine("[red]Error: Stack not found.[/]");
                        break;

                    case "Back":
                        isRunning = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
            }
    }
}
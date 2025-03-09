using Flashcards.Controllers;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Managers;

internal class CardStackManager
{
    private CardStackController cardStackController = new();
    private int id;
    private string stackName;

    internal void DisplayStackOptions()
    {
        bool loop = true;
        while (loop)
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] { "Show stacks", "Add a stack", "Edit a stack", "Delete a stack", "Return" }));

            switch (choice)
            {
                case "Show stacks":
                    DisplayStacks();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "Add a stack":
                    stackName = UserInput.getStringInput("Enter name of the new stack or enter 0 to return: ");
                    if (stackName != "0")
                    {
                        cardStackController.AddStack(stackName);
                    }
                    break;
                case "Edit a stack":
                    DisplayStacks();
                    id = UserInput.getIntInput("Enter ID of the stack you want to edit or enter 0 to return");
                    if (id != 0)
                    {
                        UserInput.GetCorrectStackId(cardStackController, id);
                        string name = UserInput.getStringInput("Enter new name for the stack: ");
                        cardStackController.EditStack(id, name);
                    }
                    break;
                case "Delete a stack":
                    DisplayStacks();
                    id = UserInput.getIntInput("Enter ID of the stack you want to delete or enter 0 to return");
                    if (id != 0)
                    {
                        UserInput.GetCorrectStackId(cardStackController, id);
                        cardStackController.DeleteStack(id);
                    }
                    break;
                case "Return":
                    loop = false;
                    break;
            }
        }
    }

    internal void DisplayStacks()
    {
        List<CardStack> stacks = cardStackController.GetAllStacks();
        if (stacks.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No stacks found![/]");
        }
        else
        {
            Console.WriteLine("Your stacks: ");
            foreach (var stack in stacks)
            {
                AnsiConsole.MarkupLine($"[green]{stack.Id}[/]. {stack.Name}");
            }
        }
    }
}

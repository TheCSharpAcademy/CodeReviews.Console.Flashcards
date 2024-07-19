using Flashcards.Controllers;

namespace Flashcards.StackView;

public static class StackViewController
{
    public static void InitStackView()
    {
        Console.Clear();

        Table table = new();
        table.AddColumns(["ID", "name"]);

        var stacks = StackController.GetAllStacks();
        int i = 1;

        foreach ( var stack in stacks )
        {
            table.AddRow([i++.ToString(), stack.Name]);
        }

        Console.WriteLine("List of stacks:");
        AnsiConsole.Write(table);

        Console.ReadKey();
        Console.WriteLine("Press C to create a new stack, U to update a stack, D to open the deletion menu, or 0 to go back.");

    }
}
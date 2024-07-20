using Flashcards.CardView;
using Flashcards.Controllers;
using Flashcards.StudyView;
using Microsoft.IdentityModel.Tokens;

namespace Flashcards.StackView;

public static class StackViewController
{
    public static void InitStackMainView()
    {
        Console.Clear();

        Table table = new();
        table.AddColumns(["ID", "name"]);

        var stacks = StackController.GetAllStacks();
        int i = 1;

        foreach (var stack in stacks) table.AddRow([i++.ToString(), stack.Name]);

        Console.WriteLine("List of stacks:");
        AnsiConsole.Write(table);
        Console.WriteLine("Press C to create a new stack, D to open the deletion menu, or 0 to go back.");

        var option = GetStackViewInput();

        if (option == 'c') InitStackCreateView();
        else if (option == 'd') return;
        else return;

    }

    public static char GetStackViewInput()
    {
        var key = Console.ReadKey(true).KeyChar;

        if (key != 'c' && key != 'd' && key != '0')
        {
            return GetStackViewInput();
        }

        return key;
    }

    public static void InitStackCreateView()
    {
        Console.WriteLine("Enter the name for your stack:");
        var input = GetStackName();

        StackController.CreateStack(input);
        Console.WriteLine($"Stack \"{input}\" created successfully.");
        Console.ReadKey();
    }

    public static string GetStackName()
    {
        var name = Console.ReadLine();

        if (name.IsNullOrEmpty() || name!.Length > 20)
        {
            Console.WriteLine($"Invalid name. Stack names must have a length of at least one character and under 40 characters total");
            return GetStackName();
        }

        return name;
    }
}
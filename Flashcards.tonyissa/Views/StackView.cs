using Flashcards.CardView;
using Flashcards.Controllers;
using Flashcards.StudyView;
using Microsoft.IdentityModel.Tokens;
using Flashcards.Models;

namespace Flashcards.StackView;

public static class StackViewController
{
    public static void InitStackMainView()
    {
        Console.Clear();

        Table table = new();
        table.AddColumns(["ID", "name"]);

        var stackList = StackController.GetAllStacks();

        foreach (var stack in stackList) table.AddRow(stack.Id.ToString(), stack.Name);

        Console.WriteLine("List of stacks:");
        AnsiConsole.Write(table);
        Console.WriteLine("Press C to create a new stack, D to open the deletion menu, or 0 to go back.");

        var option = GetStackViewInput();

        if (option == 'c') InitStackCreateView();
        else if (option == 'd') InitStackDeleteView(stackList);
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
        Console.WriteLine("Enter the name for your stack, or type 0 to quit:");
        var input = GetNewStackName();

        if (input == "0") return;

        StackController.CreateStack(input);
        Console.WriteLine($"Stack \"{input}\" created successfully. Press any key to continue...");
        Console.ReadKey();
    }

    public static string GetNewStackName()
    {
        var name = Console.ReadLine();

        if (name.IsNullOrEmpty() || name!.Length > 20)
        {
            Console.WriteLine($"Invalid name. Stack names must have a length of at least one character and under 40 characters total");
            return GetNewStackName();
        }

        return name;
    }

    public static void InitStackDeleteView(List<Stack> stackList)
    {
        Console.WriteLine("Enter the number of the stack you would like to delete, or type 0 to quit: ");
        Console.WriteLine("WARNING: This also deletes all associated flashcards");
        var stackToDelete = GetStackDeleteInput(stackList);

        if (stackToDelete == 0) return;

        StackController.DeleteStack(stackToDelete);
        Console.WriteLine($"Stack {stackToDelete} deleted successfully. Press any key to continue...");
        Console.ReadKey();
    }

    public static int GetStackDeleteInput(List<Stack> stackList)
    {
        var input = Console.ReadLine();

        if (!int.TryParse(input, out int index))
        {
            Console.WriteLine("Invalid input. Please input the number of the stack you would like to delete.");
            return GetStackDeleteInput(stackList);
        }
        else if (!stackList.Exists(stack => stack.Id == index) && index != 0)
        {
            Console.WriteLine("Stack not found. Try again.");
            return GetStackDeleteInput(stackList);
        }

        return index;
    }
}
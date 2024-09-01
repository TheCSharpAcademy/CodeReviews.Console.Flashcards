using Flashcards.Tables;
using Flashcards.Models;

namespace Flashcards;

internal class StacksInput
{
    public static void CreateStack()
    {
        Console.Clear();

        var stackList = Stacks.GetAllStacks();
        string stackName;

        while (true)
        {
            stackName = GetStackName("Enter the stack name:\n");

            if (stackList.Any(stack => stack.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("You have already created a stack with that name. Please choose a different name.\n");
            }
            else
            {
                break;
            }
        }

        Stack stack = new Stack()
        {
            Name = stackName
        };

        Stacks.InsertStack(stackName);
    }

    internal static string GetStackName(string prompt)
    {
        Console.WriteLine(prompt);

        string stackName = Console.ReadLine();

        stackName = Utility.ValidString(stackName);

        return stackName;
    }
}

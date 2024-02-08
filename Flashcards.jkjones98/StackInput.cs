using Flashcards.Functions;
using Microsoft.VisualBasic;

namespace Flashcards.jkjones98;

internal class StackInput
{
    CheckUserInput checkUserInput = new();
    StackController controller = new();
    MainMenu mainMenu = new();
    internal void AddNewStack()
    {
        Console.WriteLine("Please enter the name of the language you would like to create a stack for");
        Console.WriteLine("Alternatively, enter 0 to return to the main menu");
        string stackName = Console.ReadLine();

        if(stackName == "0") mainMenu.DisplayMenu();

        while(string.IsNullOrEmpty(stackName) || stackName.Any(char.IsDigit) || controller.CheckNameExists(stackName))
        {
            Console.WriteLine("Empty answer, duplicate stack name or contains a digit. Please enter again.");
            stackName = Console.ReadLine();
        }

        Stack stack = new()
        {
            StackName = stackName
        };

        controller.InsertStackDb(stack);
    }

    internal void ViewStacks()
    {
        controller.ViewStackDb();
    }

    internal void ChangeStackName()
    {
        controller.ViewStackDb();
        Console.WriteLine("\nEnter the Id of the stack you would like to change");
        string stackChoice = Console.ReadLine();
        int stackId = checkUserInput.CheckForChar(stackChoice, "Stacks", "StackId");
        Console.WriteLine("Please enter what you would like to change the stack name to");
        string newStackName = Console.ReadLine();

        while(string.IsNullOrEmpty(newStackName) || newStackName.Any(char.IsDigit) || controller.CheckNameExists(newStackName))
        {
            Console.WriteLine("Empty answer, duplicate stack name or contains a digit. Please enter again.");
            newStackName = Console.ReadLine();
        }
        controller.ChangeStackDb(stackId, newStackName);
        controller.ViewStackDb();
    }

    internal void DeleteStacks()
    {
        //TEST THIS
        controller.ViewStackDb();
        Console.WriteLine("\nWhich stack would you like to delete?");
        string deleteStack = Console.ReadLine();
        int deleteId = checkUserInput.CheckForChar(deleteStack,"Stacks","StackId");
        controller.DeleteStackDb(deleteId);
    }
}
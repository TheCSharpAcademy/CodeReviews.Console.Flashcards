using DataAccess;
using DataAccess.Models;
using Flashcards.SamGannon.DTOs;
using Flashcards.SamGannon.Utilities;

namespace Flashcards.SamGannon.UI;

public class StackMenu : IMenu
{ 
    private readonly IMenu _MainMenu;

    public StackMenu(MainMenu mainMenu)
    {
        _MainMenu = mainMenu;
    }

    public IDataAccess DataAccess => _MainMenu.DataAccess;

    public void ShowMenu()
    {
        MenuMessages.ShowStackMenu();
        string choice = Console.ReadLine();
        StackMenuOptions(choice);
    }

    private void StackMenuOptions(string? choice)
    {
        while (choice != "1" && choice != "2" && choice != "3" && choice != "4")
        {
            Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.");
            choice = Console.ReadLine();
        }

        switch (choice)
        {
            case "1":
                CreateStack();
                break;
            case "2":
                EditStacks();
                break;
            case "3":
                DeleteStacks();
                break;
            case "4":
                ShowMenu();
                break;
            default:
                Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.");
                break;
        }
    }

    private void CreateStack()
    {
        string stackName = InputStackName();
        DataAccess.AddStack(stackName);

        Console.WriteLine($"Stack '{stackName}' created successfully! Press a key to go back to the Main Menu.");
        Console.Read();
        _MainMenu.ShowMenu();
    }

    private string InputStackName()
    {
        Console.Clear();
        Console.WriteLine("Enter the name for your new stack:");
        Console.WriteLine();
        Console.WriteLine("**Please note: Stack names are not case sensitive.**");
        Console.WriteLine("IE. Spanish, spanish, SPANISH, SpAnISh are all the same input.");
        string? stackName = Console.ReadLine();
        string formattedStackName = stackName.Trim().ToUpper();
        int iAttempts = 0;

        while (string.IsNullOrEmpty(stackName))
        {
            if (iAttempts < 3)
            {
                Console.WriteLine("Please enter a name for your new flashcard stack");
                stackName = Console.ReadLine();
                formattedStackName = stackName.Trim().ToUpper();
                iAttempts++;
            }
            else
            {
                Console.WriteLine("You have exceeded the amount of attempts.");
                Environment.Exit(1);
            }
        }

        iAttempts = 0;
        bool stackExists = DataAccess.CheckStackName(formattedStackName);

        while (stackExists)
        {
            if (iAttempts < 3)
            {
                Console.WriteLine($"A stack with the name '{stackName}' already exists. Please enter a different name");
                stackName = Console.ReadLine();
                formattedStackName = stackName.Trim().ToUpper();
                stackExists = DataAccess.CheckStackName(formattedStackName);
                iAttempts++;
            }
            else
            {
                Console.WriteLine("You have exceeded the amount of attempts.");
                Environment.Exit(1);
            }

        }

        iAttempts = 0;
        return formattedStackName;
    }

    private void EditStacks()
    {
        ProcessStackOperation("edit");
    }

    private void DeleteStacks()
    {
        ProcessStackOperation("delete");
    }

    private void ProcessStackOperation(string editType)
    {
        ConsoleHelper.Map(DataAccess, "Stack Menu");

        Console.WriteLine($"\n\nPlease enter the name of the stack you wish to {editType}:");
        string stackName = ConsoleHelper.ValidateStackName(DataAccess);

        if(stackName == null )
        {
            _MainMenu.ShowMenu();
        }

        StackModel rawSingleStack = DataAccess.GetStackByName(stackName);
        StackDto singleStack = new StackDto(rawSingleStack);

        EditStacksSubMenu(singleStack, editType);
    }

    private void EditStacksSubMenu(StackDto stack, string editType)
    {
        TableVisualization.ShowSingleRow(new List<StackDto> { stack }, stack.StackName);

        Console.WriteLine($"\nIs this the stack you wish to {editType}? (Y/N)");
        Console.WriteLine($"Y - {editType} stack");
        Console.WriteLine("N - back to Stack list\n");

        string formattedChoice = ConsoleHelper.GetValidChoice();

        MakeEditSelection(stack, formattedChoice, editType);
    }

    private void MakeEditSelection(StackDto stack, string formattedChoice, string editType)
    {
        if (formattedChoice == "Y")
        {
            if (editType == "edit")
            {
                EditStackName(stack);
            }
            else if (editType == "delete")
            {
                DeleteStack(stack);
            }
            else
            {
                // handle error
            }
        }
        else if (formattedChoice == "N")
        {
            EditStacksSubMenu(stack, editType);
        }
        else
        {
            // handle error
        }
    }

    private void EditStackName(StackDto stack)
    {
        string newStackName = InputStackName();
        DataAccess.EditStackname(stack.StackName, newStackName);
        Console.WriteLine("Stack name edited successfully! Press a key to continue.");
        Console.ReadLine();
    }

    private void DeleteStack(StackDto stack)
    {
        DataAccess.DeleteStack(stack.StackName);
        Console.WriteLine("Stack name deleted successfully! Press a key to continue.");
        Console.ReadLine();
    }
}

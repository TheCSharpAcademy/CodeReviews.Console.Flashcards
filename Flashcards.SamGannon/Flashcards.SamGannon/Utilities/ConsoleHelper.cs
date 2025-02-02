using DataAccess;
using DataAccess.Models;
using Flashcards.SamGannon.DTOs;
using Flashcards.SamGannon.UI;

namespace Flashcards.SamGannon.Utilities;

public static class ConsoleHelper
{
    public static string ReadValidInput(IEnumerable<string> validChoices)
    {
        string choice = Console.ReadLine()?.Trim().ToUpper();

        while (!validChoices.Contains(choice))
        {
            Console.WriteLine("Invalid choice. Please enter a valid option.");
            choice = Console.ReadLine()?.Trim().ToUpper();
        }

        return choice;
    }

    public static string ValidateStackName(IDataAccess dataAccess)
    {
        string rawStackName;

        while (true)
        {
            Console.WriteLine("Enter Stack Name (or 'E' to exit): ");
            rawStackName = Console.ReadLine();
            string formattedStackName = rawStackName.Trim().ToUpper();

            if (formattedStackName.Equals("E", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            else if (dataAccess.CheckStackName(formattedStackName))
            {
                return formattedStackName;
            }
            else
            {
                Console.WriteLine("Invalid stack name. Please enter a name from the list");
            }
        }
    }

    public static string GetValidInput()
    {
        string input = "";
        while (string.IsNullOrEmpty(input))
        {
            input = Console.ReadLine();
        }
        return input;
    }

    public static string GetValidChoice()
    {
        string choice = "";
        while (choice != "Y" && choice != "N")
        {
            choice = GetValidInput().Trim().ToUpper();
        }
        return choice;
    }

    public static bool Map(IDataAccess DataAccess, string title)
    {
        Console.Clear();
        List<StackModel> lstAllStacks = new();
        lstAllStacks = DataAccess.GetAllStacks();

        if(lstAllStacks.Count < 1)
        {
            Console.WriteLine("There are no stacks to study. Press a key to exit.");
            Console.ReadLine();
            return false;
        }

        List<StackDto> stacks = StackDto.ToDto(lstAllStacks);

        TableVisualization.ShowTable(stacks, title);

        return true;
    }
}

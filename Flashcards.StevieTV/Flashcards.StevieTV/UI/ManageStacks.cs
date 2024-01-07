using Flashcards.StevieTV.Helpers;
using Flashcards.StevieTV.Models;
using Flashcards.StevieTV.Database;

namespace Flashcards.StevieTV.UI;

internal class ManageStacks
{
    public static void StacksMenu()
    {
        var exitManageStacks = false;

        while (!exitManageStacks)
        {
            PrintCurrentStackNames();
        
            Console.WriteLine("Please choose an option from the list below\n");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("0 - Return to Main Menu");
            Console.WriteLine("1 - Add a Stack");
            Console.WriteLine("2 - Remove a Stack");
            Console.WriteLine("3 - Edit Flash Cards in a Stack");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Enter your choice:");
            
            int menuOption;
            var menuInput = Console.ReadLine();

            while (!Int32.TryParse(menuInput, out menuOption) || !InputValidation.TestValidMenuOption(menuOption, 0, 3))
            {
                Console.WriteLine("Invalid Input, please enter an option from 0 to 3");
                menuInput = Console.ReadLine();
            }

            switch (menuOption)
            {
                case 0:
                    exitManageStacks = true;
                    break;
                case 1:
                    AddStack();
                    break;
                case 2:
                    RemoveStack();
                    break;
                case 3:
                    // EditStack();
                    break;

            }
        }
    }
    private static void PrintCurrentStackNames()
    {
        Console.Clear();
        Console.WriteLine("These Are The Current Stacks:\n");

        var stacks = StacksManager.GetStacks();
        List<StackDTO> simplifiedStacks = new List<StackDTO>();

        foreach (var stack in stacks)
        {
            simplifiedStacks.Add(StackMapper.StackMapToDTO(stack));
        }

        TableVisualisation.ShowTable(simplifiedStacks);

    }

    private static void AddStack()
    {
        Console.WriteLine("Please enter the name of the Stack you would like to add and press enter (or press 0 to cancel):");
        var newStackName = Console.ReadLine().Trim();
        if (Int32.TryParse(newStackName, out int input) && input == 0) return;

        while (string.IsNullOrWhiteSpace(newStackName) || CheckStackExists(newStackName))
        {
            if (Int32.TryParse(newStackName, out input) && input == 0) return;
            Console.WriteLine("Invalid Input or Stack already exists. Please enter the name of the Stack you would like to add and press enter (or press 0 to cancel):");
            newStackName = Console.ReadLine().Trim();
        }

        StacksManager.Post(new StackDTO
        {
            Name = newStackName.ToTitleCase()
        });
    }
    
    private static void RemoveStack()
    {
        Console.WriteLine("Please enter the name of the Stack you would like to remove and press enter (or press 0 to cancel):");
        var removeStackName = Console.ReadLine().Trim();

        while (string.IsNullOrWhiteSpace(removeStackName) || !CheckStackExists(removeStackName))
        {
            if (Int32.TryParse(removeStackName, out int input) && input == 0) return;
            Console.WriteLine("Invalid Input or Stack does not exist. Please enter the name of the Stack you would like to remove and press enter  (or press 0 to cancel):");
            removeStackName = Console.ReadLine().Trim();
        }
        
        Console.WriteLine($"This will remove the Stack '{removeStackName.ToTitleCase()}' and all associated Flash Cards");
        Console.WriteLine("Press Enter to continue or any other key to cancel");
        var keyPressed = Console.ReadKey();

        if (keyPressed.Key == ConsoleKey.Enter)
        {
            StacksManager.Delete(StacksManager.GetStackByName(removeStackName.ToTitleCase()));
        }
    }


    private static bool CheckStackExists(string newStackName)
    {
        var currentStacks = StacksManager.GetStacks();
        var newStackFound = false;

        foreach (var stack in currentStacks)
        {
            if (newStackName.ToLower() == stack.Name.ToLower())
                newStackFound = true;
        }

        return newStackFound;
    }
}
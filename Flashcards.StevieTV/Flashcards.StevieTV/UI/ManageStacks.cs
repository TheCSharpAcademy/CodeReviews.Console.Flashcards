using Flashcards.StevieTV.Helpers;
using Flashcards.StevieTV.Models;

namespace Flashcards.StevieTV.UI;

internal class ManageStacks
{
    public static void StacksMenu()
    {
        Console.Clear();
        Console.WriteLine("These Are The Current Stacks:\n");
        var stacks = Flashcards.DatabaseManager.GetStacks();

        List<StackDTO> simplifiedStacks = new List<StackDTO>();

        foreach (var stack in stacks)
        {
            simplifiedStacks.Add(StackMapper.StackMapToDTO(stack));
        }

        TableVisualisation.ShowTable(simplifiedStacks);

        Console.WriteLine("Please choose an option from the list below\n");
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("0 - Return to Main Menu");
        Console.WriteLine("1 - Add a Stack");
        Console.WriteLine("2 - Remove a Stack");
        Console.WriteLine("3 - Edit a Stack");
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("");
        Console.WriteLine("Enter your choice:");

        var menuInput = Console.ReadLine();
        var exitManageStacks = false;

        while (!exitManageStacks)
        {
            var menuOption = 100;

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
                    // AddStack();
                    break;
                case 2:
                    // RemoveStack();
                    break;
                case 3:
                    // EditStack();
                    break;

            }
        }
    }
}
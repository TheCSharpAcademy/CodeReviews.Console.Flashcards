using Flashcards.StevieTV.Helpers;

namespace Flashcards.StevieTV.UI;

public class Menu
{
    public static void MainMenu()
    {
        var exitApp = false;

        while (!exitApp)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the FlashCards App\n");
            Console.WriteLine("Please choose an option from the list below\n");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Manage Stacks");
            Console.WriteLine("2 - Manage Flashcards");
            Console.WriteLine("3 - Begin a Study Session");
            Console.WriteLine("4 - View Study Sessions");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Enter your choice:");

            var menuInput = Console.ReadLine();
            
            int menuOption;

            while (!Int32.TryParse(menuInput, out menuOption) || !InputValidation.TestValidMenuOption(menuOption, 0, 4))
            {
                Console.WriteLine("Invalid Input, please enter an option from 0 to 4");
                menuInput = Console.ReadLine();
            }

            switch (menuOption)
            {
                case 0:
                    Console.WriteLine("Goodbye");
                    exitApp = true;
                    Environment.Exit(0);
                    break;
                case 1:
                    ManageStacks.StacksMenu();
                    break;
                case 2:
                    // ManageCards();
                    break;
                case 3:
                    // StudySession();
                    break;
                case 4:
                    // ViewStudySessions();
                    break;
            }
        }
    }


}
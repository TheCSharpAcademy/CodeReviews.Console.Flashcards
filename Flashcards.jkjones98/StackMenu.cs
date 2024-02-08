namespace Flashcards.jkjones98;

internal class StackMenu
{
    
    internal void DisplayStackMenu()
    {
        StackInput getInputs = new();
        bool rtnMainMenu = false;

        Console.Clear();
        getInputs.ViewStacks();
        while(!rtnMainMenu)
        {
            Console.WriteLine("\n\nSTACK MENU");
            Console.WriteLine("\nChoose from the options below");
            Console.WriteLine("Enter 1 - Add new stack");
            Console.WriteLine("Enter 2 - Change stack");
            Console.WriteLine("Enter 3 - Delete stack");
            Console.WriteLine("Enter 0 - Return to main menu");
            string stackSelection = Console.ReadLine();

            while(string.IsNullOrEmpty(stackSelection))
            {
                Console.WriteLine("Invalid entry. Please choose again");
                stackSelection = Console.ReadLine();
            }

            switch(stackSelection)
            {
                case "1": 
                    getInputs.AddNewStack();
                    break;
                case "2":
                    // NEED TO TEST
                    getInputs.ChangeStackName();
                    break;
                case "3": 
                    // NEED TO TEST
                    getInputs.DeleteStacks();
                    break;
                case "0": 
                    Console.Clear();
                    MainMenu mainMenu = new();
                    mainMenu.DisplayMenu();
                    break;
                default:
                    Console.WriteLine("Invalid entry. Please try again");
                    break;
            }
        }
        
    }
}
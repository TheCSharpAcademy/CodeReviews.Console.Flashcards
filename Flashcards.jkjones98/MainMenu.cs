using System.Diagnostics;

namespace Flashcards.jkjones98;

internal class MainMenu
{
    internal void DisplayMenu()
    {
        ViewStudySessions viewSessions = new();
        StudyMenu studyMenu = new();
        StackMenu stackMenu = new();
        FlashcardsMngMenu flashcardMenu = new();
        bool closeApp = false;
        while(!closeApp)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nPlease select any of the options below");
            Console.WriteLine("Enter 1 - Manage Stacks");
            Console.WriteLine("Enter 2 - Manage Flashcards");
            Console.WriteLine("Enter 3 - Start study session");
            Console.WriteLine("Enter 4 - View study sessions");
            Console.WriteLine("Enter 0 - Exit application");

            string selection = Console.ReadLine();

            while(string.IsNullOrEmpty(selection))
            {
                Console.WriteLine("Invalid entry. Please enter the number corresponding to your selection again.");
                selection = Console.ReadLine();
            }

            switch(selection)
            {
                case "1":
                    stackMenu.DisplayStackMenu();
                    break;
                case "2":
                    flashcardMenu.DisplayFlashcardMenu();
                    break;
                case "3":
                // Start study sessions menu
                    studyMenu.DisplayStudyMenu();
                    break;
                case "4":
                    viewSessions.ViewSessionMenu();
                // View study sessions
                    break;
                case "0":
                closeApp = true;
                Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid entry please enter the number corresponding to what you would like to do.");
                    break;
            }
        }
    }
}
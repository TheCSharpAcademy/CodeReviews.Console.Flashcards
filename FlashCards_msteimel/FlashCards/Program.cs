using FlashCards.Control;
using FlashCards.Database;
using FlashCards.View;

namespace FlashCards;

internal static class Program
{
    private static void Main(string[] args)
    {
        DatabaseCreation.CreateDatabase();

        Console.WriteLine("Do you want to seed the database? Enter 1 for yes");
        string reply = Console.ReadLine();

        if (reply == "1")
        {
            Helper.SeedDatabase();
        }

        Console.WriteLine("Welcome to FlashCards!\n");

        while (true)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("1. Study Session");
            Console.WriteLine("2. Manage stacks");
            Console.WriteLine("3. Manage flashcards");
            Console.WriteLine("4. Exit\n");
            Console.WriteLine("Choose an option from the menu.");
            
            string response = Console.ReadLine();

            switch (response)
            {
                case "1":
                    Console.Clear();
                    StudySessionMenu.DisplayStudySessionMenu();
                    break;
                case "2":
                    Console.Clear();
                    StackMenu.DisplayStackMenu();
                    break;
                case "3":
                    Console.Clear();
                    FlashCardMenu.DisplayFlashCardMenu();
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please try again.\n");
                    break;
            }
        }
    }
}

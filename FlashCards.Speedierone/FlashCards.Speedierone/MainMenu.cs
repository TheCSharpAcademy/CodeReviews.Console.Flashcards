namespace FlashCards;

internal class MainMenu
{
    internal static void ShowMenu()
    {
        
        bool closeApp = false;
        while (closeApp == false)
        {            
            Console.Clear();
            Console.WriteLine("0 - Exit programme.");
            Console.WriteLine("1 - Start a study session.");
            Console.WriteLine("2 - View stacks and flashcards.");
            Console.WriteLine("3 - Add new stack/flashcards.");
            Console.WriteLine("4 - Delete stack/flashcards.");
            Console.WriteLine("5 - View previous sessions.");
            Console.WriteLine("6 - View reports.");
            Console.WriteLine("\nPlease enter the number of the option you would like.");
   
            var command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("Goodbye.");
                    closeApp = true;
                    Environment.Exit(0);
                    break;                    
                case "1":
                    StudyGame.FlashGame();
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Please choose from the following options.");
                    Console.WriteLine("0 - Return to main menu.");
                    Console.WriteLine("1 - View stacks.");
                    Console.WriteLine("2 - View flashcards.");
                    var viewCommand = Console.ReadLine();
                    switch (viewCommand)
                    {
                        case "0":
                            ShowMenu();
                            break;
                        case "1":
                            UserInput.ViewStacks();
                            Console.WriteLine("Press any button to continue.");
                            Console.ReadLine();
                            break;
                        case "2":
                            UserInput.ViewFlashCards();                           
                            break;
                        default:
                            Console.WriteLine("Incorrect Input. Press any button to try again.");
                            Console.ReadLine();
                            break;
                    }
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Please choose from the following options.");
                    Console.WriteLine("0 - Return to main menu.");
                    Console.WriteLine("1 - Add new stack.");
                    Console.WriteLine("2 - Add new flashcard to stack.");
                    var newCommand = Console.ReadLine();
                    switch (newCommand)
                    {
                        case "0":
                            ShowMenu();
                            break;
                        case "1":
                            UserInput.AddStack();
                            break;
                        case "2":
                            UserInput.AddFlashCard();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                    }
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Please choose from the following options.");
                    Console.WriteLine("0 - Return to main menu.");
                    Console.WriteLine("1 - Delete stack.");
                    Console.WriteLine("2 - Delete flashcard.");
                    var deleteCommand = Console.ReadLine();
                    switch (deleteCommand)
                    {
                        case "0":
                            ShowMenu();
                            break;
                        case "1":
                            UserInput.DeleteStack();
                            break;
                        case "2":
                            UserInput.DeleteFlashCard();
                            break;
                        default : 
                            Console.WriteLine("Invalid Input. Please try again.");
                            break;
                    }
                    break;
                case "5":
                    UserInput.ShowGameHistory();
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine("Please choose from the following options.");
                    Console.WriteLine("0 - Return to main menu.");
                    Console.WriteLine("1 - Total sessions per month.");
                    Console.WriteLine("2 - Average score per month.");
                    var reportCommand = Console.ReadLine();
                    switch (reportCommand)
                    {
                        case "0":
                            ShowMenu();
                            break;
                        case "1":
                            UserInput.TotalSessionsMonth();
                            break;
                        case "2":
                            UserInput.AverageScoreMonth();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please try again.");
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid input. Please press enter and try again.");
                    Console.ReadLine();
                    Console.Clear();
                    break;
            }
        }
    }
}

using AdityaFlashCards.Database.DatabaseManager;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using AdityaFlashCards.Database.Models;
using AdityaFlashCards.Helper;

namespace AdityaFlashCards;

internal class Application
{
    public DatabaseManager Db { get; set; }
    public Application() {
        IConfiguration configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Db = new DatabaseManager(configurationBuilder);
    }

    internal void MainMenu()
    {
        bool runApp = true;
        while (runApp)
        {
            Console.Clear();
            string option = Display.GetSelection("Hi! What do you wish to do?", new List<string> { "Manage Stacks", "Manage Flashcards", "Study", "View Study Sessions", "Quit" });
            switch (option)
            {
                case "Manage Stacks":
                    ManageStacks();
                    break;
                case "Manage Flashcards":
                    ManageFlashCards();
                    break;
                case "Study":
                    HandleStudy();
                    break;
                case "View Study Sessions":
                    HandleViewStudySession();
                    break;
                case "Quit":
                    Console.WriteLine("Goodbye");
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }

    internal void ManageStacks()
    {
        string selection = Display.GetSelection("Manage Stacks", new List<string> { "View Stacks", "Create New Stacks", "Delete Stack", "Return to Main Menu" });
        switch (selection)
        {
            case "View Stacks":
                List<Stack> allStacks = Db.GetAllStacks();
                Console.WriteLine("----------Following is the list of all stacks ---------------");
                List<string> list = new List<string>();
                foreach (var stack in allStacks)
                {
                    list.Add(stack.Name.ToString());
                }
                list.Add("Go to Main Menu");
                string stackName = Display.GetSelection("Select a stack to view from the list", list);
                Console.Clear();
                if (stackName != "Go to Main Menu")
                {
                    Console.WriteLine("----------Following are the flashcards in your stack ---------------");
                    Console.WriteLine("----------StackName: {0}---------- ", stackName);
                    List <FlashCardDtoStackView> Flashcards = Db.GetFlashCardsForGivenStack(stackName);
                    if (Flashcards.Count > 0)
                    {
                        Display.DisplayFlashCards(new string[] { "PositionInStack", "Front Side", "Back Side" }, Flashcards);
                        Console.Write("Press any key to continue...");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Write("The stack you wanted to see is empty.Press any key to continue...");
                        Console.ReadLine();
                    }
                }
                break;
            case "Create New Stacks":
                stackName = AnsiConsole.Ask<string>("Enter the name of the new stack : ");
                if (Db.CreateNewStack(stackName))
                {
                    Console.Write("The new stack was created successfully. Press any key to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.Write("The following stack name is already taken. Could not create stack. Press any key to continue...");
                    Console.ReadLine();
                }
                break;
            case "Delete Stack":
                allStacks = Db.GetAllStacks();
                if (allStacks.Count > 0)
                {
                    Console.WriteLine("----------Following is the list of stacks to delete ---------------");
                    list = new List<string>();
                    foreach (var stack in allStacks)
                    {
                        list.Add(stack.Name.ToString());
                    }
                    list.Add("Exit without deletion");
                    stackName = Display.GetSelection("Select a stack to delete from the list", list);
                    Db.DeleteStack(stackName);
                    Console.Write("The new stack was deleted successfully. Press any key to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("There are no stacks to be deleted. Kindly create some stacks.. Press any key to continue...");
                    Console.ReadLine();
                }
                break;
            case "Return to Main Menu":
                break;
            default:
                break;
        }
    }

    internal void HandleViewStudySession()
    {
        List<StudySession> sessions = Db.GetStudySessions();
        if (sessions.Count > 0)
        {
            Display.DisplayStudySessions(new string[] { "StackName", "SessionDate", "YourSessionScore" }, sessions);
            Console.Write("Press any key to continue...");
            Console.ReadLine();
        }
        else
        {
            Console.Write("There are no StudySession currently present. Press any key to continue...");
            Console.ReadLine();
        }
    }

    internal void HandleStudy()
    {
        Console.WriteLine("---------- You are in live session arena ---------------");
        List<Stack> allStacks = Db.GetAllStacks();
        if(allStacks.Count>0)
        {
            Console.WriteLine("----------Following is the list of all stacks ---------------");
            List<string> list = new List<string>();
            foreach (var stack in allStacks)
            {
                list.Add(stack.Name.ToString());
            }
            list.Add("Go to Main Menu");
            string stackName = Display.GetSelection("Select a stack from the list to start a study session", list);
            if (stackName != "Go to Main Menu")
            {
                int score = 0;
                List<FlashCardDtoStackView> Flashcards = Db.GetFlashCardsForGivenStack(stackName);
                if(Flashcards.Count > 0)
                {
                    foreach (var flashcard in Flashcards)
                    {
                        Console.WriteLine(flashcard.Question);
                        string answer = Console.ReadLine().ToLower().Trim();
                        if (answer == flashcard.Answer)
                        {
                            score++;
                            Console.WriteLine("Yay!! That was correct.. Press any key to continue...");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Sorry! It was the wrong answer.. Press any key to continue...");
                        }
                    }
                    string date = DateTime.Today.ToString("dd-MM-yyyy");
                    Db.InsertStudySession(stackName, date, score, Flashcards.Count);
                    Console.WriteLine("Session Completed. Your score was {0}/{1}. Press any key to continue...", score, Flashcards.Count);
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("There are no flashcards in this stack. Kindly insert some flashcards first.");
                    Console.Write("Press any key to continue...");
                    Console.ReadLine();
                }
            }
        }
        else
        {
            Console.WriteLine("There are no stacks present. Kindly create a stack and insert some flashcards first.");
            Console.Write("Press any key to continue...");
            Console.ReadLine();
        }
    }

    internal void ManageFlashCards()
    {
        string selection = Display.GetSelection("Manage FlashCards", new List<string> { "View All FlashCards", "Update FlashCards", "Delete FlashCards", "Create New FlashCard", "Return to Main Menu" });
        switch (selection)
        {
            case "View All FlashCards":
                List<FlashCardDtoFlashCardView> Flashcards = Db.GetFlashCards();
                if (Flashcards.Count > 0)
                {
                    Display.DisplayFlashCards(new string[] { "FlashCardId", "Front Side", "Back Side" }, Flashcards);
                    Console.Write("Press any key to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("There are no flashcards present yet. Kindly create some flashcards.");
                    Console.Write("Press any key to continue...");
                    Console.ReadLine();
                }
                break;
            case "Update FlashCards":
                Flashcards = Db.GetFlashCards();
                Display.DisplayFlashCards(new string[] { "FlashCardId", "Front Side", "Back Side" }, Flashcards);
                string flashCardId;
                do
                {
                    Console.Write("Enter a valid FlashCardId to update: ");
                    flashCardId = Console.ReadLine();
                } while (!InputValidator.IsGivenInputInteger(flashCardId));
                if (Db.IsFlashCardIdPresent(int.Parse(flashCardId)))
                {
                    Console.WriteLine("Enter the new front side of the flashcard: ");
                    string question = Console.ReadLine();
                    Console.WriteLine("Enter the new back side of the flashcard: ");
                    string answer = Console.ReadLine().ToLower().Trim();
                    Db.UpdateFlashCard(int.Parse(flashCardId), question, answer);
                }
                else
                {
                    Console.Write("The given FlashCardId is not present. Invalid Input.Press any key to continue...");
                    Console.ReadLine();
                }
                break;
            case "Delete FlashCards":
                Flashcards = Db.GetFlashCards();
                Display.DisplayFlashCards(new string[] { "FlashCardId", "Front Side", "Back Side" }, Flashcards);
                do
                {
                    Console.Write("Enter a valid FlashCardId to delete: ");
                    flashCardId = Console.ReadLine();
                } while (!InputValidator.IsGivenInputInteger(flashCardId));
                if (Db.IsFlashCardIdPresent(int.Parse(flashCardId)))
                {
                    Db.DeleteFlashCard(int.Parse(flashCardId));
                    Console.Write("The given FlashCard was deleted successfully.Press any key to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.Write("The given FlashCardId is not present. Invalid Input.Press any key to continue...");
                    Console.ReadLine();
                }
                break;
            case "Create New FlashCard":
                List<Stack> AllStacks = Db.GetAllStacks();
                List<string> list = new List<string>();
                foreach (var stack in AllStacks)
                {
                    list.Add(stack.Name.ToString());
                }
                list.Add("Go to Main Menu");
                string stackName = Display.GetSelection("----------Select a stack to create a flashcard in it---------------", list);
                if (stackName != "Go to Main Menu")
                {
                    Console.WriteLine("Enter the front side of the flashcard: ");
                    string question = Console.ReadLine();
                    Console.WriteLine("Enter the back side of the flashcard: ");
                    string answer = Console.ReadLine().ToLower().Trim();
                    Db.CreateFlashCard(stackName, question, answer);
                }
                break;
            case "Return to Main Menu":
                break;
        }
    }
}
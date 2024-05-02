using AdityaFlashCards.Database.DatabaseManager;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System;
using AdityaFlashCards.Database.Models;
using AdityaFlashCards.Helper;
using Microsoft.Extensions.Primitives;

namespace AdityaFlashCards
{
    internal class Application
    {
        public DatabaseManager Db { get; set; }
        public Application() {
            IConfiguration configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Db = new DatabaseManager(configurationBuilder);
        }

        internal void MainMenu()
        {
            bool RunApp = true;
            while (RunApp)
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
                    List<Stack> AllStacks = Db.GetAllStacks();
                    Console.WriteLine("----------Following is the list of all stacks ---------------");
                    List<string> list = new List<string>();
                    foreach (var stack in AllStacks)
                    {
                        list.Add(stack.Name.ToString());
                    }
                    list.Add("Go to Main Menu");
                    string option = Display.GetSelection("Select a stack to view from the list", list);
                    Console.Clear();
                    Console.WriteLine("----------Following are the flashcards in your stack ---------------");
                    if (option!= "Go to Main Menu")
                    {
                        Console.WriteLine("----------StackName: {0}---------- ", option);
                        List <FlashCardDTOStackView> Flashcards = Db.GetFlashCardsForGivenStack(option);
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
                    string name = AnsiConsole.Ask<string>("Enter the name of the new stack : ");
                    Db.CreateNewStack(name);
                    Console.Write("The new stack was created successfully. Press any key to continue...");
                    Console.ReadLine();
                    break;
                case "Delete Stack":
                    AllStacks = Db.GetAllStacks();
                    Console.WriteLine("----------Following is the list of stacks to delete ---------------");
                    list = new List<string>();
                    foreach (var stack in AllStacks)
                    {
                        list.Add(stack.Name.ToString());
                    }
                    list.Add("Exit without deletion");
                    option = Display.GetSelection("Select a stack to delete from the list", list);
                    Db.DeleteStack(option);
                    Console.Write("The new stack was deleted successfully. Press any key to continue...");
                    Console.ReadLine();
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
            Display.DisplayStudySessions(new string[] { "StackName", "SessionDate", "SessionScore" }, sessions);
            Console.Write("Press any key to continue...");
            Console.ReadLine();

        }

        internal void HandleStudy()
        {
            
        }

        internal void ManageFlashCards()
        {
            string selection = Display.GetSelection("Manage FlashCards", new List<string> { "View All FlashCards", "Update FlashCards", "Delete FlashCards", "Create New FlashCard", "Return to Main Menu" });
            switch (selection)
            {
                case "View All FlashCards":
                    List<FlashCardDTOFlashCardView> Flashcards = Db.GetFlashCards();
                    Display.DisplayFlashCards(new string[] { "FlashCardId", "Front Side", "Back Side" }, Flashcards);
                    Console.Write("Press any key to continue...");
                    Console.ReadLine();
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
                        string answer = Console.ReadLine();
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
                    string option = Display.GetSelection("----------Select a stack to create a flashcard in it---------------", list);
                    if (option != "Go to Main Menu")
                    {
                        Console.WriteLine("Enter the new front side of the flashcard: ");
                        string question = Console.ReadLine();
                        Console.WriteLine("Enter the new back side of the flashcard: ");
                        string answer = Console.ReadLine();
                        Db.CreateFlashCard(option, question, answer);
                    }
                    break;
                case "Return to Main Menu":
                    break;
            }

        }
    }

}

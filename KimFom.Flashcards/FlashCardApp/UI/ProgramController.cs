using FlashCardApp.Data;
using FlashCardApp.DTO;
using FlashCardApp.Input;
using FlashCardApp.Models;

namespace FlashCardApp.UI;

public static class ProgramController
{
    private static readonly IDatabaseManager DbManager = new SqlServerDatabaseManager();
    private static readonly UserInput Input = new();
    private static readonly TableVisualizationEngine DisplayTable = new();

    private static void ViewMainMenu()
    {
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("study to go to Study Area");
        Console.WriteLine("settings to go to Settings");
        Console.WriteLine("exit to End Program");
        Console.WriteLine("\nType your choice and hit Enter");
        Console.Write("Your choice? ");
    }

    private static void CreateTables()
    {
        DbManager.CreateStackTable();
        DbManager.CreateFlashCardTable();
        DbManager.CreateStudyAreaTable();
    }

    public static void StartProgram()
    {
        CreateTables();

        ViewMainMenu();
        var choice = Input.GetChoice();

        while (choice != "exit")
        {
            switch (choice)
            {
                case "study":
                    ManageStudyArea();
                    break;
                case "settings":
                    ManageStacksSettings();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    break;
            }

            ViewMainMenu();
            choice = Input.GetChoice();
        }
    }

    // Study Area Operations
    private static void ViewStudyAreaMenu()
    {
        Console.WriteLine("STUDY AREA\n");
        Console.WriteLine("new to Start a New Lesson");
        Console.WriteLine("history to View History");
        Console.WriteLine("back to Go Back");
        Console.WriteLine("\nType your choice and hit Enter");
        Console.Write("Your choice? ");
    }

    private static void ManageStudyArea()
    {
        Console.Clear();

        ViewStudyAreaMenu();
        var choice = Input.GetChoice();

        while (choice != "back")
        {
            switch (choice)
            {
                case "new":
                    StartLesson();
                    break;
                case "history":
                    ViewHistory();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    break;
            }

            ViewStudyAreaMenu();
            choice = Input.GetChoice();
        }

        Console.Clear();
    }

    private static void ViewHistory()
    {
        DisplayTable.ViewHistory();

        Console.WriteLine("Hit Enter to go back");
        Console.ReadLine();
        Console.Clear();
    }

    private static void ViewNewLessonMenu()
    {
        Console.WriteLine("Type a Stack Name to choose or back to Go Back");
        Console.Write("Your choice? ");
    }

    private static void StartLesson()
    {
        Console.Clear();

        DisplayTable.ViewStacks();

        ViewNewLessonMenu();
        var choice = Input.GetInput();
        if (choice == "back")
        {
            Console.Clear();
            return;
        }

        var stack = new Stack { Name = choice };
        var flashCards = DbManager.GetFlashCardsOfStack(stack);

        var score = PlayLessonLoop(flashCards);

        Console.Clear();
        Console.WriteLine($"Your final score is: {score}");
        Console.WriteLine("Hit Enter to return to previous menu.");
        Console.ReadLine();

        var studyArea = new StudyArea { Score = score };
        SaveScore(studyArea, stack);

        Console.Clear();
    }

    private static int PlayLessonLoop(List<FlashCardDTO> flashCards)
    {
        var score = 0;
        foreach (var card in flashCards)
        {
            Console.Clear();

            Console.WriteLine(card.Name);
            Console.Write("Your answer: ");
            var answer = Input.GetInput();

            if (answer.ToLower() != card.Content.ToLower())
            {
                Console.WriteLine("Incorrect!");
                Console.WriteLine("Correct answer is " + card.Content);
                Console.Write("Press any key to continue...");
                Console.ReadLine();
                continue;
            }

            Console.WriteLine("Correct");
            Console.Write("Press any key to continue...");
            Console.ReadLine();
            score++;
        }

        return score;
    }

    private static void SaveScore(StudyArea studyArea, Stack stack)
    {
        DbManager.SaveScore(studyArea, stack);
    }

    // Settings
    private static void DisplaySettingsMenu()
    {
        Console.WriteLine("SETTINGS\n");
        Console.WriteLine("create to Create a New Stack");
        Console.WriteLine("rename to Rename a Stack");
        Console.WriteLine("delete to Delete a Stack");
        Console.WriteLine("view to View List of Stacks");
        Console.WriteLine("back to Go Back");
        Console.WriteLine("\nType your choice and hit Enter");
        Console.Write("Your choice? ");
    }

    private static void ManageStacksSettings()
    {
        Console.Clear();

        DisplaySettingsMenu();
        var choice = Input.GetChoice();

        while (choice != "back")
        {
            switch (choice)
            {
                case "create":
                    GetStackToAdd();
                    break;
                case "rename":
                    UpdateStackName();
                    break;
                case "delete":
                    DeleteStack();
                    break;
                case "view":
                    ViewStackForFlashCardOperations();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    break;
            }

            DisplaySettingsMenu();
            choice = Input.GetChoice();
        }

        Console.Clear();
    }

    // Stack Operations
    private static void GetStackToAdd()
    {
        Console.Clear();

        Console.Write("Enter name to create or back to cancel: ");
        var name = Input.GetInput();
        if (name.ToLower() == "back")
        {
            Console.Clear();
            return;
        }

        DbManager.AddNewStack(new Stack { Name = name });
        Console.Clear();
    }

    private static void DisplayUpdateStackMenu()
    {
        Console.WriteLine("type the name of stack you want to rename");
        Console.WriteLine("back to Go Back");
        Console.Write("\nYour choice? ");
    }

    private static void UpdateStackName()
    {
        DisplayTable.ViewStacks();
        DisplayUpdateStackMenu();
        var choice = Input.GetChoice();
        while (choice != "back")
        {
            Console.Write("Enter new name or back to cancel: ");
            var newStackName = Input.GetInput();
            if (newStackName.ToLower() == "back")
            {
                Console.Clear();
                return;
            }
            DbManager.UpdateStack(new Stack { Name = choice }, new Stack { Name = newStackName });

            DisplayTable.ViewStacks();
            DisplayUpdateStackMenu();
            choice = Input.GetChoice();
        }

        Console.Clear();
    }

    private static void DisplayDeleteMenu()
    {
        Console.WriteLine("type the name of stack you want to delete");
        Console.WriteLine("back to Go Back");
        Console.Write("\nYour choice? ");
    }

    private static void DeleteStack()
    {
        DisplayTable.ViewStacks();
        DisplayDeleteMenu();
        var choice = Input.GetChoice();

        while (choice != "back")
        {
            DbManager.DeleteStack(new Stack { Name = choice });

            DisplayTable.ViewStacks();
            DisplayDeleteMenu();
            choice = Input.GetChoice();
        }

        Console.Clear();
    }

    private static void ViewStackForFlashCardOperations()
    {
        Console.Clear();
        DisplayTable.ViewStacks();

        SelectStackToOperateOn();
        Console.Clear();
    }

    private static void SelectStackToOperateOn()
    {
        Console.WriteLine("Type Stack Name and hit Enter to Perform Operations on a Stack: ");
        Console.WriteLine("back to Go Back");
        var name = Input.GetInput();
        if (name == "back")
        {
            Console.Clear();
            return;
        }

        ManageFlashCardSettings(new Stack { Name = name });
    }

    // FlashCard Operations

    private static void ViewFlashCardSettingsMenu()
    {
        Console.WriteLine("view to View FlashCards of the Stack");
        Console.WriteLine("add to Add a New FlashCard");
        Console.WriteLine("edit to Edit a FlashCard");
        Console.WriteLine("delete to Delete a FlashCard");
        Console.WriteLine("back to Go Back");
        Console.WriteLine("\nType your choice and hit Enter");
        Console.Write("Your choice? ");
    }

    private static void ManageFlashCardSettings(Stack stack)
    {
        Console.Clear();
        ViewFlashCardSettingsMenu();
        var choice = Input.GetChoice();

        while (choice != "back")
        {
            switch (choice)
            {
                case "view":
                    ViewFlashCards(stack);
                    break;
                case "add":
                    AddFlashCardToStack(stack);
                    break;
                case "edit":
                    EditFlashCard(stack);
                    break;
                case "delete":
                    DeleteFlashCard(stack);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    break;
            }

            ViewFlashCardSettingsMenu();
            choice = Input.GetChoice();
        }

        Console.Clear();
    }

    private static void ViewFlashCards(Stack stack)
    {
        Console.Clear();

        ViewFlashCardOfStack(stack);

        Console.WriteLine("Hit Enter to return to previous menu.");
        Console.ReadLine();
        Console.Clear();
    }
    
    private static void AddFlashCardToStack(Stack stack)
    {
        Console.Clear();

        Console.Write("Enter name of FlashCard to add or back to cancel: ");
        var name = Input.GetInput();
        if (name.ToLower() == "back")
        {
            Console.Clear();
            return;
        }

        Console.Write("Enter content of FlashCard: ");
        var content = Input.GetInput();

        var flashcard = new FlashCard { Name = name, Content = content };

        DbManager.AddNewFlashCard(flashcard, stack);
        Console.Clear();
    }

    private static void ViewEditFlashCardMenu()
    {
        Console.WriteLine("all to Edit Front and Back");
        Console.WriteLine("edit front to Edit Front");
        Console.WriteLine("edit back to Edit Back");
        Console.WriteLine("back to Go Back");
        Console.WriteLine("\nType your choice and hit Enter");
        Console.Write("Your choice? ");
    }

    private static void EditFlashCard(Stack stack)
    {
        Console.Clear();
        ViewEditFlashCardMenu();
        var choice = Input.GetChoice();

        while (choice != "back")
        {
            switch (choice)
            {
                case "all":
                    EditAll(stack);
                    break;
                case "edit front":
                    EditFlashCardName(stack);
                    break;
                case "edit back":
                    EditBack(stack);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    break;
            }

            ViewEditFlashCardMenu();
            choice = Input.GetChoice();
        }

        Console.Clear();
    }

    private static void EditAll(Stack stack)
    {
        ViewFlashCardOfStack(stack);
        Console.Write("Enter name of FlashCard to edit or back to cancel: ");
        var name = Input.GetChoice();
        if (name.ToLower() == "back")
        {
            Console.Clear();
            return;
        }

        Console.Write("Enter new name for FlashCard: ");
        var newName = Input.GetInput();

        Console.Write("Enter new content for FlashCard: ");
        var newContent = Input.GetInput();

        var oldFlashcard = new FlashCard { Name = name };
        var newFlashCard = new FlashCard { Name = newName, Content = newContent };

        DbManager.UpdateFlashCard(oldFlashcard, newFlashCard, stack);
        Console.Clear();
    }

    private static void EditFlashCardName(Stack stack)
    {
        ViewFlashCardOfStack(stack);
        Console.Write("Enter name of FlashCard to edit or back to cancel: ");
        var name = Input.GetChoice();
        if (name.ToLower() == "back")
        {
            Console.Clear();
            return;
        }

        Console.Write("Enter new name for FlashCard: ");
        var newName = Input.GetInput();

        var oldFlashcard = new FlashCard { Name = name };
        var newFlashCard = new FlashCard { Name = newName };

        DbManager.UpdateFlashCardName(oldFlashcard, newFlashCard, stack);
        Console.Clear();
    }

    private static void EditBack(Stack stack)
    {
        ViewFlashCardOfStack(stack);
        Console.Write("Enter name of FlashCard to edit or back to cancel: ");
        var name = Input.GetChoice();
        if (name.ToLower() == "back")
        {
            Console.Clear();
            return;
        }

        Console.Write("Enter new content for FlashCard: ");
        var newContent = Input.GetInput();

        var oldFlashcard = new FlashCard { Name = name };
        var newFlashCard = new FlashCard { Content = newContent };

        DbManager.UpdateFlashCardContent(oldFlashcard, newFlashCard, stack);
        Console.Clear();
    }

    private static void DeleteFlashCard(Stack stack)
    {
        ViewFlashCardOfStack(stack);
        Console.Write("Enter name of FlashCard to delete or back to cancel: ");
        var name = Input.GetChoice();
        if (name.ToLower() == "back")
        {
            Console.Clear();
            return;
        }

        var flashCard = new FlashCard { Name = name };

        DbManager.DeleteFlashCard(flashCard, stack);
        Console.Clear();
    }

    private static void ViewFlashCardOfStack(Stack stack)
    {
        DisplayTable.ViewFlashCards(stack);
    }
}
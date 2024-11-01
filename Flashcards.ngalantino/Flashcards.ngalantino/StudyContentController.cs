using System.Configuration;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Linq.Expressions;
using Spectre.Console;

public static class StudyContentController
{
    private static DatabaseManager db = new DatabaseManager();
    public static string SelectStack()
    {
        SelectionPrompt<string> prompt = new SelectionPrompt<string>();

        // Build the prompt
        foreach (Stack stack in db.GetStacks())
        {
            prompt.AddChoice(stack.name);
        }
        // Display the prompt
        Console.WriteLine("Select a stack:");
        string menuChoice = AnsiConsole.Prompt(prompt);

        return menuChoice;
    }

    public static string ManageStacks()
    {

        SelectionPrompt<string> prompt2 = new SelectionPrompt<string>();

        string mainMenuChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title($"Current working stack: {Menu.selectedStack}")
        .PageSize(10)
        .MoreChoicesText("(Move up and down to reveal more options)")
        .AddChoices(new[] {
                        "Return to main menu",
                        "Change current stack",
                        "View all stacks",
                        "Create a new stack",
                        "Delete a stack"
        }));

        return mainMenuChoice;

    }

    public static string ManageFlashcards()
    {

        SelectionPrompt<string> prompt2 = new SelectionPrompt<string>();

        string mainMenuChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title($"Current working stack: {Menu.selectedStack}")
        .PageSize(10)
        .MoreChoicesText("(Move up and down to reveal more options)")
        .AddChoices(new[] {
                        "Return to main menu",
                        "Change current stack",
                        "View all flashcards in stack",
                        "View X amount of cards in stack",
                        "Create a flashcard in current stack",
                        "Edit a flashcard",
                        "Delete a flashcard"
        }));

        return mainMenuChoice;

    }

    public static List<Flashcard> GetFlashcardsInStack(int numFlashcards = 0)
    {
        List<Flashcard> flashcards = db.GetFlashcards(Menu.selectedStack, numFlashcards);

        return flashcards;
    }

    public static List<Stack> GetStacks()
    {
        return db.GetStacks();
    }
    public static void CreateFlashcard()
    {
        if (!isStackSelected())
        {
            Console.WriteLine("Select a stack beforing adding a flashcard!");
            Menu.selectedStack = SelectStack();
        }

        Flashcard flashcard = new Flashcard();

        Console.WriteLine("Enter front of flashcard: ");

        flashcard.front = Console.ReadLine();

        Console.WriteLine("Enter back of flashcard.");

        flashcard.back = Console.ReadLine();

        flashcard.stack = Menu.selectedStack;


        db.AddFlashcard(flashcard);
    }

    public static void DeleteFlashcard()
    {
        Flashcard flashcard = new Flashcard();

        // Get flashcard to be deleted.
        List<Flashcard> flashcards = GetFlashcardsInStack();

        Console.WriteLine("Enter Id of flashcard to be deleted: ");
        int id = Int32.Parse(Console.ReadLine()) - 1;

        flashcard.id = flashcards[id].id;

        db.DeleteFlashcard(flashcard);
    }

    public static bool isStackSelected()
    {
        return !(Menu.selectedStack == "No stack selected");
    }

    public static void EditFlashcard()
    {
        if (!isStackSelected())
        {
            Console.WriteLine("Select a stack beforing updating a flashcard!");
            Menu.selectedStack = SelectStack();
        }

        Flashcard flashcard = new Flashcard();

        List<Flashcard> flashcards = GetFlashcardsInStack();

        Console.WriteLine("Enter Id of flashcard to be updated: ");
        int id = Int32.Parse(Console.ReadLine()) - 1;

        flashcard.id = flashcards[id].id;

        Console.WriteLine("Enter front of flashcard: ");

        flashcard.front = Console.ReadLine();

        Console.WriteLine("Enter back of flashcard.");

        flashcard.back = Console.ReadLine();

        flashcard.stack = Menu.selectedStack;


        db.EditFlashcard(flashcard);
    }

    public static void newStudySession(String stack, DateTime date, int score)
    {

        db.AddStudySession(stack, date, score);
    }

    public static List<StudySession> GetStudySessions()
    {
        List<StudySession> studySessions = db.GetStudySessions();

        return studySessions;
    }

    public static void NewStack()
    {
        bool loop = true;
        String? stackName = "";
        while (loop)
        {
            Console.WriteLine("Enter name of new stack.");

            stackName = Console.ReadLine();

            // Make sure stack name is unique
            foreach (Stack stack in GetStacks())
            {
                if (stack.name == stackName)
                {
                    Console.WriteLine("Another stack with this name already exists!");
                    loop = true;
                    break;
                }
                else {
                    loop = false;
                }
            }
        }
        db.NewStack(stackName);
    }

    public static void DeleteStack(String stackToDelete) {
        db.DeleteStack(stackToDelete);
    }
}

using Spectre.Console;

public static class StudyContentController
{
    private static DatabaseManager db = new DatabaseManager();
    public static string SelectStack()
    {
        SelectionPrompt<string> prompt = new SelectionPrompt<string>();
        String menuChoice = "";
        const String createStack = "Create a new stack";

        if (db.GetStacks().Count == 0)
        {
            Console.WriteLine("You don't have any saved stacks. Would you like to add a stack?");
            SelectionPrompt<string> prompt2 = new SelectionPrompt<string>();
            prompt2.AddChoice("Yes");
            prompt2.AddChoice("No");

            menuChoice = AnsiConsole.Prompt(prompt2);

            if (menuChoice == "No")
            {
                return "No stack selected";
            }
            else
            {
                Console.Clear();
                NewStack();
                Console.Clear();
            }
        }
        // Build the prompt
        prompt.AddChoice(createStack);

        foreach (Stack stack in db.GetStacks())
        {
            prompt.AddChoice(stack.Name);
        }
        // Display the prompt
        Console.WriteLine("Select a stack:");

        try
        {
            menuChoice = AnsiConsole.Prompt(prompt);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
        finally
        {
            Console.Clear();
        }

        if (menuChoice == createStack)
        {
            NewStack();
            return menuChoice = "No stack selected";
        }

        return menuChoice;
    }

    public static string ManageStacks()
    {

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
        if (!IsStackSelected())
        {
            Console.WriteLine("Select a stack beforing adding a flashcard!");
            Menu.selectedStack = SelectStack();
            if (Menu.selectedStack == "No stack selected")
            {
                Console.Clear();
                return;
            }
        }

        Flashcard flashcard = new Flashcard();
        bool validEntry = false;

        while (!validEntry)
        {
            Console.WriteLine("Enter front of flashcard: ");

            flashcard.Front = Console.ReadLine();

            Console.Clear();

            if (flashcard.Front.Trim() == "")
            {
                Console.WriteLine("Front of flashcard cannot be blank!");
            }
            else
            {
                validEntry = true;
            }

        }

        Console.Clear();

        validEntry = false;

        while (!validEntry)
        {
            Console.WriteLine("Enter back of flashcard.");

            flashcard.Back = Console.ReadLine();

            Console.Clear();

            if (flashcard.Back.Trim() == "")
            {
                Console.WriteLine("Back of flashcard cannot be blank!");
            }
            else
            {
                validEntry = true;
            }

        }

        flashcard.Stack = Menu.selectedStack;


        db.AddFlashcard(flashcard);
    }

    public static void DeleteFlashcard()
    {
        Flashcard flashcard = new Flashcard();

        // Get flashcard to be deleted.
        List<Flashcard> flashcards = GetFlashcardsInStack();

        Console.WriteLine("Enter Id of flashcard to be deleted (or enter 0 to cancel): ");

        int id = 0;

        try
        {
            id = Int32.Parse(Console.ReadLine()) - 1;
        }
        catch (FormatException e)
        {
            Console.WriteLine("Invalid entry!");
            DeleteFlashcard();
        }

        if (id + 1 == 0) {
            Console.Clear();
            return;
        }

        try
        {
            flashcard.Id = flashcards[id].Id;
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine("Flashcard id " + (id + 1) + " does not exist!");
            DeleteFlashcard();
        }

        db.DeleteFlashcard(flashcard);
    }

    public static bool IsStackSelected()
    {
        return !(Menu.selectedStack == "No stack selected");
    }

    public static void EditFlashcard()
    {
        if (!IsStackSelected())
        {
            Console.WriteLine("Select a stack beforing updating a flashcard!");
            Menu.selectedStack = SelectStack();
        }

        Flashcard flashcard = new Flashcard();

        List<Flashcard> flashcards = GetFlashcardsInStack();

        Console.WriteLine("Enter Id of flashcard to be updated: ");
        int id = Int32.Parse(Console.ReadLine()) - 1;

        try
        {
            flashcard.Id = flashcards[id].Id;
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine("Invalid entry!");
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            Console.Clear();
            return;
        }

        Console.WriteLine("Enter front of flashcard: ");

        flashcard.Front = Console.ReadLine();

        Console.WriteLine("Enter back of flashcard.");

        flashcard.Back = Console.ReadLine();

        flashcard.Stack = Menu.selectedStack;


        db.EditFlashcard(flashcard);
    }

    public static void NewStudySession(String stack, DateTime date, int score)
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

            if (GetStacks().Count == 0)
            {
                loop = false;
            }
            // Make sure stack name is unique
            foreach (Stack stack in GetStacks())
            {
                if (stack.Name == stackName)
                {
                    Console.WriteLine("Another stack with this name already exists!");
                    loop = true;
                    break;
                }
                else
                {
                    loop = false;
                }
            }
        }
        db.NewStack(stackName);
    }

    public static void DeleteStack(String stackToDelete)
    {
        if (stackToDelete == Menu.selectedStack)
        {
            Menu.selectedStack = "No stack selected";
        }
        db.DeleteStack(stackToDelete);
    }
}

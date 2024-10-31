using System.Data.Common;
using System.Formats.Asn1;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Spectre.Console;
public static class Menu
{
    private static string mainMenuChoice = "";
    public static string selectedStack = "No stack selected";
    public static void DisplayMenu()
    {

        while (mainMenuChoice != "Exit")
        {
            Console.Clear();
            // Create main prompt
            mainMenuChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select an option")
            .PageSize(10)
            .MoreChoicesText("(Move up and down to reveal more options)")
            .AddChoices(new[] {
                "Exit",
                "Manage stacks",
                "Manage flashcards",
                "Study",
                "View study session data"
            }));

            switch (mainMenuChoice.ToLower())
            {
                case "manage stacks":

                    while (mainMenuChoice != "Return to main menu")
                    {
                        Console.Clear();

                        mainMenuChoice = StudyContentController.ManageStacks();

                        switch (mainMenuChoice.ToLower())
                        {

                            case "change current stack":
                                selectedStack = StudyContentController.SelectStack();
                                break;

                            case "view all stacks":
                                List<Stack> stacks = StudyContentController.GetStacks();

                                DisplayTable(stacks);

                                Console.WriteLine("Press enter to continue...");

                                Console.ReadLine();

                                break;

                            case "create a new stack":
                                StudyContentController.NewStack();
                                break;

                            case "edit a stack":

                                break;

                            case "delete a stack":

                                break;
                        }

                    }

                    break;

                case "manage flashcards":
                    while (mainMenuChoice != "Return to main menu")
                    {

                        mainMenuChoice = StudyContentController.ManageFlashcards();

                        switch (mainMenuChoice)
                        {
                            case "Return to main menu":

                                break;

                            case "Change current stack":
                                selectedStack = StudyContentController.SelectStack();
                                break;

                            case "View all flashcards in stack":
                                Console.Clear();
                                List<Flashcard> flashcards = StudyContentController.GetFlashcardsInStack();

                                DisplayTable(flashcards);

                                break;

                            case "View X amount of cards in stack":
                                Console.WriteLine("How many flashcards do you want to view?");
                                int numFlashcards = Int32.Parse(Console.ReadLine());
                                List<Flashcard> xAmountofFlashcards = StudyContentController.GetFlashcardsInStack(numFlashcards);

                                DisplayTable(xAmountofFlashcards);

                                break;

                            case "Create a flashcard in current stack":
                                StudyContentController.CreateFlashcard();
                                break;

                            case "Edit a flashcard":
                                StudyContentController.EditFlashcard();
                                break;

                            case "Delete a flashcard":
                                StudyContentController.DeleteFlashcard();
                                break;

                            default:
                                break;
                        }
                    }

                    break;

                case "study":
                    // Select stack
                    if (selectedStack == "No stack selected")
                    {
                        selectedStack = StudyContentController.SelectStack();
                        Console.Clear();
                    }


                    // Begin study session by getting first flashcard in stack and iterating
                    List<Flashcard> studyFlashcards = StudyContentController.GetFlashcardsInStack();

                    string answer = "";
                    int score = 0;
                    DateTime dateTime = DateTime.Now;

                    while (answer != "0")
                    {
                        foreach (Flashcard flashcard in studyFlashcards)
                        {
                            Console.WriteLine(flashcard.front);
                            Console.WriteLine("Input your answer or press 0 to exit.");
                            answer = Console.ReadLine();

                            if (answer == "0")
                            {
                                break;
                            }

                            // Check answer
                            if (answer.ToLower().Equals(flashcard.back.ToLower()))
                            {
                                Console.WriteLine("Correct!");
                                score++;
                            }
                            else
                            {
                                Console.WriteLine("Wrong!");
                            }
                            // Calculate score
                        }

                        // Add study session
                    }
                    StudyContentController.newStudySession(selectedStack, dateTime, score);

                    // Create new study session with date.

                    // Iterate over stack
                    // Show front of card
                    // Get answer
                    // Keep track of score
                    // Insert score date and score into study session table.
                    break;

                case "view study session data":

                    DisplayTable(StudyContentController.GetStudySessions());
                    Console.WriteLine("Press any key to return.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    public static void DisplayTable(List<Flashcard> flashcards)
    {
        // Display table with ID, front, and back
        Table table = new Table();

        table.AddColumn("Id");
        table.AddColumn("Front");
        table.AddColumn("Back");

        int id = 1;

        foreach (Flashcard flashcard in flashcards)
        {
            table.AddRow(id.ToString(), flashcard.front, flashcard.back);
            id++;
        }

        AnsiConsole.Write(table);
    }

    public static void DisplayTable(List<StudySession> studySessions)
    {
        Table table = new Table();

        table.AddColumn("Stack");
        table.AddColumn("Date");
        table.AddColumn("Score");

        foreach (StudySession session in studySessions)
        {
            table.AddRow(session.stack, session.date.ToString(), session.score.ToString());
        }

        AnsiConsole.Write(table);
    }

    public static void DisplayTable(List<Stack> stacks) {

        Table table = new Table();

        table.AddColumn("Stacks");

        foreach (Stack stack in stacks) {
            table.AddRow(stack.name);
        }

        AnsiConsole.Write(table);
    }
}
using System.Data.Common;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using Spectre.Console;
public static class Menu
{
    private static string userInput = "";
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

            // Switch statement for menu options
            switch (mainMenuChoice.ToLower())
            {
                case "manage stacks":

                    selectedStack = StudyContentController.SelectStack();

                    Console.Clear();

                    string mainMenuChoice2 = StudyContentController.ManageStacks();

                    // TODO: Add switch statement for sub menu choices to manage stack

                    break;

                case "manage flashcards":
                    while (mainMenuChoice != "Return to main menu")
                    {

                        mainMenuChoice = StudyContentController.ManageFlashcards();

                        // Add nested switch statement for submenu
                        switch (mainMenuChoice)
                        {
                            case "Return to main menu":

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

                            if (answer == "0") {
                                break;
                            }

                            // Check answer
                            if (answer.ToLower().Equals(flashcard.back.ToLower())) {
                                Console.WriteLine("Correct!");
                                score++;
                            }
                            else {
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

    public static void DisplayTable(List<StudySession> studySessions) {
        Table table = new Table();

        table.AddColumn("Stack");
        table.AddColumn("Date");
        table.AddColumn("Score");

        foreach (StudySession session in studySessions) {
            table.AddRow(session.stack, session.date.ToString(), session.score.ToString());
        }

        AnsiConsole.Write(table);
    }
}
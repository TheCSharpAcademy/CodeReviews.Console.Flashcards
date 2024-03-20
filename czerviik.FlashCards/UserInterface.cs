using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace FlashCards;

public static class UserInterface
{
    public static string? OptionChoice { get; private set; }
    public static void MainMenu()
    {
        Header("main menu");

        string[] options = {
                "New Study Session",
                "New Flashcard",
                "Show Stacks",
                "Show Study Sessions",
                "Reports",
                "Exit"};

        ChooseOptions(options);
    }

    public static void StudySessionOptions(string[] stacks)
    {
        Header("new study session");
        Console.WriteLine("Select a stack");

        var modifiedStacks = new string[stacks.Length + 1];
        stacks.CopyTo(modifiedStacks, 0);
        modifiedStacks[^1] = "Go back";

        ChooseOptions(modifiedStacks);
    }

    public static void StudySessionQuestion(Stack stack, FlashcardSessionDto flashcard, int roundNo)
    {
        Header($"{stack.Name} study session");
        Console.WriteLine($"Round {roundNo}");
        Console.WriteLine($"\nFront side:\n{flashcard.Question}");
        Console.WriteLine("\nBack side:");
    }

    public static void CorrectAnswer(int score, int totalRounds)
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine($"Correct!({score}/{totalRounds})");
        Console.ResetColor();
        UserInput.DisplayMessage();
    }

    public static void WrongAnswer(int score, int totalRounds, FlashcardSessionDto flashcard)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine($"\nWrong!({score}/{totalRounds})");
        Console.ResetColor();
        Console.WriteLine($"Correct answer was: {flashcard.Answer}");
        UserInput.DisplayMessage();
    }

    public static void FinalizeSession(int score, int totalRounds, Stack stack)
    {
        Header($"{stack.Name} study session");
        Console.WriteLine("Session over!");
        Console.WriteLine($"You scored:{score}/{totalRounds}");
        Console.WriteLine("\nTry again?");
        ChooseOptions(["Yes", "No"]);

    }

    public static void NewFlashcard(string[] stacks)
    {
        Header("new flashcard");

        if (stacks.Length != 0)
        {
            Console.WriteLine("Select a stack");

            var modifiedStacks = new string[stacks.Length + 2];
            stacks.CopyTo(modifiedStacks, 0);
            modifiedStacks[^2] = "Create a new stack";
            modifiedStacks[^1] = "Go back";

            ChooseOptions(modifiedStacks);
        }
    }

    public static void ShowStacks(string[] stacks)
    {
        string[] modifiedStacks;
        Header("show stacks");

        if (stacks.Length != 0)
        {
            Console.WriteLine("Select a stack");

            modifiedStacks = new string[stacks.Length + 2];
            stacks.CopyTo(modifiedStacks, 0);
            modifiedStacks[^2] = "Show all";

            modifiedStacks[^1] = "Go back";

            ChooseOptions(modifiedStacks);
        }
    }

    public static void DeleteStack(string[] stacks)
    {
        string[] modifiedStacks;
        Header("delete a stack");

        if (stacks.Length != 0)
        {
            Console.WriteLine("Select a stack");

            modifiedStacks = new string[stacks.Length + 1];
            stacks.CopyTo(modifiedStacks, 0);

            modifiedStacks[^1] = "Go back";

            ChooseOptions(modifiedStacks);
        }
    }

    public static void DeleteStackConfirm(Stack stack)
    {
        Console.Clear();
        Console.WriteLine($"Do you really want to delete {stack.Name}? (all stack's flashcards will be lost)\n");
        ChooseOptions(["Yes", "No"]);
    }
    public static void ShowFlashcards(List<FlashcardReviewDto> flashcards, Stack stack)
    {
        string[] options = {
                "Update a Flashcard",
                "Delete a Flashcard",
                "Delete a Stack",
                "Go back"};

        Header("show stacks");
        FlashcardsTable(flashcards, stack);

        if (flashcards.Count == 0)
        {
            UserInput.DisplayMessage("No Flashcards in this stack.", "go back");
            OptionChoice = "Go back";
        }
        else
            ChooseOptions(options);

    }

    public static void ShowFlashcards(List<FlashcardReviewDto> flashcards, List<Stack> stacks)
    {
        string[] options = {
                "Update a Flashcard",
                "Delete a Flashcard",
                "Delete a Stack",
                "Go back"};

        Header("show stacks");
        FlashcardsTable(flashcards, stacks);

        if (flashcards.Count == 0)
        {
            UserInput.DisplayMessage("No Flashcards in this stack.");
            OptionChoice = "Go back";
        }

        else
            ChooseOptions(options);
    }

    public static void NewFlashcardQuestion(string currentStack)
    {
        Header($"new {currentStack} flashcard");

        Console.WriteLine("Enter a question (esc - Go back):");
    }

    public static void NewFlashcardAnswer(string currentStack, string question)
    {
        Header($"new {currentStack} flashcard");
        Console.WriteLine($"Question: {question}");
        Console.WriteLine("Enter an answer (esc - Go back):");
    }

    public static void NewFlashcardConfirm(string currentStack, string question, string answer)
    {
        Header($"new {currentStack} flashcard");
        Console.WriteLine($"Stack: {currentStack}");
        Console.WriteLine($"Question: {question}");
        Console.WriteLine($"Answer: {answer}");
        Console.WriteLine();
        ChooseOptions(["Confirm", "Enter again"]);
    }

    public static void UpdateFlashcardQuestion(int id)
    {
        Header($"update flashcard #{id}");

        Console.WriteLine("Enter a question (esc - Go back):");
    }

    public static void UpdateFlashcardAnswer(int id, string question)
    {
        Header($"update flashcard #{id}");
        Console.WriteLine($"Question: {question}");
        Console.WriteLine("Enter an answer (esc - Go back):");
    }

    public static void UpdateFlashcardConfirm(int id, string question, string answer, string currentStack)
    {
        Header($"update flashcard #{id}");
        Console.WriteLine($"Stack: {currentStack}");
        Console.WriteLine($"Question: {question}");
        Console.WriteLine($"Answer: {answer}");
        Console.WriteLine();
        ChooseOptions(["Confirm", "Enter again"]);
    }

    public static void DeleteFlashcardConfirm(int id)
    {
        Console.WriteLine();
        Console.WriteLine($"Delete flashcard #{id}?");
        ChooseOptions(["Yes", "No"]);
    }

    public static void AnotherFlashcard()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Flashcard saved!");
        Console.ResetColor();
        Console.WriteLine();
        ChooseOptions(["Add another flashcard", "Done"]);
    }
    public static void NewStack()
    {
        Header("create new stack");
        Console.WriteLine("Enter stack's name: ");
    }
    public static void UpdateFlashcard(List<FlashcardReviewDto> flashcards, Stack stack)
    {
        Header("show stacks");
        FlashcardsTable(flashcards, stack);
        Console.WriteLine("Type a Flashcard Id to Update:\n");
    }

    public static void DeleteFlashcard(List<FlashcardReviewDto> flashcards, Stack stack)
    {
        Header("show stacks");
        FlashcardsTable(flashcards, stack);
        Console.WriteLine("Type a Flashcard Id to Delete:\n");
    }

    public static void ShowStudySessions(List<StudySession> studySessions, Dictionary<int, string> stackIdDict)
    {
        Header("show study sessions");
        SessionsTable(studySessions, stackIdDict);
    }

    public static void ReportsMenu()
    {
        Header("reports");
        ChooseOptions(["Number of sessions/month", "Average score/month", "Go back"]);
    }

    public static void ShowYears(string[] years)
    {
        Console.Clear();
        Console.WriteLine("Choose a year of your Report:");

        var modifiedYears = new string[years.Length + 1];
        years.CopyTo(modifiedYears, 0);
        modifiedYears[^1] = "Go back";

        ChooseOptions(modifiedYears);
    }

    public static void NumberOfSessionsReport(Dictionary<int, Dictionary<int, int>> sessionCount, Dictionary<int, string> stackIdDict, string year)
    {
        Header($"number of sessions per month of {year} per stack report");
        NumberOfSessionsTable(sessionCount, stackIdDict);
    }

    private static void Header(string headerText)
    {
        Console.Clear();
        Console.WriteLine($"----- {headerText.ToUpper()} -----");
        Console.WriteLine();
    }

    private static void ChooseOptions(string[] options)
    {
        OptionChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle("red")
            .AddChoices(options)
            );
    }

    private static void FlashcardsTable(List<FlashcardReviewDto> flashcards, Stack stack)
    {
        var table = new Table()
        .AddColumns("Id", "Front side", "Back side")
        .Title(stack.Name)
        .Border(TableBorder.Rounded);
        foreach (var flashcard in flashcards)
        {
            if (flashcard.StackId == stack.Id)
                table.AddRow(flashcard.DisplayId.ToString(), flashcard.Question, flashcard.Answer);
        }
        AnsiConsole.Write(table);
        Console.WriteLine();
    }

    private static void FlashcardsTable(List<FlashcardReviewDto> flashcards, List<Stack> stacks)
    {
        foreach (var stack in stacks)
        {
            var table = new Table()
            .AddColumns("Id", "Front side", "Back side")
            .Title(stack.Name)
            .Border(TableBorder.Rounded);
            foreach (var flashcard in flashcards)
            {
                if (flashcard.StackId == stack.Id)
                    table.AddRow(flashcard.DisplayId.ToString(), flashcard.Question, flashcard.Answer);
            }
            AnsiConsole.Write(table);
            Console.WriteLine();
        }
    }

    private static void SessionsTable(List<StudySession> sessions, Dictionary<int, string> stackIdDict)
    {
        var table = new Table()
        .AddColumns("Id", "Stack", "Score", "Rounds", "Date")
        .Border(TableBorder.Rounded);
        foreach (var session in sessions)
        {
            table.AddRow(session.Id.ToString(), stackIdDict[session.StackId], session.Score.ToString(), session.Rounds.ToString(), session.Date.ToString("yyyy-MM-dd"));
        }
        AnsiConsole.Write(table);
    }

    private static void NumberOfSessionsTable(Dictionary<int, Dictionary<int, int>> sessionCount, Dictionary<int, string> stackIdDict)
    {
        var table = new Table()
        .AddColumns("Stack name", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec")
        .Border(TableBorder.Rounded);

        foreach (var stackIdPair in stackIdDict)
        {
            var row = new List<string> { stackIdPair.Value };
            for (int month = 1; month <= 12; month++)
            {
                row.Add(sessionCount.ContainsKey(stackIdPair.Key) && sessionCount[stackIdPair.Key].ContainsKey(month)
                        ? sessionCount[stackIdPair.Key][month].ToString()
                        : "0");
            }
            table.AddRow(row.ToArray());
        }
        AnsiConsole.Write(table);
    }
}



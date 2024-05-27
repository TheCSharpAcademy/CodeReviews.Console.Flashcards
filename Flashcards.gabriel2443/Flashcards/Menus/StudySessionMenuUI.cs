using Flashcards.Database;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Menus;

internal class StudySessionMenuUI
{
    private StudySessionDatabase studySessionDatabase = new StudySessionDatabase();

    internal void StudySessionMenu()
    {
        var mainMenu = new MainMenuUI();
        bool isRunning = true;

        while (isRunning)
        {
            var select = new SelectionPrompt<string>();
            select.Title("  [bold]Study session MENU[/]\n");
            select.AddChoice("Go back to main menu");
            select.AddChoice("Start study session");
            select.AddChoice("View study session");
            var selectedOption = AnsiConsole.Prompt(select);

            switch (selectedOption)
            {
                case "Start study session":
                    StartSession();
                    break;

                case "Go back to main menu":
                    mainMenu.MainMenu();
                    break;

                case "View study session":
                    ViewStudySession();
                    break;
            }
        }
    }

    internal void StartSession()
    {
        Console.Clear();
        var random = new Random();
        var stackDatabaseManager = new StackDatabaseManager();
        var flashDatabaseManager = new FlashcardDatabaseManager();
        var studySession = new StudySession();
        int score = 0;
        int counter = 0;

        AnsiConsole.WriteLine("  Please select a stack you want to study");

        var cardStacks = stackDatabaseManager.GetStacks();
        if (cardStacks.Count() == 0)
        {
            AnsiConsole.WriteLine("  There are no stacks to study, please add a stack if you want to start a study session");
            return;
        }
        var select = new SelectionPrompt<CardStack>();
        select.Title("Select a stack");
        select.AddChoices(cardStacks);
        select.AddChoice(new CardStack { CardstackId = 0, CardstackName = "Go back to menu" });
        select.UseConverter(stackName => stackName.CardstackName);
        var selectedCardStack = AnsiConsole.Prompt(select);
        if (selectedCardStack.CardstackName == "Go back to menu") return;

        var getFlashcards = flashDatabaseManager.ReadFlashcardsDTO(selectedCardStack);
        AnsiConsole.WriteLine($"In the stack {selectedCardStack.CardstackName.ToUpper()} are {getFlashcards.Count()} flashcards, press S to start or 0 to go back to menu");
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input)) return;
        if (input == "0") return;
        studySession.DateStart = DateTime.Now;
        if (input.ToLower() == "s")
        {
            for (int i = 0; i < getFlashcards.Count; i++)
            {
                var randomFlashcard = random.Next(getFlashcards.Count);
                var flashcard = getFlashcards[randomFlashcard];
                AnsiConsole.WriteLine($" {++counter}.What is the answer to this flashcard {flashcard.Question} ");
                var answer = AnsiConsole.Prompt(new TextPrompt<string>("Please type your answer or press 0 to go back to study menu "));
                if (answer == "0") StudySessionMenu();
                if (answer.ToLower() != flashcard.Answer.ToLower())
                {
                    Console.Clear();
                    AnsiConsole.WriteLine($"You are incorrect, the answer to the quesstion is {flashcard.Answer}.");
                }

                if (answer.ToLower() == flashcard.Answer.ToLower())
                {
                    Console.Clear();
                    AnsiConsole.WriteLine("You are correct ");
                    score++;
                }
            }
        }
        var stackId = selectedCardStack.CardstackId;
        studySession.DateEnd = DateTime.Now;
        studySession.Score = score;
        studySession.QuestionCounter = counter;
        studySession.StackId = stackId;
        studySessionDatabase.AddStudySession(studySession);
    }

    internal void ViewStudySession()
    {
        Console.Clear();
        var studySessions = studySessionDatabase.GetStudySession();

        foreach (var session in studySessions)
        {
            var table = new Table();
            var stackName = studySessionDatabase.GetStackName(session);
            table.AddColumn(new TableColumn("Start Time").Centered());
            table.AddColumn(new TableColumn("End Time").Centered());
            table.AddColumn(new TableColumn("Score").Centered());
            table.AddColumn(new TableColumn("Stack").Centered());
            table.AddColumn(new TableColumn("Number of Questions").Centered());
            table.AddRow($"{session.DateStart}", $"{session.DateEnd}", $"{session.Score}", $"{stackName}", $"{session.QuestionCounter}");
            AnsiConsole.Write(table);
        }
    }
}
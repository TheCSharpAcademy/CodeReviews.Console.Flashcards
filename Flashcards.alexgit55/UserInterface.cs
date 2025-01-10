using Flashcards.alexgit55.Models;
using Spectre.Console;
using static Flashcards.alexgit55.Enums;

namespace Flashcards.alexgit55;

internal class UserInterface
{
    internal static void DisplayMainMenu()
    {
        var isMenuRunning = true;
        var menuMessage = "Press any key to return to the Main Menu.";

        while (isMenuRunning)
        {
            DisplayHeader("Welcome to the Flashcards App!");

            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Enums.MainMenuChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        Enums.MainMenuChoices.ManageStacks, 
                        Enums.MainMenuChoices.ManageFlashcards, 
                        Enums.MainMenuChoices.StudySession,
                        Enums.MainMenuChoices.StudyHistory,
                        Enums.MainMenuChoices.Quit)
            );

            switch (usersChoice)
            {
                case MainMenuChoices.ManageStacks:
                    StacksMenu();
                    break;
                case MainMenuChoices.ManageFlashcards:
                    FlashcardsMenu();
                    break;
                case MainMenuChoices.StudySession:
                    StudySession();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case MainMenuChoices.StudyHistory:
                    ViewStudyHistory();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case MainMenuChoices.Quit:
                    DisplayHeader("Thank you for using the Flashcards App!");
                    menuMessage = "Goodbye!";
                    isMenuRunning = false;
                    break;
            }
        }
    }

    private static void StacksMenu()
    {
        var isMenuRunning = true;
        var menuMessage = "Press any key to return to the Stacks Menu";

        while (isMenuRunning)
        {
            DisplayHeader("Stacks Menu");
            var usersChoice = AnsiConsole.Prompt(
                   new SelectionPrompt<StackChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(
                       StackChoices.ViewStacks,
                       StackChoices.AddStack,
                       StackChoices.UpdateStackName,
                       StackChoices.DeleteStack,
                       StackChoices.ReturnToMainMenu)
                    );

            switch (usersChoice)
            {
                case StackChoices.ViewStacks:
                    ViewStacks();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case StackChoices.AddStack:
                    AddStack();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case StackChoices.DeleteStack:
                    DeleteStack();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case StackChoices.UpdateStackName:
                    UpdateStack();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case StackChoices.ReturnToMainMenu:
                    isMenuRunning = false;
                    break;
            }
        }
    }

    private static void FlashcardsMenu()
    {
        var isMenuRunning = true;
        var menuMessage = "Press any key to return to the Flashcards Menu";

        while (isMenuRunning)
        {
            DisplayHeader("Flashcards Menu");
            var usersChoice = AnsiConsole.Prompt(
                   new SelectionPrompt<FlashcardChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(
                       FlashcardChoices.ViewFlashcards,
                       FlashcardChoices.AddFlashcard,
                       FlashcardChoices.UpdateFlashcard,
                       FlashcardChoices.DeleteFlashcard,
                       FlashcardChoices.ReturnToMainMenu)
                    );

            switch (usersChoice)
            {
                case FlashcardChoices.ViewFlashcards:
                    ViewFlashcards();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case FlashcardChoices.AddFlashcard:
                    AddFlashcard();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case FlashcardChoices.DeleteFlashcard:
                    DeleteFlashcard();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case FlashcardChoices.UpdateFlashcard:
                    UpdateFlashcard();
                    AnsiConsole.MarkupLine($"\n{menuMessage}\n");
                    Console.ReadKey();
                    break;
                case FlashcardChoices.ReturnToMainMenu:
                    isMenuRunning = false;
                    break;
            }
        }
    }

    private static int ChooseStack(string message)
    {
        var dataAccess = new DataAccess();
        var stacks = dataAccess.GetAllStacks();

        var stacksArray = stacks.Select(x => x.Name).ToArray();
        var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(message)
            .AddChoices(stacksArray));

        var stackId = stacks.Single(x => x.Name == option).Id;

        return stackId;
    }

    private static void UpdateStack()
    {
        var stack = new Stack
        {
            Id = ChooseStack("Choose stack to update:"),
            Name = AnsiConsole.Ask<string>("Insert Stack's Name.")
        };

        var dataAccess = new DataAccess();
        dataAccess.UpdateStack(stack);
    }

    private static void DeleteStack()
    {
        var id = ChooseStack("Choose stack to delete");

        if (!AnsiConsole.Confirm("Are you sure?"))
            return;

        var dataAccess = new DataAccess();
        dataAccess.DeleteStack(id);
    }

    private static void AddStack()
    {
        Stack stack = new()
        {
            Name = AnsiConsole.Ask<string>("Insert Stack's New Name: ")
        };

        while (string.IsNullOrEmpty(stack.Name))
        {
            stack.Name = AnsiConsole.Ask<string>("The name can't be empty. Try again.");
        }

        var dataAccess = new DataAccess();
        dataAccess.InsertStack(stack);
    }

    private static void ViewStacks()
    {
        var dataAccess = new DataAccess();
        var stackList = dataAccess.GetStackListData();

        var table = new Table();
        table.AddColumn("StackName");
        table.AddColumn("Flashcards");

        foreach (var stack in stackList)
        {
            table.AddRow(stack.StackName, stack.FlashcardCount.ToString());
        }

        AnsiConsole.Write(table);
    }

    private static int ChooseFlashcard(string message, int stackId)
    {
        var dataAccess = new DataAccess();
        var flashcards = dataAccess.GetFlashcards(stackId);

        var flashcardsArray = flashcards.Select(x => x.Question).ToArray();
        var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(message)
            .AddChoices(flashcardsArray));

        var flashcardId = flashcards.Single(x => x.Question == option).Id;

        return flashcardId;
    }

    private static void UpdateFlashcard()
    {
        var stackId = ChooseStack("Choose stack where flashcard is:");
        var flashcardId = ChooseFlashcard("Choose flashcard to update", stackId);

        var propertiesToUpdate = new Dictionary<string, object>();

        if (AnsiConsole.Confirm("Would you like to update question?"))
        {
            var question = GetQuestion();
            propertiesToUpdate.Add("Question", question);
        }

        if (AnsiConsole.Confirm("Would you like to update answer?"))
        {
            var answer = GetAnswer();
            propertiesToUpdate.Add("Answer", answer);
        }

        if (AnsiConsole.Confirm("Would you like to update stack?"))
        {
            var stack = ChooseStack("Choose new stack for flashcard");

            propertiesToUpdate.Add("StackId", stack);
        }

        var dataAccess = new DataAccess();
        dataAccess.UpdateFlashcard(flashcardId, propertiesToUpdate);
    }

    private static string GetQuestion()
    {
        var question = AnsiConsole.Ask<string>("Insert Question: ");

        while (string.IsNullOrEmpty(question))
        {
            question = AnsiConsole.Ask<string>("Question can't be empty. Try again.");
        }

        return question;
    }

    private static string GetAnswer()
    {
        var answer = AnsiConsole.Ask<string>("Insert answer: ");

        while (string.IsNullOrEmpty(answer))
        {
            answer = AnsiConsole.Ask<string>("Answer can't be empty. Try again.");
        }

        return answer;
    }

    private static void DeleteFlashcard()
    {
        var stackId = ChooseStack("Where's the flashcard you want to delete?");
        var flashcard = ChooseFlashcard("Choose flashcard to delete", stackId);

        if (!AnsiConsole.Confirm("Are you sure?"))
            return;

        var dataAccess = new DataAccess();
        dataAccess.DeleteFlashcard(flashcard);
    }

    private static void AddFlashcard()
    {
        Flashcard flashcard = new();

        flashcard.StackId = ChooseStack("Choose a stack for the new flashcard");
        flashcard.Question = GetQuestion();
        flashcard.Answer = GetAnswer();

        var dataAccess = new DataAccess();
        dataAccess.InsertFlashcard(flashcard);
    }

    private static void ViewFlashcards()
    {
        var stackId = ChooseStack("Choose stack to view flashcards");
        var dataAccess = new DataAccess();
        var flashcards = dataAccess.GetFlashcards(stackId);
        var count = 1;

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Question");
        table.AddColumn("Answer");

        foreach (var flashcard in flashcards)
        {
            table.AddRow(count.ToString(),flashcard.Question, flashcard.Answer);
            count++;
        }

        AnsiConsole.Write(table);
    }

    internal static void StudySession()
    {
        DisplayHeader("Study Session");

        var id = ChooseStack("Choose stack to study");

        var dataAccess = new DataAccess();
        var flashcards = dataAccess.GetFlashcards(id);
        var question = 1;

        var studySession = new StudySession();
        studySession.Questions = flashcards.Count();
        studySession.StackId = id;
        studySession.Date = DateTime.Now;

        var correctAnswers = 0; // tracking the results

        foreach (var flashcard in flashcards)
        {
            Console.WriteLine($"Question {question} of {studySession.Questions}");
            var answer = AnsiConsole.Ask<string>($"{flashcard.Question}: ");

            // We're only checking if the answer is empty. 
            while (string.IsNullOrEmpty(answer))
                answer = AnsiConsole.Ask<string>($"Answer can't be empty. {flashcard.Question}: ");

            // this ignores leading and trailing whitespaces and the case of the characters
            if (string.Equals(answer.Trim(), flashcard.Answer, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                Console.WriteLine($"Correct!\n"); // \n adds a new line for better readability
            }
            else
            {
                Console.WriteLine($"Wrong, the answer is {flashcard.Answer}\n");
            }

            question++;

        }

        Console.WriteLine($"You've got {correctAnswers} out of {flashcards.Count()}!");
        studySession.CorrectAnswers = correctAnswers;
        //calculating the time of the session based on initial and current time
        studySession.Time = DateTime.Now - studySession.Date;

        dataAccess.InsertStudySession(studySession);
    }

    internal static void ViewStudyHistory()
    {
        DisplayHeader("Study History");

        var dataAccess = new DataAccess();
        var sessions = dataAccess.GetStudySessionData();

        var table = new Table();

        table.AddColumn("Date");
        table.AddColumn("Stack");
        table.AddColumn("Result");
        table.AddColumn("Percentage");
        table.AddColumn("Duration");

        foreach (var session in sessions)
        {
            table.AddRow(session.Date.ToShortDateString(), session.StackName, $"{session.CorrectAnswers} out of {session.Questions}", $"{session.Percentage}%", session.Time.ToString());
        }

        AnsiConsole.Write(table);
    }

    private static void DisplayHeader(string message)
    // This method displays a header with a message using the Spectre.Console library.
    {
        Console.Clear();
        string divider = new string('-', message.Length);
        AnsiConsole.MarkupLine($"[bold green]{divider}[/]");
        AnsiConsole.MarkupLine($"[bold green]{message}[/]");
        AnsiConsole.MarkupLine($"[bold green]{divider}[/]\n");
    }
}

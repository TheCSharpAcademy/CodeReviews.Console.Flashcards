using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Flashcards.RyanW84.Models;
using Spectre.Console;
using static Flashcards.RyanW84.Enums;

namespace Flashcards.RyanW84;

internal class UserInterface
{
    private static int stackId = 0;

    //Menu Methods
    private static string GetEnumDisplayName(Enum enumValue) //Enums weren't showing their display name, this fixes it
    {
        var displayAttribute =
            enumValue
                .GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

        if (displayAttribute == null)
        {
            Console.WriteLine("No Enum display names found");
        }

        return displayAttribute != null ? displayAttribute.Name : enumValue.ToString();
    }

    internal static void MainMenu()
    {
        Console.Clear();

        var isMenuRunning = true;

        var userInterface = new UserInterface();

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues(typeof(MainMenuChoices)).Cast<MainMenuChoices>())
                    .UseConverter(choice => GetEnumDisplayName(choice))
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
                    break;
                case MainMenuChoices.StudyHistory:
                    ViewStudyHistory();
                    break;
                case MainMenuChoices.Reports:
                    userInterface.DisplayMonthlySessionCount();
                    break;
                case MainMenuChoices.Quit:
                    System.Console.WriteLine("Goodbye");
                    isMenuRunning = false;
                    break;
            }
        }
    }

    internal static void StacksMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<StackChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues(typeof(StackChoices)).Cast<StackChoices>())
                    .UseConverter(choice => GetEnumDisplayName(choice))
            );

            switch (usersChoice)
            {
                case StackChoices.ViewStacks:
                    var dataAccess = new DataAccess();
                    ViewStacks(dataAccess.GetAllStacks());
                    break;
                case StackChoices.AddStack:
                    AddStack();
                    break;
                case StackChoices.DeleteStack:
                    DeleteStack();
                    break;
                case StackChoices.UpdateStack:
                    UpdateStack();
                    break;
                case StackChoices.ReturnToMainMenu:
                    isMenuRunning = false;
                    break;
            }
        }
    }

    internal static void FlashcardsMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<FlashcardChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues(typeof(FlashcardChoices)).Cast<FlashcardChoices>())
                    .UseConverter(choice => GetEnumDisplayName(choice))
            );

            switch (usersChoice)
            {
                case FlashcardChoices.ViewFlashcards:
                    ChooseStack("Choose the stack");
                    ViewFlashcards(stackId);
                    break;
                case FlashcardChoices.AddFlashcard:
                    AddFlashcard();
                    break;
                case FlashcardChoices.DeleteFlashcard:
                    DeleteFlashcard();
                    break;
                case FlashcardChoices.UpdateFlashcard:
                    UpdateFlashcard();
                    break;
                case FlashcardChoices.ReturnToMainMenu:
                    isMenuRunning = false;
                    break;
            }
        }
    }

    //Stacks Methods
    private static int ChooseStack(string message)
    {
        var dataAccess = new DataAccess();
        var stacks = dataAccess.GetAllStacks();

        var stacksArray = stacks.Select(x => x.Name).ToArray();
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Choose Stack").AddChoices(stacksArray)
        );

        stackId = stacks.Single(x => x.Name == option).Id;

        return stackId;
    }

    private static void ViewStacks(IEnumerable<Stack> stacks)
    {
        var table = new Table();
        table.AddColumn("#");
        table.AddColumn("Name");

        int index = 0;

        foreach (var stack in stacks)
        {
            index++;
            table.AddRow(index.ToString(), stack.Name);
        }
        AnsiConsole.Write(table);
        Console.WriteLine("Press any key to return to Main Menu");
        Console.ReadKey();
        Console.Clear();
    }

    private static void AddStack()
    {
        Stack stack = new();

        stack.Name = AnsiConsole.Ask<string>("Insert Stack's Name.");

        while (string.IsNullOrEmpty(stack.Name))
        {
            stack.Name = AnsiConsole.Ask<string>("Stack's name can't be empty. Try again.");
        }

        var dataAccess = new DataAccess();
        dataAccess.InsertStack(stack);
    }

    private static void DeleteStack()
    {
        var id = ChooseStack("Choose stack to delete: ");

        if (!AnsiConsole.Confirm("Are you sure?"))
            return;

        var dataAccess = new DataAccess();
        dataAccess.DeleteStack(id);
    }

    private static void UpdateStack()
    {
        var stack = new Stack();

        stack.Id = ChooseStack("Choose stack to update");
        stack.Name = AnsiConsole.Ask<string>("Insert Stack's Name.");

        var dataAccess = new DataAccess();
        dataAccess.UpdateStack(stack);
    }

    //Flashcards Methods
    private static int ChooseFlashcard(string message, int stackId)
    {
        var dataAccess = new DataAccess();
        IEnumerable<Flashcard> flashcards =
            (IEnumerable<Flashcard>)dataAccess.GetFlashcards(stackId);

        var flashcardsArray = flashcards.Select(x => x.Question).ToArray();
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title(message).AddChoices(flashcardsArray)
        );

        var flashcardId = flashcards.Single(x => x.Question == option).Id;

        return flashcardId;
    }

    private static void ViewFlashcards(int stackId)
    {
        var dataAccess = new DataAccess();
        IEnumerable<Flashcard> flashcards =
            (IEnumerable<Flashcard>)dataAccess.GetFlashcards(stackId);

        var table = new Table();
        table.AddColumn("#");
        table.AddColumn("Question");
        table.AddColumn("Answer");
        // I have intentionally removed stackID as users should not be shown background DB entries

        int index = 0;

        foreach (var flashcard in flashcards)
        {
            index++;
            table.AddRow(
                index.ToString(),
                flashcard.Question,
                flashcard.Answer
            // StackId removed
            );
        }
        AnsiConsole.Write(table);

        Console.WriteLine("Press any key to return to Main Menu");
        Console.ReadKey();
        Console.Clear();
    }

    private static void AddFlashcard()
    {
        Flashcard flashcard = new();

        flashcard.StackId = ChooseStack("Choose a stack for the new flashcard: ");
        flashcard.Question = GetQuestion();
        flashcard.Answer = GetAnswer();

        var dataAccess = new DataAccess();
        dataAccess.InsertFlashcard(flashcard);
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
        var question = AnsiConsole.Ask<string>("Insert Question.");

        while (string.IsNullOrEmpty(question))
        {
            question = AnsiConsole.Ask<string>("Question can't be empty. Try again.");
        }

        return question;
    }

    private static string GetAnswer()
    {
        var answer = AnsiConsole.Ask<string>("Insert answer.");

        while (string.IsNullOrEmpty(answer))
        {
            answer = AnsiConsole.Ask<string>("Answer can't be empty. Try again.");
        }

        return answer;
    }

    //Study Session Methods
    private static void StudySession()
    {
        var id = ChooseStack("Choose Stack to study");

        var dataAccess = new DataAccess();
        var flashcards = dataAccess.GetFlashcards(id);

        var studySession = new StudySession();
        studySession.Questions = flashcards.Count();
        studySession.StackId = id;
        studySession.Date = DateTime.Now;
        var correctAnswers = 0;

        Console.WriteLine("Flashcards: Study Session\n");

        foreach (var flashcard in flashcards)
        {
            var answer = AnsiConsole.Ask<string>($"Question: {flashcard.Question}\nAnswer: ");

            while (string.IsNullOrEmpty(answer))
            {
                answer = AnsiConsole.Ask<string>($"Answer can't be empty. {flashcard.Question}: ");
            }

            if (string.Equals(answer.Trim(), flashcard.Answer, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                Console.WriteLine("Correct\n");
            }
            else
            {
                Console.WriteLine($"Incorrect, the answer is: {flashcard.Answer}\n");
            }
        }
        Console.WriteLine($"You've got {correctAnswers} out of {flashcards.Count()}");

        studySession.CorrectAnswers = correctAnswers;
        studySession.Time = DateTime.Now - studySession.Date;

        dataAccess.InsertStudySession(studySession);

        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        Console.Clear();
    }

    private static void ViewStudyHistory()
    {
        Console.Clear();
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
            table.AddRow(
                session.Date.ToShortDateString(),
                session.StackName,
                $"{session.CorrectAnswers} out of {session.Questions}",
                $"{session.Percentage}%",
                session.Time.ToString()
            );
        }

        AnsiConsole.Write(table);
        Console.WriteLine("Press any key to return to Main Menu");
        Console.ReadKey();
        Console.Clear();
    }

    //Report Methods
    internal static int GetYear()
    {
        var year = AnsiConsole.Ask<int>("Please enter the Year to generate the report (YYYY)");

        while (year <= 2000 && year >= 3025)
        {
            year = AnsiConsole.Ask<int>("Invalid date, try again");
        }
        return year;
    }

    internal void DisplayMonthlySessionCount()
    {
        int year = GetYear();
        var dataAccess = new DataAccess();
        List<Models.DTO.MonthlySessionCountDTO> reportData = dataAccess.GetReportData(year);

        if (reportData.Count == 0)
        {
            AnsiConsole.MarkupLine($"[red]No study sessions found for the year {year}.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("Month");
        table.AddColumn("Session Count");

        foreach (var session in reportData)
        {
            table.AddRow(
                new Text(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(session.Month)),
                new Text(session.SessionCount.ToString())
            );
        }

        AnsiConsole.Write(table);
        Console.WriteLine("Press any key to return to Main Menu");
        Console.ReadKey();
        Console.Clear();
    }
}

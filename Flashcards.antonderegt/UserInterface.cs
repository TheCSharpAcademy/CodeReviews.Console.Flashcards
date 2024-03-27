using Flashcards.Models;
using Spectre.Console;
using System.ComponentModel;
using System.Reflection;

namespace Flashcards;

public static class UserInterface
{
    public static void ShowMainMenu()
    {
        AnsiConsole.Clear();
        bool keepRunning = true;

        while (keepRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues<MainMenu>().Select(GetDescription).ToList())
                );

            MainMenu option = GetEnumFromDescription<MainMenu>(usersChoice);

            switch (option)
            {
                case MainMenu.ManageFlashcards:
                    ShowFlashcardMenu();
                    break;
                case MainMenu.ManageStacks:
                    ShowStackMenu();
                    break;
                case MainMenu.Study:
                    StartStudy();
                    break;
                case MainMenu.ShowStudyReport:
                    ShowStudyReport();
                    break;
                case MainMenu.Quit:
                    Environment.Exit(0);
                    return;
                default:
                    break;
            }
        }
    }

    public static void ShowFlashcardMenu()
    {
        AnsiConsole.Clear();
        bool keepRunning = true;

        while (keepRunning)

        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues<FlashcardMenu>().Select(GetDescription).ToList())
                );

            FlashcardMenu option = GetEnumFromDescription<FlashcardMenu>(usersChoice);

            switch (option)
            {
                case FlashcardMenu.AddFlashcard:
                    AddFlashcard();
                    break;
                case FlashcardMenu.RemoveFlashcard:
                    RemoveFlashcard();
                    break;
                case FlashcardMenu.ShowFlashcard:
                    ShowFlashcards();
                    AnsiConsole.Markup("Press enter to return to menu...");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    break;
                case FlashcardMenu.Quit:
                    keepRunning = false;
                    return;
                default:
                    break;
            }
        }
    }

    public static void AddFlashcard()
    {
        Stack? stack = SelectStack();

        if (stack == null)
        {
            AnsiConsole.Markup("[red]Error finding stack, try again...[/]");
            Console.ReadLine();
            AnsiConsole.Clear();
            AddFlashcard();
            return;
        }

        Flashcard flashcard = CreateFlashcard(stack.Id);
        DataAccess dataAccess = new();
        bool success = dataAccess.AddFlashcard(flashcard);

        if (success)
        {
            AnsiConsole.Markup($"\n[green]Added flascard: {flashcard.Question}.[/] Press enter to return...");
        }
        else
        {
            AnsiConsole.Markup($"\n[red]Failed to add flashcard: {flashcard.Question}.[/] Press enter to return...");
        }

        AnsiConsole.Clear();
    }

    public static Stack? SelectStack()
    {
        DataAccess dataAccess = new();
        List<Stack> stacks = dataAccess.GetStacks().ToList();
        ShowStacks(stacks);

        int id = AnsiConsole.Ask<int>("Select [green]stack[/]: ") - 1;

        if (id < 0 || id >= stacks.Count)
        {
            return null;
        }

        return stacks[id];
    }

    public static void RemoveFlashcard()
    {
        DataAccess dataAccess = new();
        List<DTOs.Flashcard> flashcards = dataAccess.GetFlashcards().ToList();
        ShowFlashcards(flashcards);
        int id = AnsiConsole.Ask<int>("Id to remove: ") - 1;

        if (id < 0 || id >= flashcards.Count)
        {
            AnsiConsole.Markup($"\n[red]Invalid id.[/] Press enter to return...");
            Console.ReadLine();
            AnsiConsole.Clear();
            return;
        }

        DTOs.Flashcard flashcard = flashcards[id];
        bool success = dataAccess.RemoveFlashcard(flashcard.Id);

        if (success)
        {
            AnsiConsole.Markup($"\n[green]Removed flashcard: {flashcard.Question}.[/] Press enter to return...");
        }
        else
        {
            AnsiConsole.Markup($"\n[red]Failed to remove flashcard: {flashcard.Question}.[/] Press enter to return...");
        }

        AnsiConsole.Clear();
    }

    public static void ShowFlashcards(IEnumerable<DTOs.Flashcard>? flashcards = null)
    {
        if (flashcards == null)
        {
            DataAccess dataAccess = new();
            flashcards = dataAccess.GetFlashcards();
        }

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Question");
        table.AddColumn("Answer");

        int index = 1;
        foreach (DTOs.Flashcard flashcard in flashcards)
        {
            table.AddRow(index++.ToString(), flashcard.Question ?? "", flashcard.Answer ?? "");
        }

        AnsiConsole.Write(table);
    }

    public static Flashcard CreateFlashcard(int stackId)
    {
        string question = AnsiConsole.Ask<string>("Question: ");
        string answer = AnsiConsole.Ask<string>("Answer: ");

        return new Flashcard() { Question = question, Answer = answer, StackId = stackId };
    }

    public static void ShowStackMenu()
    {
        AnsiConsole.Clear();
        bool keepRunning = true;

        while (keepRunning)

        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues<StackMenu>().Select(GetDescription).ToList())
                );

            StackMenu option = GetEnumFromDescription<StackMenu>(usersChoice);

            switch (option)
            {
                case StackMenu.AddStack:
                    AddStack();
                    break;
                case StackMenu.RemoveStack:
                    RemoveStack();
                    break;
                case StackMenu.ShowStack:
                    ShowStacks();
                    AnsiConsole.Markup("Press enter to return to menu...");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    break;
                case StackMenu.Quit:
                    keepRunning = false;
                    return;
                default:
                    break;
            }
        }
    }

    public static void ShowStacks(IEnumerable<Stack>? stacks = null)
    {
        if (stacks == null)
        {
            DataAccess dataAccess = new();
            stacks = dataAccess.GetStacks();
        }

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");

        int index = 1;
        foreach (Stack stack in stacks)
        {
            table.AddRow(index++.ToString(), stack.Name ?? "");
        }

        AnsiConsole.Write(table);
    }

    public static void AddStack()
    {
        string name = AnsiConsole.Ask<string>("Name: ");
        Stack stack = new() { Name = name };
        DataAccess dataAccess = new();
        bool success = dataAccess.AddStack(stack);

        if (success)
        {
            AnsiConsole.Markup($"\n[green]Added stack: {name}.[/] Press enter to return...");
        }
        else
        {
            AnsiConsole.Markup($"\n[red]Failed to add stack: {name}.[/] Press enter to return...");
        }

        Console.ReadLine();

        AnsiConsole.Clear();
    }

    public static void RemoveStack()
    {
        DataAccess dataAccess = new();
        List<Stack> stacks = dataAccess.GetStacks().ToList();
        ShowStacks(stacks);
        int id = AnsiConsole.Ask<int>("Id to remove: ") - 1;

        if (id < 0 || id >= stacks.Count)
        {
            AnsiConsole.Markup($"\n[red]Invalid id.[/] Press enter to return...");
            Console.ReadLine();
            AnsiConsole.Clear();
            return;
        }

        Stack stack = stacks[id];
        bool success = dataAccess.RemoveStack(stack.Id);

        if (success)
        {
            AnsiConsole.Markup($"\n[green]Removed stack: {stack.Name}.[/] Press enter to return...");
        }
        else
        {
            AnsiConsole.Markup($"\n[red]Failed to remove stack: {stack.Name}.[/] Press enter to return...");
        }

        AnsiConsole.Clear();
    }

    public static void StartStudy()
    {
        DataAccess dataAccess = new();
        List<Stack> stacks = dataAccess.GetStacks().ToList();
        ShowStacks(stacks);
        int id = AnsiConsole.Ask<int>("Which stack do you want to study: ") - 1;

        if (id < 0 || id >= stacks.Count)
        {
            AnsiConsole.Markup($"\n[red]Invalid id.[/] Press enter to return...");
            Console.ReadLine();
            AnsiConsole.Clear();
            return;
        }

        Stack stack = stacks[id];
        IEnumerable<DTOs.Flashcard> flashcards = dataAccess.GetFlashcardsByStackId(stack.Id);
        StudyFlashcards(flashcards, stack.Id);
    }

    public static void StudyFlashcards(IEnumerable<DTOs.Flashcard> flashcards, int stackId)
    {
        AnsiConsole.Clear();

        int questionNumber = 1;
        int correctAnswers = 0;
        foreach (DTOs.Flashcard flashcard in flashcards)
        {
            AnsiConsole.MarkupLine($"[blue]Question {questionNumber++}[/]");
            string answer = AnsiConsole.Ask<string>($"{flashcard.Question}: ");
            if (string.Equals(answer.Trim(), flashcard.Answer, StringComparison.OrdinalIgnoreCase))
            {
                AnsiConsole.Markup("[green]Correct![/] Press enter to continue...");
                correctAnswers++;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]False...[/]");
                AnsiConsole.MarkupLine($"The correct answer is: [blue]{flashcard.Answer}[/]");
                AnsiConsole.MarkupLine("Press enter to continue...");
            }

            Console.ReadLine();
            AnsiConsole.Clear();
        }

        int numberOfQuestions = flashcards.Count();
        AnsiConsole.Markup($"[blue]You got {correctAnswers} right out of {numberOfQuestions}.[/] Press enter to continue...");

        int score = CalculateScore(numberOfQuestions, correctAnswers);
        StudySession studySession = new() { Score = score, Date = DateTime.Now, StackId = stackId };
        DataAccess dataAccess = new();
        dataAccess.AddStudySession(studySession);

        Console.ReadLine();
        AnsiConsole.Clear();
    }

    public static int CalculateScore(int numberOfQuestions, int correctAnswers)
    {
        return correctAnswers * 100 / numberOfQuestions;
    }

    public static void ShowStudyReport()
    {
        int year = AnsiConsole.Ask<int>("Which year: ");
        DataAccess dataAccess = new();
        IEnumerable<StudyReport> studyReports = dataAccess.GetStudyReport(year);

        var table = new Table();
        TableTitle title = new("Average Scores Per Month");
        table.Title = title;
        table.AddColumn("Stack");
        table.AddColumn("January");
        table.AddColumn("February");
        table.AddColumn("March");
        table.AddColumn("April");
        table.AddColumn("May");
        table.AddColumn("June");
        table.AddColumn("July");
        table.AddColumn("August");
        table.AddColumn("September");
        table.AddColumn("October");
        table.AddColumn("November");
        table.AddColumn("December");

        foreach (StudyReport report in studyReports)
        {
            table.AddRow(report.Name!, report.January!, report.February!, report.March!, report.April!, report.May!, report.June!, report.July!, report.August!, report.September!, report.October!, report.November!, report.December!);
        }

        AnsiConsole.Write(table);

        AnsiConsole.Markup("\nPress enter to return to menu...");
        Console.ReadLine();
        AnsiConsole.Clear();
    }

    public static string GetDescription<T>(T value) where T : Enum
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field == null)
        {
            return "Unknown menu item";
        }

        return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute ? attribute.Description : value.ToString();
    }

    public static T GetEnumFromDescription<T>(string description) where T : Enum
    {
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            if (GetDescription(value) == description)
            {
                return value;
            }
        }

        throw new ArgumentException($"No enum value with description {description} found.");
    }
}
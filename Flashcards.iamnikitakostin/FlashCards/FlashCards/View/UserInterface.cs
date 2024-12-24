using FlashCards.Controllers;
using FlashCards.Models;
using FlashCards.Services;
using Spectre.Console;
using static FlashCards.Enums;

namespace FlashCards.View;
internal class UserInterface : ConsoleController
{
    private readonly StackService _stackService;
    private readonly FlashcardService _flashcardService;
    private readonly StudySessionService _studySessionService;

    public UserInterface(StackService stackService, FlashcardService flashcardService, StudySessionService studySessionService)
    {
        _stackService = stackService;
        _flashcardService = flashcardService;
        _studySessionService = studySessionService;
    }
    internal void MainMenu()
    {
        var mainMenuOptions = EnumToDisplayNames<MainMenuOptions>();
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                .Title("What do you want to do next?")
                .AddChoices(mainMenuOptions.Keys)
                .UseConverter(option => mainMenuOptions[option]));

            switch (choice)
            {
                case MainMenuOptions.Study:
                    StudySessionMenu();
                    break;
                case MainMenuOptions.ViewStudyData:
                    ViewStudyData();
                    break;
                case MainMenuOptions.ManageFlashCards:
                    var manageFlashCardsChoice = ManageNavigationMenu();

                    switch (manageFlashCardsChoice)
                    {
                        case ManageNavMenuOptions.Add:
                            AddFlashCardMenu();
                            break;
                        case ManageNavMenuOptions.ManageAll:
                            var flashcard = _flashcardService.SelectFlashcardToManage();
                            if (flashcard == null) break;

                            ManageFlashcardMenu(flashcard);
                            break;
                        default:
                            break;
                    }
                    break;
                case MainMenuOptions.ManageStacks:
                    var manageStacksChoice = ManageNavigationMenu();

                    switch (manageStacksChoice)
                    {
                        case ManageNavMenuOptions.Add:
                            AddStackMenu();
                            break;
                        case ManageNavMenuOptions.ManageAll:
                            var stack = _stackService.SelectStackToManage();
                            if (stack == null) break;

                            ManageStackMenu(stack);
                            break;
                        default:
                            break;
                    }
                    break;
                case MainMenuOptions.About:
                    About();
                    break;
            default:
                    return;
            }
        }
    }

    internal void ViewStudyData()
    {
        Table sessionsTable = DrawStudyingSessionsTable();
        sessionsTable = _studySessionService.GetReport(sessionsTable);
        AnsiConsole.Write(sessionsTable);
        AnsiConsole.WriteLine("\nPress any button to continue...");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    internal static string ManageAllItemsMenu(List<string> choices)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select an item to manage:")
            .AddChoices(choices));
        return choice;
    }

    internal void ManageStackMenu(Models.Stack stack)
    {
        var manageStackMenuOptions = EnumToDisplayNames<ManageStackMenuOptions>();
        bool isMenuOpened = true;

        while (isMenuOpened)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<ManageStackMenuOptions>()
                .Title("What do you want to do next?")
                .AddChoices(manageStackMenuOptions.Keys)
                .UseConverter(option => manageStackMenuOptions[option]));

            switch (choice)
            {
                case ManageStackMenuOptions.ViewFullInfo:
                    StackService.ShowFull(stack);
                    break;
                case ManageStackMenuOptions.Edit:
                    var editStackMenuOptions = EnumToDisplayNames<ManageStackEditOptions>();
                    var editChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<ManageStackEditOptions>()
                        .Title("What do you want to edit?")
                        .AddChoices(editStackMenuOptions.Keys)
                        .UseConverter(option => editStackMenuOptions[option]));
                    var update = AnsiConsole.Prompt(
                        new TextPrompt<string>($"Enter new name for your stack:"));
                    stack.Name = update;

                    _stackService.Edit(editChoice, stack);
                    break;
                case ManageStackMenuOptions.Delete:
                    var confirmationMenuOptions = EnumToDisplayNames<ConfirmationMenuOptions>();
                    var confirmationChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<ConfirmationMenuOptions>()
                        .Title("Are you sure you would like to delete it? The action is irreversible.")
                        .AddChoices(confirmationMenuOptions.Keys)
                        .UseConverter(option => confirmationMenuOptions[option]));
                        
                    if (confirmationChoice == ConfirmationMenuOptions.No) break;

                    _stackService.Delete(stack.Id);
                    isMenuOpened = false;
                    break;
                default:
                    return;
            }
        }
    }

    internal static ManageNavMenuOptions ManageNavigationMenu()
    {
        var manageNavMenuOptions = EnumToDisplayNames<ManageNavMenuOptions>();
        AnsiConsole.Clear();
        var manageNavMenuChoice = AnsiConsole.Prompt(
            new SelectionPrompt<ManageNavMenuOptions>()
            .Title("What do you want to do next?")
            .AddChoices(manageNavMenuOptions.Keys)
            .UseConverter(option => manageNavMenuOptions[option]));

        return manageNavMenuChoice;
    }

    internal static void ShowFlashcard(Flashcard flashcard)
    {
        var table = DrawFlashcardReviewTable();
        table.AddRow(flashcard.FrontText);
        AnsiConsole.Write(table);

        AnsiConsole.Markup("Press any key to show the back of the card.");
        AnsiConsole.Console.Input.ReadKey(false);

        AnsiConsole.Clear();
        table.RemoveRow(0);
        table.AddColumn(new TableColumn("Back").Centered());
        table.AddRow(flashcard.FrontText, flashcard.BackText);
        AnsiConsole.Write(table);
    }

    internal static bool StudySessionManageMenu()
    {
        AnsiConsole.Clear();
        var studyMenuOptions = EnumToDisplayNames<StudyMenuOptions>();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<StudyMenuOptions>()
            .Title("Continue?")
            .AddChoices(studyMenuOptions.Keys)
            .UseConverter(option => studyMenuOptions[option]));

        switch (choice)
        {
            case StudyMenuOptions.NextCard:
                return true;
            default:
                return false;
        }
    }

    internal void StudySessionMenu()
    {
        var stacks = _stackService.GetAll();
        if (stacks == null || stacks.Count < 1)
        {
            ErrorMessage("Dear user, you do not have a stack/any flashcards yet.\nPlease, create them first.");
            AddStackMenu();
            return;
        }

        var stackChoices = stacks.Select(s => s.Name).ToList();
        var selectedChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select a stack to study:")
            .AddChoices(stackChoices));

        var stack = stacks.FirstOrDefault(s => s.Name == selectedChoice);

        stack.Flashcards = _flashcardService.GetFlashcardsByStack(stack.Id);

        if (stack == null || stack.Flashcards.Count < 1)
        {
            ErrorMessage("Dear user, you do not have any flashcards in the stack yet.\nPlease, create one first.");
            return;
        }

        _studySessionService.StartSession(stack);
    }

    internal void ManageFlashcardMenu(Flashcard flashcard)
    {
        var manageFlashcardMenuOptions = EnumToDisplayNames<ManageFlashcardMenuOptions>();
        bool isMenuOpened = true;

        while (isMenuOpened)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<ManageFlashcardMenuOptions>()
                .Title("What do you want to do next?")
                .AddChoices(manageFlashcardMenuOptions.Keys)
                .UseConverter(option => manageFlashcardMenuOptions[option]));

            switch (choice)
            {
                case ManageFlashcardMenuOptions.ViewFullInfo:
                    flashcard.Stack = _stackService.GetById(flashcard.StackId);
                    FlashcardService.ShowFull(flashcard);
                    break;
                case ManageFlashcardMenuOptions.Edit:
                    var editFlashcardMenuOptions = EnumToDisplayNames<ManageFlashcardEditOptions>();
                    var editChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<ManageFlashcardEditOptions>()
                        .Title("What do you want to edit?")
                        .AddChoices(editFlashcardMenuOptions.Keys)
                        .UseConverter(option => editFlashcardMenuOptions[option]));

                    var update = AnsiConsole.Prompt(
                        new TextPrompt<string>($"Enter a new value for your flashcard:"));
                    if (editChoice == ManageFlashcardEditOptions.Front)
                    {
                        flashcard.FrontText = update;
                    }
                    else {
                        flashcard.BackText = update;
                    }

                    _flashcardService.Edit(editChoice, flashcard);
                    break;
                case ManageFlashcardMenuOptions.Delete:
                    var confirmationMenuOptions = EnumToDisplayNames<ConfirmationMenuOptions>();
                    var confirmationChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<ConfirmationMenuOptions>()
                        .Title("Are you sure you would like to delete it? The action is irreversible.")
                        .AddChoices(confirmationMenuOptions.Keys)
                        .UseConverter(option => confirmationMenuOptions[option]));

                    if (confirmationChoice == ConfirmationMenuOptions.No) break;
                    _flashcardService.Delete(flashcard.Id);
                    isMenuOpened = false;
                    break;
                default:
                    isMenuOpened = false;
                    break;
            }
        }
    }

    internal static void StandardMenu()
    {
        var standardMenuOptions = EnumToDisplayNames<StandardMenuOptions>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<StandardMenuOptions>()
                .Title("What do you want to do next?")
                .AddChoices(standardMenuOptions.Keys)
                .UseConverter(option => standardMenuOptions[option]));

            switch (choice)
            {
                case StandardMenuOptions.Back:
                    return;
                default:
                    break;
            }
        }
    }

    internal void AddFlashCardMenu() {
        List<Models.Stack> currentStacks = _stackService.GetAll();

        var stackChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What do you want to do next?")
                .AddChoices(currentStacks.Select(s => s.Name).ToArray()));

        Flashcard flashcard = new();
        flashcard.FrontText = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter front text of your new card:"));
        flashcard.BackText = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter back text of your new card:"));
        flashcard.StackId = currentStacks.Where(s => s.Name == stackChoice).Select(s => s.Id).FirstOrDefault();
        flashcard.CreationTime = DateTime.Now;

        _flashcardService.Add(flashcard);
    }
    internal void AddStackMenu() {
        Models.Stack stack = new();
        stack.Name = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter a name for your new stack:"));

        _stackService.Add(stack);
    }

    internal static BreakPeriodInSecondsOptions ChooseNextShowTime(Flashcard flashcard)
    {
        List<BreakPeriodInSecondsOptions> newPeriods = FlashcardService.GetNewShowtimes(flashcard);
            
        var periodChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select when the card will be repeated next time")
                .AddChoices(newPeriods.Select(s => s.ToString()).ToArray()));

        if (Enum.TryParse(periodChoice, out BreakPeriodInSecondsOptions selectedPeriod))
        {
            return selectedPeriod;
        }
        return BreakPeriodInSecondsOptions.Minute;
    }

    internal static void About()
    {
        DisplayMessage("Created by Nikita Kostin.\n");
    }

    static Dictionary<TEnum, string> EnumToDisplayNames<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .ToDictionary(
                value => value,
                value => SplitCamelCase(value.ToString())
            );
    }

    internal static string SplitCamelCase(string input)
    {
        return string.Join(" ", System.Text.RegularExpressions.Regex
            .Split(input, @"(?<!^)(?=[A-Z])"));
    }
}

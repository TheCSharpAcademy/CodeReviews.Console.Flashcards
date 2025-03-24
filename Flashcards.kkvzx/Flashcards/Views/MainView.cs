using Flashcards.Models.Dtos;
using Flashcards.shared;
using Flashcards.Views.utils;
using Spectre.Console;

namespace Flashcards.Views;

public class MainView : ConsoleApplication
{
    public readonly ManageStacksView ManageStacks = new();
    public readonly ManageFlashcardsView ManageFlashcards = new();
    public readonly UserInput Input = new();
    
    public MenuOption ShowMenu()
    {
        AnsiConsole.Write(new FigletText("Flashcards!").Centered().Color(Color.DarkCyan));
        AnsiConsole.MarkupLine("");

        var menuDict = new Dictionary<string, MenuOption>
        {
            { "Exit", MenuOption.Exit },
            { "Seed", MenuOption.Seed },
            { "Delete Data", MenuOption.DeleteData },
            { "Manage Stacks", MenuOption.ManageStacks },
            { "Manage Flashcards", MenuOption.ManageFlashcards },
            { "Learn", MenuOption.Learn },
            { "Show sessions", MenuOption.ShowSessions },
        };
        
        var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("[darkcyan]Select an option: [/]")
            .AddChoices(menuDict.Keys));

        return menuDict[selection];
    }

    public StackDto SelectStack(List<StackDto> stacks)
    {
        AnsiConsole.Clear();
        var input = new UserInput();

        DtosDisplay.DisplayStacks(stacks);
        var selectedStack = input.GetExistingEntity(stacks,
            "Enter Idx of selected flashcard stack: ");

        return selectedStack;
    }

    public void DisplaySessions(List<SessionDto> sessions, List<StackDto> stacks) =>
        DtosDisplay.DisplaySessions(sessions, stacks);
}
using Flashcards.FunRunRushFlush.App.Interfaces;
using Flashcards.FunRunRushFlush.Controller.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Flashcards.FunRunRushFlush.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Reflection.PortableExecutable;

namespace Flashcards.FunRunRushFlush.App.Screens;

public class StackScreen : IStackScreen
{
    private readonly ILogger<StackScreen> _log;
    private readonly ICrudController _crud;
    private readonly IUserInputValidationService _userInpValidation;
    private readonly IFlashcardScreen _flashcardScreen;

    public StackScreen(ILogger<StackScreen> log, ICrudController crud, IUserInputValidationService userInpValidation, IFlashcardScreen flashcardScreen)
    {
        _log = log;
        _crud = crud;
        _userInpValidation = userInpValidation;
        _flashcardScreen = flashcardScreen;
    }

    public async Task RunStackView()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AppHelperFunctions.AppHeader("Flashcards", true);

            var fStacks = _crud.ShowAllStacks();
            fStacks.Add(new Stack(0, "Create new Stack"));

            var table = new Table().Centered().Expand();
            table.Border = TableBorder.Rounded;

            table.AddColumn("Topic").Centered();


            int selectedIndex = 0;
            bool exit = false;
            Stack selectedStack = null;

            await AnsiConsole.Live(table)
                .Overflow(VerticalOverflow.Ellipsis)
                .StartAsync(async ctx =>
                {
                    while (!exit)
                    {
                        table.Rows.Clear();
                        table.Title("[[ [green]Flashcards Stack Overview [/]]]");
                        table.Caption("[[[blue] [[Up/Down]] Navigation, [[Enter]] Select, [[ESC]] Escape[/]]]");

                        for (int i = 0; i < fStacks.Count; i++)
                        {
                            var stack = fStacks[i];
                            if (fStacks.Count - 1 == i)
                            {
                                if (i == selectedIndex)
                                {
                                    table.AddRow("[blue] > Create new Stack of Flashcards <[/]");
                                }
                                else
                                {
                                    table.AddRow("[dim]> Create new Stack of Flashcards <[/]");
                                }

                            }
                            else if (i == selectedIndex)
                            {
                                table.AddRow($"[blue]>{stack.Name}[/]");
                            }
                            else
                            {
                                table.AddRow(stack.Name);
                            }
                        }


                        ctx.Refresh();

                        var key = Console.ReadKey(true).Key;

                        switch (key)
                        {
                            case ConsoleKey.Escape:
                                exit = true;
                                break;

                            case ConsoleKey.UpArrow:
                                selectedIndex--;
                                if (selectedIndex < 0)
                                    selectedIndex = fStacks.Count - 1;
                                break;

                            case ConsoleKey.DownArrow:
                                selectedIndex++;
                                if (selectedIndex >= fStacks.Count)
                                    selectedIndex = 0;
                                break;

                            case ConsoleKey.Enter:

                                selectedStack = fStacks[selectedIndex];
                                exit = true;
                                break;
                        }
                    }
                });


            if (selectedStack != null)
            {
                if (selectedStack.Id == 0)
                {
                    var codSes = _userInpValidation.ValidateUserStackInput();
                    _crud.CreateStack(codSes);
                }
                else
                {
                    var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .PageSize(10)
                            .AddChoices(new[] {
                             "Select", "Update", "Delete", "Back"
                            }));

                    if (choice == "Select")
                    {
                        await _flashcardScreen.RunFlashcardsView(selectedStack);
                    }

                    if (choice == "Update")
                    {
                        var codSes = _userInpValidation.ValidateUserStackInput(selectedStack);

                        _crud.UpdateStack(codSes);
                    }
                    if (choice == "Delete")
                    {
                        var confirmation = AnsiConsole.Prompt(
                            new TextPrompt<bool>($"[yellow]Are you sure you want to [red]Delete[/] the Stack?: [/]")
                                .AddChoice(true)
                                .AddChoice(false)
                                .DefaultValue(false)
                                .WithConverter(choice => choice ? "y" : "n"));

                        if (confirmation)
                        {
                            _crud.DeleteStack(selectedStack);
                        }
                    }
                }
            }
            else
            {


                if (AppHelperFunctions.ReturnMenu())
                {
                    break;
                }
            }
        }
    }

    public async Task RunStackSelection()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AppHelperFunctions.AppHeader("Flashcards", true);

            var fStacks = _crud.ShowAllStacks();

            if (fStacks == null || fStacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Create a Stack with Flashcards first [/]");
                Console.ReadKey(true);
                return;
            }
            var table = new Table().Centered().Expand();
            table.Border = TableBorder.Rounded;

            table.AddColumn("Topic").Centered();


            int selectedIndex = 0;
            bool exit = false;
            Stack selectedStack = null;

            await AnsiConsole.Live(table)
                .Overflow(VerticalOverflow.Ellipsis)
                .StartAsync(async ctx =>
                {
                    while (!exit)
                    {
                        table.Rows.Clear();
                        table.Title("[[ [green]Flashcards Stack Overview [/]]]");
                        table.Caption("[[[blue] [[Up/Down]] Navigation, [[Enter]] Select, [[ESC]] Escape[/]]]");

                        for (int i = 0; i < fStacks.Count; i++)
                        {
                            var stack = fStacks[i];

                            if (i == selectedIndex)
                            {
                                table.AddRow($"[blue]>{stack.Name}[/]");
                            }
                            else
                            {
                                table.AddRow(stack.Name);
                            }
                        }


                        ctx.Refresh();

                        var key = Console.ReadKey(true).Key;

                        switch (key)
                        {
                            case ConsoleKey.Escape:
                                exit = true;
                                break;

                            case ConsoleKey.UpArrow:
                                selectedIndex--;
                                if (selectedIndex < 0)
                                    selectedIndex = fStacks.Count - 1;
                                break;

                            case ConsoleKey.DownArrow:
                                selectedIndex++;
                                if (selectedIndex >= fStacks.Count)
                                    selectedIndex = 0;
                                break;

                            case ConsoleKey.Enter:

                                selectedStack = fStacks[selectedIndex];
                                exit = true;
                                break;
                        }
                    }
                });


            if (selectedStack != null)
            {

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(10)
                        .AddChoices(new[] {
                             "All", "Only Unsolved", "Only Solved", "Back"
                        }));

                if (choice == "All")
                {
                    await _flashcardScreen.RunFlashcardSession(selectedStack, SessionMode.All);
                }
                if (choice == "Only Unsolved")
                {
                    await _flashcardScreen.RunFlashcardSession(selectedStack, SessionMode.Only_Unsolved);
                }
                if (choice == "Only Solved")
                {
                    await _flashcardScreen.RunFlashcardSession(selectedStack, SessionMode.Only_Solved);
                }

            }
            else
            {

                if (AppHelperFunctions.ReturnMenu())
                {
                    break;
                }
            }
        }
    }
}

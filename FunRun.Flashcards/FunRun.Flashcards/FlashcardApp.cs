using FunRun.Flashcards.Controller.Interfaces;
using FunRun.Flashcards.Data.Model;
using FunRun.Flashcards.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace FunRun.Flashcards;

public class FlashcardApp
{
    private readonly ILogger<FlashcardApp> _log;
    private readonly ICrudController _crud;
    private readonly IUserInputValidationService _userInpValidation;

    public FlashcardApp(ILogger<FlashcardApp> log, ICrudController crud, IUserInputValidationService userInp)
    {
        _log = log;
        _crud = crud;
        _userInpValidation = userInp;
    }

    public async Task RunApp()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Flashcards").Centered().Color(Color.Blue));

            AnsiConsole.Write(
             new Markup("[blue]Inspired by the [link=https://thecsharpacademy.com/project/14/flashcards]C#Academy[/][/]")
             .Centered());

            var fStacks = _crud.ShowAllStacks();
            fStacks.Add(new Stack(-1,"Create new Stack"));

            var table = new Table().Centered();
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
                                    table.AddRow("[blue]> Create new Session <[/]");
                                }
                                else
                                {
                                    table.AddRow("[dim]> Create new Session <[/]");
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
                    var codSes = _userInpValidation.ValidateUserSessionInput();
                    _crud.CreateStack(codSes);
                }
                else
                {
                    var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .PageSize(10)
                            .AddChoices(new[] {
                                "Update", "Delete", "Back"
                            }));

                    if (choice == "Update")
                    {
                        var codSes = _userInpValidation.ValidateUserSessionInput(selectedStack);

                        _crud.UpdateStack(codSes);
                    }
                    if (choice == "Delete")
                    {
                        var confirmation = AnsiConsole.Prompt(
                            new TextPrompt<bool>($"[yellow]Are you sure you want to [red]Delete[/] the Session: [/]")
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
                var confirmation = AnsiConsole.Prompt(
        new TextPrompt<bool>($"[yellow]Are you sure you want to Close the App[/]")
            .AddChoice(true)
            .AddChoice(false)
            .DefaultValue(false)
            .WithConverter(choice => choice ? "y" : "n"));

                if (confirmation)
                {
                    break;
                }
            }
        }
    }

}

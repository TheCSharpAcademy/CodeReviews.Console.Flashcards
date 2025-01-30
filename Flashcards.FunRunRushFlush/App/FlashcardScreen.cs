using Flashcards.FunRunRushFlush.App.Interfaces;
using Flashcards.FunRunRushFlush.Controller.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Flashcards.FunRunRushFlush.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Flashcards.FunRunRushFlush.App;

public class FlashcardScreen : IFlashcardScreen
{
    private readonly ILogger<FlashcardScreen> _log;
    private readonly ICrudController _crud;
    private readonly IUserInputValidationService _userInpValidation;

    public FlashcardScreen(ILogger<FlashcardScreen> log, ICrudController crud, IUserInputValidationService userInp)
    {
        _log = log;
        _crud = crud;
        _userInpValidation = userInp;
    }

    public async Task RunFlashcardsView(Stack stack)
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText(stack.Name).Centered().Color(Color.Blue));


            var objects = _crud.ShowAllFlashcardsOfSelectedStack(stack);
            objects.Add(new Flashcard(0, 0, "Create new Flashcard", "Create new Flashcard", false));

            var table = new Table().Centered().Expand();
            table.Border = TableBorder.Rounded;

            table.AddColumn("Front").Centered();
            table.AddColumn("Back").Centered();
            table.AddColumn("Solved").Centered();



            int selectedIndex = 0;
            bool exit = false;
            Flashcard selectedObject = null;

            await AnsiConsole.Live(table)
                .Overflow(VerticalOverflow.Ellipsis)
                .StartAsync(async ctx =>
                {
                    while (!exit)
                    {
                        table.Rows.Clear();
                        table.Title("[[ [green]Flashcards Stack Overview [/]]]");
                        table.Caption("[[[blue] [[Up/Down]] Navigation, [[Enter]] Select, [[ESC]] Escape[/]]]");

                        for (int i = 0; i < objects.Count; i++)
                        {
                            var flashcard = objects[i];
                            if (objects.Count - 1 == i)
                            {
                                if (i == selectedIndex)
                                {
                                    table.AddRow("[blue] > Create new Flashcard <[/]",
                                                 "[blue] > Create new Flashcard <[/]",
                                                 false.ToString());


                                }
                                else
                                {
                                    table.AddRow("[dim] > Create new Flashcard <[/]",
                                                 "[dim] > Create new Flashcard <[/]",
                                                 false.ToString());
                                }

                            }
                            else if (i == selectedIndex)
                            {
                                table.AddRow($"[blue]>{flashcard.Front}[/]",
                                             $"[blue]{flashcard.Back}[/]",
                                             $"[blue]{flashcard.Solved}[/]");
                            }
                            else
                            {
                                table.AddRow($"{flashcard.Front}",
                                             $"{flashcard.Back}",
                                             $"{flashcard.Solved}");
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
                                    selectedIndex = objects.Count - 1;
                                break;

                            case ConsoleKey.DownArrow:
                                selectedIndex++;
                                if (selectedIndex >= objects.Count)
                                    selectedIndex = 0;
                                break;

                            case ConsoleKey.Enter:

                                selectedObject = objects[selectedIndex];
                                exit = true;
                                break;
                        }
                    }
                });


            if (selectedObject != null)
            {
                //if (selectedObject.Id == 0)
                //{
                //    var codSes = _userInpValidation.ValidateUserSessionInput();
                //    _crud.CreateStack(codSes);
                //}
                //else
                //{
                //    var choice = AnsiConsole.Prompt(
                //        new SelectionPrompt<string>()
                //            .PageSize(10)
                //            .AddChoices(new[] {
                //                "Update", "Delete", "Back"
                //            }));

                //    if (choice == "Update")
                //    {
                //        var codSes = _userInpValidation.ValidateUserSessionInput(selectedObject);

                //        _crud.UpdateStack(codSes);
                //    }
                //    if (choice == "Delete")
                //    {
                //        var confirmation = AnsiConsole.Prompt(
                //            new TextPrompt<bool>($"[yellow]Are you sure you want to [red]Delete[/] the Session: [/]")
                //                .AddChoice(true)
                //                .AddChoice(false)
                //                .DefaultValue(false)
                //                .WithConverter(choice => choice ? "y" : "n"));

                //        if (confirmation)
                //        {
                //            _crud.DeleteStack(selectedObject);
                //        }
                //    }
                //}
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

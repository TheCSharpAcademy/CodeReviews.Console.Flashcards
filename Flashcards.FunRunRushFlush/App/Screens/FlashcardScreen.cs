using Flashcards.FunRunRushFlush.App.Interfaces;
using Flashcards.FunRunRushFlush.Controller.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Flashcards.FunRunRushFlush.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Flashcards.FunRunRushFlush.App.Screens;

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

    public async Task RunFlashcardsView(Stack selectedStack)
    {
        while (true)
        {
            AnsiConsole.Clear();

            AppHelperFunctions.AppHeader(selectedStack.Name);

            var objects = _crud.ShowAllFlashcardsOfSelectedStack(selectedStack);

            objects.Add(new Flashcard(0, 0, "Create new Flashcard", "Create new Flashcard", false));

            var table = new Table().Centered().Expand();
            table.Border = TableBorder.Rounded;

            table.AddColumn("Id");
            table.AddColumn("Front");
            table.AddColumn("Back");
            table.AddColumn("Solved");


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
                                    table.AddRow("[blue]>0[/]",
                                                 "[blue] > Create new Flashcard <[/]",
                                                 "[blue] > Create new Flashcard <[/]",
                                                 false.ToString());
                                }
                                else
                                {
                                    table.AddRow("[dim]0[/]",
                                                 "[dim] > Create new Flashcard <[/]",
                                                 "[dim] > Create new Flashcard <[/]",
                                                 false.ToString());
                                }

                            }
                            else if (i == selectedIndex)
                            {
                                table.AddRow($"[blue]>{i + 1}[/]",
                                    $"[blue]{flashcard.Front}[/]",
                                             $"[blue]{flashcard.Back}[/]",
                                             $"[blue]{flashcard.Solved}[/]");
                            }
                            else
                            {
                                table.AddRow($"{i + 1}",
                                             $"{flashcard.Front}",
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
                if (selectedObject.Id == 0)
                {
                    var newFlashcard = _userInpValidation.ValidateUserFlashcardInput(selectedStack);
                    _crud.CreateFlashcard(newFlashcard);
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
                        var flashcard = _userInpValidation.ValidateUserFlashcardInput(selectedStack, selectedObject);
                        _crud.UpdateFlashcard(flashcard);
                    }
                    if (choice == "Delete")
                    {
                        var confirmation = AnsiConsole.Prompt(
                            new TextPrompt<bool>($"[yellow]Are you sure you want to [red]Delete[/] the Flashcard: [/]")
                                .AddChoice(true)
                                .AddChoice(false)
                                .DefaultValue(false)
                                .WithConverter(choice => choice ? "y" : "n"));

                        if (confirmation)
                        {
                            _crud.DeleteFlashcard(selectedObject);
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

    public async Task RunFlashcardSession(Stack selectedStack, SessionMode sessionMode)
    {
        var objects = _crud.ShowAllFlashcardsOfSelectedStack(selectedStack);

        if (objects == null || objects.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]The Stack is empty [/]");
            Console.ReadKey(true);
            return;
        }

        if (sessionMode == SessionMode.Only_Unsolved)
        {
            objects = objects.Where(x => x.Solved == false).ToList();
            if (objects == null || objects.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]There a no Unsolved Flashcards left[/]");
                Console.ReadKey(true);
                return;
            }
        }
        if (sessionMode == SessionMode.Only_Solved)
        {
            objects = objects.Where(x => x.Solved == true).ToList();
            if (objects == null || objects.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]There a no Solved Flashcards left[/]");
                Console.ReadKey(true);
                return;
            }
        }

        var panel = new Panel("Front").Header("Question").Expand().Border(BoxBorder.Rounded);

        foreach (var question in objects)
        {
            AnsiConsole.Clear();

            AppHelperFunctions.AppHeader(selectedStack.Name);

            AnsiConsole.MarkupLine("\n");
            AnsiConsole.Write(new Panel(question.Front)
                .Header("Question")
                .HeaderAlignment(Justify.Center)
                .Expand()
                .Border(BoxBorder.Rounded));


            var answer = AnsiConsole.Prompt(
                new TextPrompt<string>("[yellow]Enter your Answer (max 250 Chars, caseinsensitive):[/]")
                    .Validate(input =>
                    {
                        if (string.IsNullOrWhiteSpace(input) || input.Length > 250)
                            return ValidationResult.Error("[red]Please enter a valid input, up to 250 characters![/]");
                        return ValidationResult.Success();
                    }));

            if (answer.Trim().ToLower() == question.Back.ToString().Trim().ToLower())
            {
                question.Solved = true;
                AnsiConsole.MarkupLine("[green]Correct![/]");
            }
            else
            {
                question.Solved = false;
                AnsiConsole.MarkupLine("[red]Incorrect. Moving on...[/]");
            }


            Console.ReadKey(true);
        }

        int solvedCount = objects.Count(x => x.Solved == true);
        string result = $"Mode: {sessionMode.ToString()} - {solvedCount}/{objects.Count}";
        StudySession studySession = new StudySession(0, selectedStack.Id, selectedStack.Name, result, DateTime.Now);

        foreach (Flashcard item in objects)
        {
            _crud.UpdateFlashcard(item);
        }

        _crud.CreateStudySession(studySession);

        AnsiConsole.MarkupLine("[green]Study Session finished![/]");
        Console.ReadKey(true);
    }
}

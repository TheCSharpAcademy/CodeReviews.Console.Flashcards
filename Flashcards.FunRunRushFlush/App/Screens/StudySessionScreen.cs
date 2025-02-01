using Flashcards.FunRunRushFlush.App.Interfaces;
using Flashcards.FunRunRushFlush.Controller.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Flashcards.FunRunRushFlush.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Flashcards.FunRunRushFlush.App.Screens;

public class StudySessionScreen : IStudySessionScreen
{
    private readonly ILogger<StudySessionScreen> _log;
    private readonly ICrudController _crud;
    private readonly IUserInputValidationService _userInpValidation;
    private readonly IFlashcardScreen _flashcardScreen;
    private readonly IStackScreen _stackScreen;

    public StudySessionScreen(ILogger<StudySessionScreen> log,
                              ICrudController crud,
                              IUserInputValidationService userInpValidation,
                              IFlashcardScreen flashcardScreen,
                              IStackScreen stackScreen)
    {
        _log = log;
        _crud = crud;
        _userInpValidation = userInpValidation;
        _flashcardScreen = flashcardScreen;
        _stackScreen = stackScreen;
    }

    public async Task RunStudySessionView()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AppHelperFunctions.AppHeader("Study Session");

            var fSessions = _crud.ShowAllStudySessions();
            fSessions.Add(new StudySession(0, 0, "New Study Session", "New Study Session", DateTime.Now));

            var table = new Table().Centered().Expand();
            table.Border = TableBorder.Rounded;

            table.AddColumn("Topic");
            table.AddColumn("Details");
            table.AddColumn("Date");

            int selectedIndex = 0;
            bool exit = false;
            StudySession selectedSession = null;

            await AnsiConsole.Live(table)
                .Overflow(VerticalOverflow.Ellipsis)
                .StartAsync(async ctx =>
                {
                    while (!exit)
                    {
                        table.Rows.Clear();
                        table.Title("[[ [green]Study Session Overview [/]]]");
                        table.Caption("[[[blue] [[Up/Down]] Navigation, [[Enter]] Select, [[ESC]] Escape[/]]]");

                        for (int i = 0; i < fSessions.Count; i++)
                        {
                            var session = fSessions[i];
                            if (fSessions.Count - 1 == i)
                            {
                                if (i == selectedIndex)
                                {
                                    table.AddRow("[blue] > Create a new Study Session <[/]", "");
                                }
                                else
                                {
                                    table.AddRow("[dim]> Create a new Study Session <[/]", "");
                                }

                            }
                            else if (i == selectedIndex)
                            {
                                table.AddRow($"[blue]>{session.StackName}[/]",
                                            $"[blue]{session.UsedFlashcards}[/]",
                                                $"[blue]{session.Date.ToShortDateString()}[/]");
                            }
                            else
                            {
                                table.AddRow(session.StackName, session.UsedFlashcards, session.Date.ToShortDateString());
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
                                    selectedIndex = fSessions.Count - 1;
                                break;

                            case ConsoleKey.DownArrow:
                                selectedIndex++;
                                if (selectedIndex >= fSessions.Count)
                                    selectedIndex = 0;
                                break;

                            case ConsoleKey.Enter:

                                selectedSession = fSessions[selectedIndex];
                                exit = true;
                                break;
                        }
                    }
                });


            if (selectedSession != null)
            {
                if (selectedSession.Id == 0)
                {
                    await _stackScreen.RunStackSelection();
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

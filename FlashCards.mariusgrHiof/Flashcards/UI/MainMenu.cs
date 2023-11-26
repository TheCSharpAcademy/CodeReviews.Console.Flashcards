using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.UI;

public class MainMenu
{
    public MainMenu(StacksController stacksController, FlashcardsController flashcardsController, StudySessionsController studySessionsController)
    {
        _stacksController = stacksController;
        _flashcardsController = flashcardsController;
        _studySessionsController = studySessionsController;
    }

    bool exitApp = false;
    private readonly StacksController _stacksController;
    private readonly FlashcardsController _flashcardsController;
    private readonly StudySessionsController _studySessionsController;

    public async Task ShowMenu()
    {
        while (!exitApp)
        {
            AnsiConsole.Clear();

            AnsiConsole.WriteLine("Main Menu");

            ShowMenuItems();

            Console.Write("Enter a number: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    exitApp = true;
                    break;
                case "2":
                    await ManageStacks();
                    break;
                case "3":
                    await ManageFlashcards();
                    break;
                case "4":
                    await Study();
                    break;
                case "5":
                    await StudySessionsData();
                    break;
                default:
                    Console.WriteLine("Invalid number.Try again");
                    break;
            }
        }

    }

    private async Task StudySessionsData()
    {
        Table table = new Table();
        table.AddColumns("Date", "Score", "Stack name");

        var studySessions = await _studySessionsController.GetAllStudySessionsAsync();

        if (studySessions.Count == 0)
        {
            Console.WriteLine("No records found.");
            Console.WriteLine("Press any key to continue");

            Console.ReadLine();
            return;
        }

        foreach (var studySession in studySessions)
        {
            table.AddRow(studySession.Date.ToString(), studySession.Score.ToString(), studySession.Stack.Name);
        }

        AnsiConsole.Write(table);

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private async Task Study()
    {
        Study study = new Study(_studySessionsController, _stacksController, _flashcardsController);

        await study.Run();

    }

    private void ShowMenuItems()
    {
        Console.WriteLine("1. Exit");
        Console.WriteLine("2. Manage Stacks");
        Console.WriteLine("3. Manage Flashcards");
        Console.WriteLine("4. Study");
        Console.WriteLine("5. View study sessions data");
    }

    private async Task ManageFlashcards()
    {
        ManageFlashcards manageFlashcards = new ManageFlashcards(_flashcardsController, _stacksController);
        await manageFlashcards.ShowMenu();

    }

    private async Task ManageStacks()
    {
        ManageStacks manageStacks = new ManageStacks(_stacksController);
        await manageStacks.ShowMenu();
    }

}
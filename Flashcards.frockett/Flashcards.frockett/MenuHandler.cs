using Spectre.Console;
using Library;

namespace Flashcards.frockett;

internal class MenuHandler
{
    private readonly DisplayService displayService;
    private readonly StackController stackController;
    private readonly CardController cardController;
    private readonly StudySessionController studySessionController;

    public MenuHandler(DisplayService displayService, StackController stackController, CardController cardController, StudySessionController studySessionController)
    {
        this.displayService = displayService;
        this.stackController = stackController;
        this.cardController = cardController;
        this.studySessionController = studySessionController;
    }
    
    public void ShowMainMenu()
    {
        AnsiConsole.Clear();

        string[] menuOptions =
                {"Start Study Session", "Flashcard Menu",
                "Edit Stacks", "View Reports", "Exit Program",};

        string choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
                            .PageSize(10)
                            .MoreChoicesText("Keep scrolling for more options")
                            .AddChoices(menuOptions));

        /* Before, the menu selection was parsed based on an int.parse of the first character, which was a number. 
        *  But having the numbers could confuse the user, since you can't input a number in the menu.
        *  So instead, menuSelection is the index in the menu array + 1 (the +1 is for ease of human readability) */

        int menuSelection = Array.IndexOf(menuOptions, choice) + 1;

        switch (menuSelection)
        {
            case 1:
                HandleStudySession();
                break;
            case 2:
                HandleFlashCardSubmenu();
                break;
            case 3:
                HandleStackSubmenu();
                break;
            case 4:
                HandleReportSubmenu();
                break;
            case 5:
                Environment.Exit(0);
                break;
        }
    }

    private void HandleStudySession()
    {
        AnsiConsole.Markup("\n[red]NOT READY, TURN BACK!![/]");
        Console.ReadLine();
        ShowMainMenu();
        throw new NotImplementedException();
    }

    private void HandleFlashCardSubmenu()
    {
        string[] flashcardMenuOptions =
               {"Add Flashcard", "Delete Flashcard", "Display all cards in stack", "Return to Main Menu",};

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
            .PageSize(10)
            .MoreChoicesText("Keep scrolling for more options")
            .AddChoices(flashcardMenuOptions));

        int menuSelection = Array.IndexOf(flashcardMenuOptions, choice) + 1;

        switch (menuSelection)
        {
            case 1:
                displayService.DisplayAllStacks(stackController.GetListOfStacks());
                //cardController.InsertCard();
                ShowMainMenu();
                break;
            case 2:
                ShowMainMenu();
                break;
            case 3:
                ShowMainMenu();
                break;
            case 4:
                ShowMainMenu();
                break;
        }
    }

    private void HandleStackSubmenu()
    {
        AnsiConsole.Markup("\n[red]NOT READY, TURN BACK!![/]");
        Console.ReadLine();
        ShowMainMenu();
        throw new NotImplementedException();
    }

    private void HandleReportSubmenu()
    {
        AnsiConsole.Markup("\n[red]NOT READY, TURN BACK!![/]");
        Console.ReadLine();
        ShowMainMenu();
        throw new NotImplementedException();
    }
}

using Spectre.Console;
using Library;
using System.Reflection;
using Library.Models;

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
                {"Study Session Menu", "Flashcard Menu",
                "Stacks Menu", "View Reports", "Exit Program",};

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
        string[] studySessionOptions =
               {"Start Study Session", "View Past Study Sessions", "Return to Main Menu",};

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
            .PageSize(10)
            .MoreChoicesText("Keep scrolling for more options")
            .AddChoices(studySessionOptions));

        int menuSelection = Array.IndexOf(studySessionOptions, choice) + 1;

        switch (menuSelection)
        {
            case 1:
                displayService.DisplayAllStacks(stackController.GetListOfStacks(), false);
                StackDTO stackInfo = stackController.GetStackDTOByName();
                // There might be too much method chaining going on here. I didn't want to declare a bunch of variables in my menuHandler class, but idk if that or this is worse...
                StackDTO completeStack = studySessionController.GetCardsWithStack(cardController.GetCardDTOs(stackInfo.stackId), stackInfo);
                studySessionController.PerformStudySession(completeStack);
                ShowMainMenu();
                break;
            case 2:
                displayService.DisplayStudySessions(studySessionController.FetchAllStudySession());
                ShowMainMenu();
                break;
            case 3:
                ShowMainMenu();
                break;
        }
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
                displayService.DisplayAllStacks(stackController.GetListOfStacks(), false);
                cardController.InsertCard();
                ShowMainMenu();
                break;
            case 2:
                displayService.DisplayAllStacks(stackController.GetListOfStacks(), false);
                int stackId = cardController.GetStackIdFromUser();
                displayService.DisplayCards(cardController.GetCardDTOs(stackId), false);
                cardController.DeleteCard(stackId);
                ShowMainMenu();
                break;
            case 3:
                displayService.DisplayAllStacks(stackController.GetListOfStacks(), false);
                stackId = cardController.GetStackIdFromUser();
                displayService.DisplayCards(cardController.GetCardDTOs(stackId));
                ShowMainMenu();
                break;
            case 4:
                ShowMainMenu();
                break;
        }
    }

    private void HandleStackSubmenu()
    {
        string[] stackMenuOptions =
               {"Add Stack", "Delete Stack", "Display All Stacks", "Return to Main Menu",};

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
            .PageSize(10)
            .MoreChoicesText("Keep scrolling for more options")
            .AddChoices(stackMenuOptions));

        int menuSelection = Array.IndexOf(stackMenuOptions, choice) + 1;

        switch (menuSelection)
        {
            case 1:
                displayService.DisplayAllStacks(stackController.GetListOfStacks(), false);
                stackController.InsertStack();
                ShowMainMenu();
                break;
            case 2:
                displayService.DisplayAllStacks(stackController.GetListOfStacks(), false);
                stackController.DeleteStackById();
                ShowMainMenu();
                break;
            case 3:
                displayService.DisplayAllStacks(stackController.GetListOfStacks());
                ShowMainMenu();
                break;
            case 4:
                ShowMainMenu();
                break;
        }
    }

    private void HandleReportSubmenu()
    {
        AnsiConsole.Markup("\n[red]NOT READY, TURN BACK!![/]");
        Console.ReadLine();
        ShowMainMenu();
        throw new NotImplementedException();
    }
}

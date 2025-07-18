namespace DotNETConsole.Flashcards.Controllers;

using Enums;
using DTO;
using UI;
using Views;
using Helper;
using Database;

public class MenuController
{
    static bool Running { get; set; }
    private StackController _stackController = new StackController();
    private CardController _cardController = new CardController();
    private StudySessionController _studySessionController = new StudySessionController();

    public MenuController()
    {
        Running = true;
    }

    public void MainMenu()
    {
        var menu = new Menu();
        var userViews = new UserViews();
        var userInput = new UserInput();
        while (Running)
        {
            Console.Clear();
            var userChoice = menu.GetChoice();
            switch (userChoice)
            {
                case MainUI.StartStudySession:
                    var selectedStackForSession = userInput.SelectSingleStack();
                    if (selectedStackForSession is not null)
                    {
                        List<CardViewDto> stackCards = _cardController.GetAllCardsView();
                        if (stackCards.Count == 0)
                        {
                            userViews.Tost("No Card Found !!", "info");
                            userInput.ContinueInput();
                        }
                        else
                        {
                            int sessionId = _studySessionController.AddSession(selectedStackForSession.ID);
                            int score = userViews.CheckKnowledge(stackCards, selectedStackForSession.Name);
                            _studySessionController.UpdateScore(sessionId, score);
                        }
                    }
                    else
                    {

                        userInput.ContinueInput();
                    }
                    break;
                case MainUI.ViewSessionLog:
                    List<SessionDto> sessionLogs = _studySessionController.GetAllSession();
                    if (sessionLogs.Count == 0)
                    {
                        userViews.Tost("No log found !!", "info");
                    }
                    else
                    {
                        userViews.PrintSessionTable(sessionLogs);
                    }
                    userInput.ContinueInput();
                    break;
                case MainUI.ViewStack:
                    List<StackViewDto> stackViews = _stackController.GetStacksView();
                    if (stackViews.Count == 0)
                    {
                        userViews.Tost("No Stack Found!", "info");
                    }
                    else
                    {

                        userViews.PrintStackTable(stackViews);
                    }
                    userInput.ContinueInput();
                    break;
                case MainUI.ViewCards:
                    List<CardViewDto> cards = _cardController.GetAllCardsView();
                    if (cards.Count == 0)
                    {
                        userViews.Tost("No Card Found !!", "info");
                    }
                    else
                    {

                        userViews.PrintCardTable(cards);
                    }
                    userInput.ContinueInput();
                    break;
                case MainUI.CreateStack:
                    string userStackNameInput = userInput.GetStackName();
                    _stackController.CeateStack(userStackNameInput);
                    userInput.ContinueInput();
                    break;
                case MainUI.ModifyStack:
                    var selectedStackForModification = userInput.SelectSingleStack();
                    if (selectedStackForModification is not null)
                    {
                        userViews.ContentSummary($"Choose Option for: {selectedStackForModification.Name}");
                        var stackModifiactionChoiceKey = userInput.ModifyOptionPrompt();
                        switch (stackModifiactionChoiceKey?.Key)
                        {
                            case ConsoleKey.Escape:
                                break;
                            case ConsoleKey.D:
                                _stackController.DeleteStack(selectedStackForModification);
                                userInput.ContinueInput();
                                break;
                            case ConsoleKey.E:
                                Console.Clear();
                                userViews.ContentSummary($"Editing: {selectedStackForModification.Name}");
                                string newStackNameInput = userInput.GetStackName();
                                _stackController.UpdateStack(newStackNameInput, selectedStackForModification);
                                userInput.ContinueInput();
                                break;
                        }
                    }
                    else
                    {
                        userInput.ContinueInput();
                    }
                    break;
                case MainUI.ModifyCard:
                    var selectedCardForModification = userInput.SelectSingleCard();
                    if (selectedCardForModification is not null)
                    {
                        userViews.ContentSummary($"Choose Option for: {selectedCardForModification.Question}");
                        var stackModifiactionChoiceKey = userInput.ModifyOptionPrompt();
                        switch (stackModifiactionChoiceKey?.Key)
                        {
                            case ConsoleKey.Escape:
                                break;
                            case ConsoleKey.D:
                                _cardController.DeleteCard(selectedCardForModification.ID);
                                userInput.ContinueInput();
                                break;
                            case ConsoleKey.E:
                                Console.Clear();
                                userViews.ContentSummary($"Editing: {selectedCardForModification.Question}");
                                _cardController.ModifyCard(selectedCardForModification.ID);
                                userInput.ContinueInput();
                                break;
                        }
                    }
                    else
                    {
                        userInput.ContinueInput();
                    }
                    break;
                case MainUI.AddCards:
                    var selectedStackForNewCard = userInput.SelectSingleStack();
                    if (selectedStackForNewCard is not null)
                    {
                        int numberOfCard = userInput.GetNumberOfCard();
                        for (int i = 0; i < numberOfCard; i++)
                        {
                            _cardController.CreateNewCard(selectedStackForNewCard);
                        }
                    }
                    userInput.ContinueInput();
                    break;
                case MainUI.Reset:
                    string confimation = userInput.DeleteConfimation();
                    if (confimation == "yes")
                    {
                        var migration = new Migration();
                        migration.Reset();
                        userInput.ContinueInput("Database Reseted!!!");
                    }
                    Console.Clear();
                    break;
                case MainUI.Exit:
                    Running = false;
                    Console.Clear();
                    break;
            }
        }
    }
}

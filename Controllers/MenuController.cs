namespace DotNETConsole.Flashcards.Controllers;

using Enums;
using DTO;
using UI;
using Views;
using Helper;
using Models;

public class MenuController
{
    static bool Running { get; set; }
    private StackController _stackController = new StackController();
    private CardController _cardController = new CardController();

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
                        _cardController.CreateNewCard(selectedStackForNewCard);
                    }
                    userInput.ContinueInput();
                    break;
                case MainUI.Exit:
                    Running = false;
                    Console.Clear();
                    break;
            }
        }
    }
}

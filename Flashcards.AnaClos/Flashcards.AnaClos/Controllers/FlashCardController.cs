using Spectre.Console;
using Flashcards.AnaClos.DTOs;
using Flashcards.AnaClos.Models;
using System.Reflection.Metadata.Ecma335;

namespace Flashcards.AnaClos.Controllers;

public class FlashCardController
{
    private ConsoleController _consoleController;
    private DataBaseController _dataBaseController;
    private StackController _stackController;
    public FlashCardController(ConsoleController consoleController, DataBaseController dataBaseController, StackController stackController)
    {
        _consoleController = consoleController;
        _dataBaseController = dataBaseController;
        _stackController = stackController;
    }

    public List<FlashCard> GetFlashCards(int stackId)
    {
        var flashCards = new List<FlashCard>();
        var sql = $"SELECT * FROM FlashCards WHERE Id ={stackId}";
        try
        {
            flashCards = _dataBaseController.Query<FlashCard>(sql);
        }
        catch (Exception ex)
        {
            _consoleController.ShowMessage(ex.Message, "red bold");
            _consoleController.PressKey("Press any key to continue");
        }
        return flashCards;
    }

    public List<FlashCard> GetFlashCards()
    {
        var flashCards = new List<FlashCard>();
        var sql = "SELECT * FROM FlashCards";
        try
        {
            flashCards = _dataBaseController.Query<FlashCard>(sql);
        }
        catch (Exception ex)
        {
            _consoleController.ShowMessage(ex.Message, "red bold");
            _consoleController.PressKey("Press any key to continue");
        }
        return flashCards;
    }

    public List<string> GetFlashCardsFronts(List<FlashCard> flashCards)
    {
        return  flashCards.Select(x => x.Front).ToList();
    }

    public List<FlashCardDTO> GetFlashCardsDTO(List<FlashCard> flashCards)
    {
        int order = 1;
        var flashCardDTOs = new List<FlashCardDTO>();
        foreach(var flashCard in flashCards) 
        {
            flashCardDTOs.Add(new FlashCardDTO { Back = flashCard.Back, Front = flashCard.Front, SequentialId = order, StackId = flashCard.StackId });
            order++;
        }
        return flashCardDTOs;
    }

    public void DeleteFlashCard()
    {
        string selectStack = "Select a Stack Where your flash card resides.";
        string selectFlashCard = "Select a flashcard to delete.";
        string noFlashCard = "There is no flashcard to delete.";
        string message;
        string returnOption = "Return to Main";
        var stacks = _stackController.GetStacks();
        //var flashcards = GetFlashCards();
        var stackNames = _stackController.GetStackNames(stacks);
        stackNames.Add(returnOption);

        string response = _consoleController.Menu(selectStack, "blue", stackNames);
        if (response == returnOption)
        {
            return;
        }
        int stackId = stacks.FirstOrDefault(x => x.Name == response).Id;
        var flashcards = GetFlashCards(stackId);
        var flashcardsFronts = GetFlashCardsFronts(flashcards);
        flashcardsFronts.Add(returnOption);
        message = flashcardsFronts.Count > 1 ? selectFlashCard : noFlashCard;

        response = _consoleController.Menu(message, "blue", flashcardsFronts);
        if (response == returnOption)
        {
            return;
        }
        var flashCard = new FlashCard { Front = response };
        var sql = "DELETE FlashCards WHERE Front = @Front";
        int rows;
        try
        {
            rows = _dataBaseController.Execute<FlashCard>(sql, flashCard);
            _consoleController.ShowMessage($"{rows} flashcard deleted.", "green");
            _consoleController.PressKey("Press any key to continue");
        }
        catch (Exception ex)
        {
            _consoleController.ShowMessage(ex.Message, "red bold");
            _consoleController.PressKey("Press any key to continue");
        }
    }

    public void AddFlashCard()
    {
        int rows;
        int stackId;
        Stack? stack;
        string message = "Will the flashcard be for a new stack?";
        var confirmation = AnsiConsole.Prompt(
        new TextPrompt<bool>(message)
            .AddChoice(true)
            .AddChoice(false)
            .DefaultValue(true)
            .WithConverter(choice => choice ? "yes" : "no"));
        if (confirmation)
        {
            stack = _stackController.AddStack();
            if (stack==null)
            {
                return;
            }
            stackId= stack.Id;
        }
        else
        {
            string title = "Select a stack to add the FlashCard to.";
            string returnOption = "Return to Main";
            var stacks = _stackController.GetStacks();
            if(stacks==null || stacks.Count == 0)
            {
                _consoleController.ShowMessage("You must enter a Stack first.", "red bold");
                _consoleController.PressKey("Press any key to continue");
                return;
            }
            var stackNames = _stackController.GetStackNames(stacks);
            stackNames.Add(returnOption);
            string response = _consoleController.Menu(title, "blue", stackNames);
            if (response == returnOption)
            {
                return;
            }
            stackId = stacks.FirstOrDefault(x => x.Name == response).Id;
        }
        string front = _consoleController.GetString("Please enter the front of the flashcard or press 0 to return to main menu:");
        while (front == "")
        {
            front = _consoleController.GetString("Please enter the front of the flashcard or press 0 to return to main menu:");
        }        
        if (front == "0")
            return;
        string back = _consoleController.GetString("Please enter the back of the flashcard or press 0 to return to main menu:");
        while (back == "")
        {
            back = _consoleController.GetString("Please enter the back of the flashcard or press 0 to return to main menu:");
        }
        
        if (back == "0")
            return;

        try
        {
            var flashCard = new FlashCard { Front = front, Back = back, StackId = stackId };
            var sql = "INSERT INTO FlashCards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)";
            rows = _dataBaseController.Execute<FlashCard>(sql, flashCard);
            _consoleController.ShowMessage($"{rows} flashCard added.", "green");
            _consoleController.PressKey("Press any key to continue");
        }
        catch (Exception ex)
        {
            _consoleController.ShowMessage(ex.Message, "red bold");
            _consoleController.PressKey("Press any key to continue");
        }
    }
}
using Spectre.Console;
using Flashcards.AnaClos.DTOs;
using Flashcards.AnaClos.Models;

namespace Flashcards.AnaClos.Controllers;

public class FlashCardController
{
    private ConsoleController _consoleController;
    private DataBaseController _dataBaseController;
    private StackController _stackController;
    private string returnOption = "Return to Main";

    public FlashCardController(ConsoleController consoleController, DataBaseController dataBaseController, StackController stackController)
    {
        _consoleController = consoleController;
        _dataBaseController = dataBaseController;
        _stackController = stackController;
    }

    public List<FlashCardDTO> GetFlashCardsDTO(string stackName)
    {
        return FlashCardDTO.Flashcards.Where(x => x.StackName == stackName).ToList();
    }

    public List<TableRecordDTO> FrontToTableRecord(List<FlashCardDTO> flashCards)
    {
        var tableRecord = new List<TableRecordDTO>();
        foreach(var flashCard in flashCards)
        {
            var record = new TableRecordDTO { Column1=flashCard.SequentialId.ToString(),Column2=flashCard.Front};
            tableRecord.Add(record);
        }
        return tableRecord;
    }

    public List<TableRecordDTO> BackToTableRecord(List<FlashCard> flashCards)
    {
        var tableRecord = new List<TableRecordDTO>();
        foreach (var flashCard in flashCards)
        {
            var record = new TableRecordDTO { Column1 = flashCard.StackId.ToString(), Column2 = flashCard.Back };
        }
        return tableRecord;
    }

    public List<FlashCard> GetFlashCards(int stackId)
    {
        var flashCards = new List<FlashCard>();
        var sql = $"SELECT * FROM FlashCards WHERE StackId ={stackId}";
        try
        {
            flashCards = _dataBaseController.Query<FlashCard>(sql);
        }
        catch (Exception ex)
        {
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
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
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
        }
        return flashCards;
    }

    public List<string> GetFlashCardsFronts(List<FlashCard> flashCards)
    {
        return  flashCards.Select(x => x.Front).ToList();
    }

    public void UpdateOrderFlashCardsDTO(string stackName)
    {
        int order = 1;
        foreach (var flashCard in FlashCardDTO.Flashcards)
        {
            if (flashCard.StackName == stackName)
            {
                flashCard.SequentialId = order++;
            }
        }
    }

    public void DeleteFlashCard()
    {
        string selectStack = "Select a Stack Where your flash card resides.";
        string selectFlashCard = "Select a flashcard to delete.";
        string noFlashCard = "There is no flashcard to delete.";
        string message;
        
        var stacks = _stackController.GetStacks();
        var stackNames = _stackController.GetStackNames(stacks);
        stackNames.Add(returnOption);

        string stackName = _consoleController.Menu(selectStack, "blue", stackNames);
        if (stackName == returnOption)
        {
            return;
        }
        int stackId = stacks.FirstOrDefault(x => x.Name == stackName).Id;
        var flashcards = GetFlashCards(stackId);
        var flashcardsFronts = GetFlashCardsFronts(flashcards);
        flashcardsFronts.Add(returnOption);
        message = flashcardsFronts.Count > 1 ? selectFlashCard : noFlashCard;

        var response = _consoleController.Menu(message, "blue", flashcardsFronts);
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

            FlashCardDTO.Flashcards.RemoveAll(x=>x.Front==response);

            UpdateOrderFlashCardsDTO(stackName);

            _consoleController.MessageAndPressKey($"{rows} flashcard indexes updated.", "green");
        }
        catch (Exception ex)
        {
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
        }
    }

    public void AddFlashCard()
    {
        int rows;
        int stackId;
        string stackName = string.Empty;
        Stack stack;
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
            stackName = stack.Name;
        }
        else
        {
            string title = "Select a stack to add the FlashCard to.";
            var stacks = _stackController.GetStacks();
            if(stacks==null || stacks.Count == 0)
            {
                _consoleController.MessageAndPressKey("You must enter a Stack first.", "red bold");
                return;
            }
            var stackNames = _stackController.GetStackNames(stacks);
            stackNames.Add(returnOption);
            stackName = _consoleController.Menu(title, "blue", stackNames);
            if (stackName == returnOption)
            {
                return;
            }
            stackId = stacks.FirstOrDefault(x => x.Name == stackName).Id;
        }
        string front = _consoleController.GetString("Please enter the front of the flashcard or press # to return to main menu:");
      
        if (front.Trim() == "#")
            return;

        string back = _consoleController.GetString("Please enter the back of the flashcard or press # to return to main menu:");
        
        if (back.Trim() == "#")
            return;

        try
        {
            var flashCard = new FlashCard { Front = front, Back = back, StackId = stackId };
            var sql = "INSERT INTO FlashCards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)";
            rows = _dataBaseController.Execute<FlashCard>(sql, flashCard);

            var flashCardDTO = new FlashCardDTO { Front = front, Back = back, StackName = stackName };

            FlashCardDTO.Flashcards.Add(flashCardDTO);
            UpdateOrderFlashCardsDTO(stackName);

            _consoleController.MessageAndPressKey($"{rows} flashCard added.", "green");
        }
        catch (Exception ex)
        {
            _consoleController.MessageAndPressKey(ex.Message,"red bold");
        }
    }   
}
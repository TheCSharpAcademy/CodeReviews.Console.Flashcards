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

    public void InitFlashCardsDto()
    {
        var flashcards = GetFlashCards();
        foreach(var flashcard in flashcards)
        {
            var stack = _stackController.GetStackById(flashcard.StackId);
            FlashCardDto.Flashcards.Add(new FlashCardDto { Front = flashcard.Front, Back = flashcard.Back,StackName=stack.Name });
        }
    }

    public List<FlashCardDto> GetFlashCardsDto(string stackName)
    {
        return FlashCardDto.Flashcards.Where(x => x.StackName == stackName).ToList();
    }

    public List<TableRecordDto> FrontToTableRecord(List<FlashCardDto> flashCards)
    {
        var tableRecord = new List<TableRecordDto>();
        foreach(var flashCard in flashCards)
        {
            var record = new TableRecordDto { Column1=flashCard.SequentialId.ToString(),Column2=flashCard.Front};
            tableRecord.Add(record);
        }
        return tableRecord;
    }

    public List<TableRecordDto> BackToTableRecord(List<FlashCardDto> flashCards)
    {
        var tableRecord = new List<TableRecordDto>();
        foreach (var flashCard in flashCards)
        {
            var record = new TableRecordDto { Column1 = flashCard.SequentialId.ToString(), Column2 = flashCard.Back };
            tableRecord.Add(record);
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

    public void UpdateOrderFlashCardsDto(string stackName)
    {
        int order = 1;
        foreach (var flashCard in FlashCardDto.Flashcards)
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

            FlashCardDto.Flashcards.RemoveAll(x=>x.Front==response);

            UpdateOrderFlashCardsDto(stackName);

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

            var flashCardDto = new FlashCardDto { Front = front, Back = back, StackName = stackName };

            FlashCardDto.Flashcards.Add(flashCardDto);
            UpdateOrderFlashCardsDto(stackName);

            _consoleController.MessageAndPressKey($"{rows} flashCard added.", "green");
        }
        catch (Exception ex)
        {
            _consoleController.MessageAndPressKey(ex.Message,"red bold");
        }
    }   
}
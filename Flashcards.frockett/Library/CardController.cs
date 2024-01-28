using DataAccess;
using Library.Models;
using Spectre.Console;

namespace Library;

public class CardController
{
    private readonly IDataAccess dataAccess;
    private readonly InputValidation inputValidation;

    public CardController(IDataAccess dataAccess, InputValidation inputValidation)
    {
        this.dataAccess = dataAccess;
        this.inputValidation = inputValidation;
    }

    public void InsertCard()
    {
        int stackToUpdate = inputValidation.GetStackId(dataAccess.GetListOfStacks());
        //dataAccess.GetStackById(stackToUpdate);

        CardDTO newFlashcardDTO = inputValidation.GetNewFlashCardInput();

        CardModel newFlashcard = new CardModel();
        newFlashcard.StackId = stackToUpdate;
        newFlashcard.Question = newFlashcardDTO.Question;
        newFlashcard.Answer = newFlashcardDTO.Answer;

        dataAccess.InsertCard(newFlashcard);
    }

    public void DeleteCard()
    {
        // instantiate mapper class, create map
        IdMapper mapper = new IdMapper();
        List<CardModel> flashcards = GetCardsInStack();
        mapper.BuildFlashCardMap(flashcards);

        // cache stack id from first element in list, then get the desired index
        int index = inputValidation.GetCardIndex("Enter the number of the card you'd like to delete: ");

        int dbId = mapper.RetrieveIdFromMap(index);

        if (dbId == -1)
        {
            AnsiConsole.Markup("[red]The card you've selected does not exist[/]\n");
            return;
        }
        else
        {
            dataAccess.DeleteCardById(dbId);
        }
    }

    private List<CardModel> GetCardsInStack()
    {
        // Send a list of all current stacks to the input validation class, which checks the name against the name property of each item.
        int stackId = inputValidation.GetStackId(dataAccess.GetListOfStacks());
        // Get all the card models in a list based on the stackId
        List<CardModel> cards = dataAccess.GetCardsInStack(stackId);
        // Return the cards (probably to the GetCardDTOs method)
        return cards;
    }

    public List<CardDTO> GetCardDTOs()
    {
        List<CardDTO> cardDTOs = new List<CardDTO>();
        
        List<CardModel> flashcards = GetCardsInStack();

        foreach (CardModel card in flashcards)
        {
            cardDTOs.Add(
                new CardDTO(card.Question, card.Answer));
        }

        return cardDTOs;
    }

}

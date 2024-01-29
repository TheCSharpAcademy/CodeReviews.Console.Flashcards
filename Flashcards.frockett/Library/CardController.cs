using DataAccess;
using Library.Models;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;

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
        int stackToUpdate = GetStackIdFromUser();
        //dataAccess.GetStackById(stackToUpdate);

        CardDTO newFlashcardDTO = inputValidation.GetNewFlashCardInput();

        CardModel newFlashcard = new CardModel();
        newFlashcard.StackId = stackToUpdate;
        newFlashcard.Question = newFlashcardDTO.Question;
        newFlashcard.Answer = newFlashcardDTO.Answer;

        dataAccess.InsertCard(newFlashcard);
    }

    public void DeleteCard(int stackId)
    {
        // instantiate mapper class, create map
        IdMapper mapper = new IdMapper();
        List<CardModel> flashcards = GetCardsInStack(stackId);
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

    public int GetStackIdFromUser()
    {
        // send a list of all current stacks from dataAccess to inputValidation to present to the user
        StackModel selectedStack = inputValidation.GetMatchingStackFromList(dataAccess.GetListOfStacks(), "Enter the name of the stack you'd like to select: ");
        int stackId = selectedStack.Id;
        return stackId;
    }
    private List<CardModel> GetCardsInStack(int stackId)
    {
        // Get all the card models in a list based on the stackId
        List<CardModel> cards = dataAccess.GetCardsByStackId(stackId);
        // Return the cards (probably to the GetCardDTOs method)
        return cards;
    }

    public List<CardDTO> GetCardDTOs(int stackId)
    {
        List <CardModel> flashcards = GetCardsInStack(stackId);

        List<CardDTO> cardDTOs = new List<CardDTO>();
        
        foreach (CardModel card in flashcards)
        {
            cardDTOs.Add(
                new CardDTO(card.Question, card.Answer));
        }

        return cardDTOs;
    }

}

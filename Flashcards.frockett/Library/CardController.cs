using DataAccess;
using Library.Models;

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
        dataAccess.GetStackById(stackToUpdate);
        CardModel newFlashcard = inputValidation.GetNewFlashCardInput();
        dataAccess.InsertCard(newFlashcard);
    }

    public List<CardDTO> GetCardsInStack()
    {
        int stackId = inputValidation.GetStackId(dataAccess.GetListOfStacks());
        
        List<CardModel> cards = dataAccess.GetCardsInStack(stackId);

        List<CardDTO> cardDTOs = new List<CardDTO>();

        foreach (CardModel card in cards)
        {
            cardDTOs.Add(
                new CardDTO(card.Question, card.Answer));
        }

        return cardDTOs;
    }

}

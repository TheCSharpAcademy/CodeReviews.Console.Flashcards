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
        int stackToUpdate = inputValidation.GetStackId();
        dataAccess.GetStackById(stackToUpdate);
        CardModel newFlashcard = inputValidation.GetNewFlashCardInput();
        dataAccess.InsertCard(newFlashcard);
    }


}

using FlashcardsLibrary.Data;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Controller;

internal class CardManager : Controller
{
    internal void EditPack()
    {
        string packChoice = PackMenu(PackGateway.GetPacksList());
        // Get list of cards in pack
        List<CardModel> cards = CardGateway.GetPackList(packChoice);
        // Display list
        List<CardFaceDTO>
        // Run CardManager with pack passed in
        throw new NotImplementedException();
    }

    internal void ManageCards()
    {
        throw new NotImplementedException();
    }
}

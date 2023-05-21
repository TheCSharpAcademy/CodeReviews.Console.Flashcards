using FlashcardsLibrary.Data;

namespace Flashcards.CoreyJordan.Controller;

internal class CardManager : Controller
{
    internal void EditPack()
    {
        string packChoice = PackMenu(PackGateway.GetPacksList());
        // Get list of cards in pack
        // Display list
        // Run CardManager with pack passed in
        throw new NotImplementedException();
    }

    internal void ManageCards()
    {
        throw new NotImplementedException();
    }
}

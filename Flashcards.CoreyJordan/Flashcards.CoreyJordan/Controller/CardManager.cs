using Flashcards.CoreyJordan.Display;
using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary.Data;

namespace Flashcards.CoreyJordan.Controller;

internal class CardManager : Controller
{
    internal void EditPack()
    {
        List<PackNamesDTO> allPacks = PackNamesDTO.GetPacksDTO(PackGateway.GetPacksList());
        UIPack.DisplayPacks(allPacks);
        string choiceName = UIPack.ChoosePack(allPacks);
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

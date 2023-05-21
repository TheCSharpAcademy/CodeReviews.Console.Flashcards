using Flashcards.CoreyJordan.Display;
using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Controller;
internal abstract class Controller
{
    internal ConsoleUI UIConsole { get; } = new();
    internal PackUI UIPack { get; } = new();
    internal InputModel UserInput { get; } = new();

    internal string PackMenu(List<PackModel> packs)
    {
        List<PackNamesDTO> allPacks = PackNamesDTO.GetPacksDTO(packs);
        UIPack.DisplayPacks(allPacks);

        return UIPack.ChoosePack(allPacks);
    }
}

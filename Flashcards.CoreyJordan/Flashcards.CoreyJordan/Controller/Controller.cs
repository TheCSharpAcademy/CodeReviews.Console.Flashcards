using Flashcards.CoreyJordan.Display;
using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary.Data;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Controller;
internal abstract class Controller
{
    internal ConsoleUI UIConsole { get; } = new();
    internal PackUI UIPack { get; } = new();
    internal InputModel UserInput { get; } = new();
    internal CardUI UICard { get; } = new();

    internal List<PackNamesDto> DisplayPacksList(List<PackModel> packs)
    {
        List<PackNamesDto> allPacks = PackNamesDto.GetPacksDto(packs);
        UIPack.DisplayPacks(allPacks);

        return allPacks;
    }

    internal List<CardFaceDto> DisplayCardList(List<CardModel> cards)
    {
        List<CardFaceDto> allCards = CardFaceDto.GetCardsDto(cards);
        UICard.DisplayCards(allCards);

        return allCards;
    }
}

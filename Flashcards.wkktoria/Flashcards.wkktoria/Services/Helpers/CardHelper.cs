using Flashcards.wkktoria.Models.Dtos;

namespace Flashcards.wkktoria.Services.Helpers;

internal static class CardHelper
{
    internal static List<CardDto> ToFullDto(List<CardDto> cards)
    {
        if (!cards.Any()) return cards;

        var size = cards.Count;

        for (var i = 0; i < size; i++) cards[i].DtoId = i + 1;

        return cards;
    }
}
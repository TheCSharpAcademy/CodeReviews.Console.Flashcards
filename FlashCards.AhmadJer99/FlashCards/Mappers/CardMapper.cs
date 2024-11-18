using FlashCards.Models;
using FlashCards.Dtos;
namespace FlashCards.Mappers;

internal static class CardMapper
{
    public static CardDto ToCardDto(this Card card)
    {
       return new CardDto()
        {
           cardnumber = card.cardnumber,
           front = card.front,
           back = card.back
        };
    }
}

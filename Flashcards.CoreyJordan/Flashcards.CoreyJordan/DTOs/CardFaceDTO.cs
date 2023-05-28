using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
internal class CardFaceDto
{
    public int CardNumber { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }

    public CardFaceDto(int number, string question, string answer)
    {
        CardNumber = number;
        Question = question;
        Answer = answer;
    }

    public static List<CardFaceDto> GetCardsDto(List<CardModel> cardModels)
    {
        List<CardFaceDto> cards = new();
        for (int i = 0; i < cardModels.Count; i++)
        {
            cards.Add(new CardFaceDto(i + 1, cardModels[i].Question, cardModels[i].Answer));
        }
        return cards;
    }
}

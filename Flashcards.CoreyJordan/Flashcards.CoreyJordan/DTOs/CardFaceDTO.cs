using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
internal class CardFaceDTO
{
    public int CardNumber { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }

    public CardFaceDTO(int number, string question, string answer)
    {
        CardNumber = number;
        Question = question;
        Answer = answer;
    }

    public static List<CardFaceDTO> GetCardsDTO(List<CardModel> cardModels)
    {
        List<CardFaceDTO> cards = new();
        for (int i = 0; i < cardModels.Count; i++)
        {
            cards.Add(new CardFaceDTO(i + 1, cardModels[i].Question, cardModels[i].Answer));
        }
        return cards;
    }
}

using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
internal class CardFaceDTO
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public CardFaceDTO(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }

    public static List<CardFaceDTO> GetCardsDTO(List<CardModel> cardModels)
    {
        List<CardFaceDTO> cards = new();
        for (int i = 0; i < cardModels.Count; i++)
        {
            cards.Add(new CardFaceDTO(cardModels[i].Question, cardModels[i].Answer));
        }
        return cards;
    }
}

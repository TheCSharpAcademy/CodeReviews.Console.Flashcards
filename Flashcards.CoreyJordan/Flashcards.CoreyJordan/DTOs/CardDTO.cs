using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
internal class CardDto
{
    public int Number { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Deck { get; set; }

    public CardDto(int number, string question, string answer, string deckName)
    {
        Number = number;
        Question = question;
        Answer = answer;
        Deck = deckName;
    }

    public static List<CardDto> GetListDto(List<CardModel> cardModels)
    {
        List<CardDto> cards = new();
        for (int i = 0; i < cardModels.Count; i++)
        {
            cards.Add(new CardDto(i + 1, cardModels[i].Question, cardModels[i].Answer, cardModels[i].DeckName));
        }
        return cards;
    }
}

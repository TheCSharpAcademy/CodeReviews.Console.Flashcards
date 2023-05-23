using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
internal class CardDTO
{
    public int Number { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Deck { get; set; }

    public CardDTO(int number, string question, string answer, string deckName)
    {
        Number = number;
        Question = question;
        Answer = answer;
        Deck = deckName;
    }

    public static List<CardDTO> GetListDTO(List<CardModel> cardModels)
    {
        List<CardDTO> cards = new();
        for (int i = 0; i < cardModels.Count; i++)
        {
            cards.Add(new CardDTO(i + 1, cardModels[i].Question, cardModels[i].Answer, cardModels[i].DeckName));
        }
        return cards;
    }
}

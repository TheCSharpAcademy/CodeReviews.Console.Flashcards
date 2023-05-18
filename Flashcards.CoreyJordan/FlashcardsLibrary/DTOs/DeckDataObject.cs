using FlashcardsLibrary.Models;

namespace FlashcardsLibrary.DTOs;
public class DeckDataObject
{
    public string Name { get; set; }
    public List<FlashCardDataObject> Deck { get; set; }

    public DeckDataObject(string name, List<FlashCardModel> deck)
    {
        Name = name;
        Deck = new();
        for (int i = 0; i < deck.Count; i++)
        {
            Deck.Add(new FlashCardDataObject(deck[i]));
            Deck[i].Id = i + 1;

        }
    }
}

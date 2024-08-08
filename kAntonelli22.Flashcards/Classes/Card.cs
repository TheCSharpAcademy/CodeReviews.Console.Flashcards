using Spectre.Console;

namespace Flashcards;

internal class Card
{
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }
    public static List<Card> Cards { get; set; } = new List<Card>();
    public Card(string Front, string Back, CardStack stack)
    {
        this.Front = Front;
        this.Back = Back;
        stack.Cards.Add(this);
    }

    public Card(string Front, string Back, int Stack_Id)
    {
        this.Front = Front;
        this.Back = Back;
        StackId = Stack_Id;
    }
}
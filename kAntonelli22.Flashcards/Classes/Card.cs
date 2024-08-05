using Spectre.Console;

namespace Flashcards;

internal class Card
{
    public string front;
    public string back;
    public static List<Card> Cards { get; set; } = new List<Card>();
    public Card(string cardQuestion, string cardAnswer, CardStack stack)
    {
        front = cardQuestion;
        back = cardAnswer;
        stack.Cards.Add(this);
    }

}
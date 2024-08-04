using Spectre.Console;

namespace Flashcards;

internal class Card
{
    public string question;
    public string answer;
    public static List<Card> Cards { get; set; } = new List<Card>();
    public Card(string cardQuestion, string cardAnswer, CardStack stack)
    {
        question = cardQuestion;
        answer = cardAnswer;
        stack.Cards.Add(this);
    }

}
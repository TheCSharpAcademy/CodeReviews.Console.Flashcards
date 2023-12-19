namespace Flashcards;

class CardsDTO(Cards card, int cardID)
{
    public int CardID = cardID;
    public string Question = card.Question;
    public string Answer = card.Answer;
}
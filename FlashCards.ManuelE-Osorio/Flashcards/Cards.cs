namespace Flashcards;

class Cards
{
    public int CardID;
    public int StackID;
    public string Question;
    public string Answer;

    public Cards(int cardID, int stackID, string question, string answer)
    {
        CardID = cardID;
        StackID = stackID;
        Question = question;
        Answer = answer;
    }
}
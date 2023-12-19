namespace Flashcards;

class Cards(int stackID, string question, string answer, int cardID = 0)
{
    public int CardID = cardID;
    public int StackID = stackID;
    public string Question = question;
    public string Answer = answer;
}
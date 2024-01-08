namespace Flashcards;

class Cards(string stackName, string question, string answer, int cardID = 0)
{
    public int CardID = cardID;
    public string StackName = stackName;
    public string Question = question;
    public string Answer = answer;

    public static Cards FromCsv(string cardLine)
    {
        string[] data = cardLine.Split(',');
        Cards card = new(data[0], data[1], data[2]);
        return card;
    }
}
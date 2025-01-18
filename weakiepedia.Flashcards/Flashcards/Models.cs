namespace Flashcards;

internal class Stack
{
    internal int Id { get; set; }
    internal string Name { get; set; }
}

internal class Flashcard
{
    internal int Id { get; set; }
    internal string Question { get; set; }
    internal string Answer { get; set; }
    internal int StackID { get; set; }
}

internal class Session
{
    internal int Id { get; set; }
    internal int Score { get; set; }
    internal DateTime Date { get; set; }
    internal int StackId { get; set; }
    
    internal Session(int id, int score, DateTime date, int stackId)
    {
        Id = id;
        Score = score;
        Date = date;
        StackId = stackId;
    }
}
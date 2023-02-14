

namespace Lonchanick9427.FlashCard;

public class StudySession
{
    public int Id { get; set; }
    public int StackFk { get; set; }
    public int Score { get; set; }
    public string User_ { get; set; }
    public DateTime Init { get; set; }
    public DateTime Fin { get; set; }
    public StudySession() {}

    public void Print()
    {
        Console.WriteLine("Id Session: "+Id);
        Console.WriteLine("User: " + User_);
        Console.WriteLine("Init: " + Init);
        Console.WriteLine("End: " + Fin);
        Console.WriteLine("Scores: " + Score);
    }

}

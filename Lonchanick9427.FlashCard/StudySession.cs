using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Console.WriteLine(Id);
        Console.WriteLine(User_);
        Console.WriteLine(Init);
        Console.WriteLine(Fin);
        Console.WriteLine(Score);
    }

}

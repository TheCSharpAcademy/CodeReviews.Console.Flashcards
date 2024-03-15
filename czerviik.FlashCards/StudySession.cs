using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards;

public class StudySession
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public int Rounds { get; set; }
    public int StackId { get; set; }
}
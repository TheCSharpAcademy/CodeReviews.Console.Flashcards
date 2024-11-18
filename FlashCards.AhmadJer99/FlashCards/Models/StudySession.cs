
namespace FlashCards.Models;

internal class StudySession
{
    public int FK_stack_id { get; set; }
    public DateTime session_date { get; set; }
    public  int  score { get; set; }
    public int session_id { get; set; }
}


using Flashcards.selnoom.Models;

namespace Flashcards.selnoom.Data;

class StudySession
{
    public int StudySessionId { get; set; }
    public int StackId { get; set; }
    public int Score { get; set; }
    public DateTime SessionDate { get; set; }
}

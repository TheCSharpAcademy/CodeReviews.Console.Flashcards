using Kmakai.FlashCards.Controllers;

namespace Kmakai.FlashCards.Models;

public class StudySession
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public int Score { get; set; } = 0;
    public DateOnly Date { get; set; }
}

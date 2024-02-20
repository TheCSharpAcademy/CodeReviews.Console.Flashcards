
namespace Library.Models;

public class StudySessionModel
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string? StackName { get; set; }
    public DateTime SessionDateTime { get; set; }
    public int Score { get; set; }
}

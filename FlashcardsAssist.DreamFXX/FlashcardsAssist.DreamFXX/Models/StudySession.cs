using System.ComponentModel.DataAnnotations.Schema;

namespace FlashcardsAssist.DreamFXX.Models;
public class StudySession
{
    public int Id { get; set; }
    [ForeignKey("StackId")]
    public int StackId { get; set; }
    public string StackName { get; set; } = string.Empty;
    public DateTime StudyDate { get; set; }
    public int Score { get; set; }
}

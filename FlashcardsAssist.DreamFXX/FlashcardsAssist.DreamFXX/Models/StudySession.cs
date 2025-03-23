using System.ComponentModel.DataAnnotations.Schema;

namespace FlashcardsAssist.DreamFXX.Models;
internal class StudySession
{
    public int Id { get; set; }
    [ForeignKey("StackId")]
    public int StackId { get; set; }
    public DateTime StudyDate { get; set; }
    public int Score { get; set; }
}

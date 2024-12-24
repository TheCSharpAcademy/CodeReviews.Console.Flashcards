namespace FlashCards.Models;
internal class Flashcard
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public virtual Stack Stack { get; set; }
    public string FrontText { get; set; }
    public string BackText { get; set; }
    public DateTime? LastTimeReviewed { get; set; } = null;
    public DateTime? NextTimeToReview { get; set; } = null;
    public int ReviewBreakInSeconds { get; set; } = 60;
    public DateTime CreationTime { get; set; }
}
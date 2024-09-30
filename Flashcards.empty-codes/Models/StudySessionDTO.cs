namespace Flashcards.empty_codes.Models;

internal class StudySessionDto
{
    public int SessionId { get; set; }
    public DateTime StudyDate { get; set; }
    public string? Score { get; set; }
    public int StackId { get; set; }
}
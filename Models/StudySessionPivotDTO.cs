namespace dotnetMAUI.Flashcards.Models;

public class StudySessionPivotDto
{
    public string StackName { get; set; } = null!;
    public Dictionary<string, int> MonthlyCounts { get; set; } = new();
}

namespace dotnetMAUI.Flashcards.Models;

public class StudySessionPivotDTO
{
    public string StackName { get; set; } = null!;
    public Dictionary<string, int> MonthlyCounts { get; set; } = new();
}

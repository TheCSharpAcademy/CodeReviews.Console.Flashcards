namespace Flashcards.Models;

class StudySessionDTO
{
    internal DateTime Date { get; set; }
    internal int Score { get; set; }
    internal string? StackName { get; set; }
}
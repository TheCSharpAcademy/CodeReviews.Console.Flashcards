namespace FlashcardsLibrary;

public class SessionQuestion
{
    public int QuestionId { get; set; }
    public int SessionId { get; set; }
    public required string QuestionText { get; set; }
    public required string AnswerText { get; set; }
    public required string UserAnswer { get; set; }
    public bool IsCorrect { get; set; }
}
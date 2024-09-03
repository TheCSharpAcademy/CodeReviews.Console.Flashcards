namespace FlashcardsLibrary;
public static class SessionQuestionMapper
{
    public static SessionQuestionDto MapToDto(SessionQuestion question)
    {
        return new SessionQuestionDto
        {
            Id = question.QuestionId,
            Question = question.QuestionText,
            Answer = question.AnswerText,
            UserResponse = question.UserAnswer,
            Result = question.IsCorrect,
        };
    }
}
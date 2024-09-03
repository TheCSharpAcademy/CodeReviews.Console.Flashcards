namespace FlashcardsLibrary;
public static class SessionQuestionMapper
{
    public static SessionQuestionDTO MapToDTO(SessionQuestion question)
    {
        return new SessionQuestionDTO
        {
            Id = question.QuestionId,
            Question = question.QuestionText,
            Answer = question.AnswerText,
            UserResponse = question.UserAnswer,
            Result = question.IsCorrect,
        };
    }
}
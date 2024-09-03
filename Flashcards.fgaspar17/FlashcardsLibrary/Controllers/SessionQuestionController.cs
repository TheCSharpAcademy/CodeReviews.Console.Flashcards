using CodingTrackerLibrary;

namespace FlashcardsLibrary;
public class SessionQuestionController
{
    public static List<SessionQuestion> GetSessionQuestionsBySessionId(int sessionId)
    {
        string sql = @"SELECT QuestionId, SessionId, QuestionText, AnswerText, UserAnswer, IsCorrect 
                        FROM SessionQuestion
                        WHERE SessionId = @SessionId;";
        return SqlExecutionService.GetListModelsByKey<int, SessionQuestion>(sql, field: "SessionId", sessionId);
    }

    public static bool InsertSessionQuestion(SessionQuestion sessionQuestion)
    {
        string sql = $@"INSERT INTO dbo.SessionQuestion (SessionId, QuestionText, AnswerText, UserAnswer, IsCorrect) 
                                        VALUES (@SessionId, @QuestionText, @AnswerText, @UserAnswer, @IsCorrect);";

        return SqlExecutionService.ExecuteCommand<SessionQuestion>(sql, sessionQuestion);
    }
}
namespace FlashcardsLibrary;
public class SessionQuestionService
{
    public static List<SessionQuestionDto> GetSessionQuestionsBySessionId(int sessionId)
    {
        return SessionQuestionController.GetSessionQuestionsBySessionId(sessionId).Select(question => SessionQuestionMapper.MapToDto(question)).ToList();
    }
}
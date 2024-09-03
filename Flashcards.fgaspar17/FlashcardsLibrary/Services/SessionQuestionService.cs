namespace FlashcardsLibrary;
public class SessionQuestionService
{
    public static List<SessionQuestionDTO> GetSessionQuestionsBySessionId(int sessionId)
    {
        return SessionQuestionController.GetSessionQuestionsBySessionId(sessionId).Select(question => SessionQuestionMapper.MapToDTO(question)).ToList();
    }
}
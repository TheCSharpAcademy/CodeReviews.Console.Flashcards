namespace FlashcardsLibrary;
public static class StudySessionMapper
{
    public static StudySessionDTO MapToDTO(StudySession session)
    {
        return new StudySessionDTO
        {
            Id = session.SessionId,
            Date = session.SessionDate,
            Score = session.Score,
            Stack = session.StackName,
        };
    }
}
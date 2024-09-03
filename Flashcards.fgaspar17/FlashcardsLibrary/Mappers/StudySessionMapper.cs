namespace FlashcardsLibrary;
public static class StudySessionMapper
{
    public static StudySessionDto MapToDto(StudySession session)
    {
        return new StudySessionDto
        {
            Id = session.SessionId,
            Date = session.SessionDate,
            Score = session.Score,
            Stack = session.StackName,
        };
    }
}
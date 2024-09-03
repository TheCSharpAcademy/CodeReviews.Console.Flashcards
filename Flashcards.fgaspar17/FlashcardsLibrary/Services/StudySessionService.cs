namespace FlashcardsLibrary;
public static class StudySessionService
{
    public static List<StudySessionDto> GetStudySessions()
    {
        List<StudySession> studySessions = StudySessionController.GetStudySessions();
        return studySessions.Select(studySession => StudySessionMapper.MapToDto(studySession)).ToList();
    }
}
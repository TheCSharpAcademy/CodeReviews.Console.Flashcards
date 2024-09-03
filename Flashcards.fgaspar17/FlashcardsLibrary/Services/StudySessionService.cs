namespace FlashcardsLibrary;
public static class StudySessionService
{
    public static List<StudySessionDTO> GetStudySessions()
    {
        List<StudySession> studySessions = StudySessionController.GetStudySessions();
        return studySessions.Select(studySession => StudySessionMapper.MapToDTO(studySession)).ToList();
    }
}
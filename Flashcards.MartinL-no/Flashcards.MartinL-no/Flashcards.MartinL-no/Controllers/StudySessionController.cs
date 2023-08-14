namespace Flashcards.MartinL_no.Controllers;

internal class StudySessionController
{
    private StudySessionRepository _sessionRepo;

    public StudySessionController(StudySessionRepository sessionRepo)
    {
        _sessionRepo = sessionRepo;
    }
}
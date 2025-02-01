using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.Data.Interfaces
{
    public interface IStudySessionDataAccess
    {
        void CreateStudySession(StudySession sSession);
        List<StudySession> GetAllStudySessions();
    }
}
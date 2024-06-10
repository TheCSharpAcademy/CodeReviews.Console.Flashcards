using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository.StudySessions
{
    public interface ISessionRepository
    {
        public void Post(int stackId, int score, int totalQuestions);
        List<Session> GetSessionsByStackId(int id);
        List<Session> GetSessionsByYear(int year);
        void Delete(Session session);
    }
}
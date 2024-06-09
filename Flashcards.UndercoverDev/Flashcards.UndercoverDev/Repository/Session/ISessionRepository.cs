namespace Flashcards.UndercoverDev.Repository.Session
{
    public interface ISessionRepository
    {
        public void Post(int stackId, int score, int totalQuestions);
    }
}
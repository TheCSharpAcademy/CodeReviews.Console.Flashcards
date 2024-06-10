namespace Flashcards.UndercoverDev.Services.Session
{
    public interface ISessionServices
    {
        void StartSession();
        void ViewSession();
        void DeleteSession(int stackId);
    }
}
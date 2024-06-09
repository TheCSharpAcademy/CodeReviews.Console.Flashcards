namespace Flashcards.UndercoverDev.DataConfig
{
    public interface IDatabaseManager
    {
        void InitializeDatabase();
        void CreateFlashcardsTables();
        void CreateStacksTables();
        void CreateSessionsTables();
    }
}
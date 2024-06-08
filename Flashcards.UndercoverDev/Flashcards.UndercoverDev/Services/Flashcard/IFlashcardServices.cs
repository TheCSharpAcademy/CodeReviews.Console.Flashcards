namespace Flashcards.UndercoverDev.Services
{
    public interface IFlashcardServices
    {
        void AddFlashcard();
        void DeleteFlashcard();
        bool IfQuestionExists(string question);
        string ValidateQuestion();
        string ValidateAnswer();
        bool IsFlashcardForNewStack();
        void AddFlashcardToOldStack();
        void AddFlashcardToNewStack();
    }
}
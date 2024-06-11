namespace Flashcards.UndercoverDev.Services
{
    public interface IStackServices
    {
        void AddStack();
        void UpdateStack();
        void DeleteStack();
        public bool CheckIfStackExists(string stackName);
    }
}
namespace Flashcards.UndercoverDev.Services
{
    public interface IStackServices
    {
        void AddStack();
        void DeleteStack();
        public bool CheckIfStackExists(string stackName);
    }
}
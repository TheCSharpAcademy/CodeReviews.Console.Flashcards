using Flashcards.DAL.DTO;
using Flashcards.DAL.Model;
using System.Runtime.InteropServices;

namespace Flashcards.DAL
{
    public class Controller
    {
        private readonly Repository _repository;
        
        public Controller(Repository repository)
        {
            _repository = repository;
        }

        public bool CreateFlashcard(string front, string back, string stackName) => 
            _repository.Insert("Flashcard", GetFlashcardObject(front,back, stackName));

        public bool CreateStack(string name) =>
            _repository.Insert("Stack", GetStackObject(name));

        public bool UpdateFlashcard(string front, string back, string stackName) => 
            _repository.Update("Flashcard", GetFlashcardObject(front, back, stackName));

        public bool UpdateStack(string currentName, string updatedName) =>
            _repository.Update("Stack", GetStackObject(currentName, updatedName));

        public bool DeleteFlashcard(int id) =>
            _repository.Delete("Flashcard", id);

        public bool DeleteStack(string name) =>
            _repository.Delete("Stack", _repository.GetStackIDByName(name));

        public FlashcardStackDTO GetFlashCardByID(int id) =>
            _repository.GetFlashcardByID(id);

        public List<StackDTO> GetAllStacks() =>
            _repository.GetAllStacks();

        private Flashcard GetFlashcardObject(string front, string back, string stackName) =>
            new Flashcard
            {
                Front = front,
                Back = back,
                StackID = _repository.GetStackIDByName(stackName)
            };

        private Stack GetStackObject(string name) =>
            new Stack
            {
                Name = name
            };

        private Stack GetStackObject(string currentName, string updatedName) =>
            new Stack
            {
                ID = _repository.GetStackIDByName(currentName),
                Name = updatedName
            };
    }
}

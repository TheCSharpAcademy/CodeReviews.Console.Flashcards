using Dapper;
using Flashcards.Arashi256.Classes;
using Flashcards.Arashi256.Models;

namespace Flashcards.Arashi256.Controllers
{
    internal class FlashcardController
    {
        private FlashcardsDatabase _flashcardsDatabase;
        private StackController _stackController;

        public FlashcardController(StackController stackController)
        {
            _flashcardsDatabase = new FlashcardsDatabase();
            _stackController = stackController;
            if (_stackController == null) _stackController = new StackController();
        }

        public bool AddFlashcard(FlashcardDto dtoFlashcard)
        {
            if (_flashcardsDatabase.CheckDuplicateFlashcard(dtoFlashcard.StackId, dtoFlashcard.Front, dtoFlashcard.Back))
            {
                Flashcard newFlashcard = new Flashcard() { Id = dtoFlashcard.Id, StackId = dtoFlashcard.StackId, Front = dtoFlashcard.Front, Back = dtoFlashcard.Back };
                int rows = _flashcardsDatabase.AddNewFlashcard(newFlashcard);
                return rows > 0 ? true : false;
            }
            else
                return false;
        }

        public List<FlashcardDto> GetAllFlashcardsForStack(int stackid)
        {
            List<FlashcardDto> viewFlashcards = new List<FlashcardDto>();
            var parameters = new DynamicParameters();
            parameters.Add("StackId", stackid);
            List<Flashcard> flashcards = _flashcardsDatabase.GetFlashcardResults("SELECT * FROM dbo.flashcards WHERE StackId = @StackId", parameters);
            for (int i = 0; i < flashcards.Count; i++)
            {
                viewFlashcards.Add(new FlashcardDto() { DisplayId = i + 1, Id = flashcards[i].Id, StackId = flashcards[i].StackId, Front = flashcards[i].Front, Back = flashcards[i].Back, Subject = GetSubjectFromStackID(stackid) });
            }
            return viewFlashcards;
        }

        private string GetSubjectFromStackID(int id)
        {
            StackDto theStack = _stackController.GetStack(id);
            return theStack.Subject;
        }

        public bool UpdateFlashcard(FlashcardDto dtoFlashcard)
        {
            if (_flashcardsDatabase.CheckDuplicateFlashcard(dtoFlashcard.StackId, dtoFlashcard.Front, dtoFlashcard.Back))
            {
                Flashcard updatedFlashcard = new Flashcard() { Id = dtoFlashcard.Id, StackId = dtoFlashcard.StackId, Front = dtoFlashcard.Front, Back = dtoFlashcard.Back };
                int rows = _flashcardsDatabase.UpdateFlashcard(updatedFlashcard);
                return rows > 0 ? true : false;
            }
            else
                return false;
        }

        public bool DeleteFlashcard(FlashcardDto dtoFlashcard)
        {
            Flashcard newFlashcard = new Flashcard() { Id = dtoFlashcard.Id, StackId = dtoFlashcard.StackId, Front = dtoFlashcard.Front, Back = dtoFlashcard.Back };
            int rows = _flashcardsDatabase.DeleteFlashcard(newFlashcard);
            return rows > 0 ? true : false;
        }
    }
}

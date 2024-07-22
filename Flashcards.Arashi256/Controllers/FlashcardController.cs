using Dapper;
using Flashcards.Arashi256.Classes;
using Flashcards.Arashi256.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool AddFlashcard(Flashcard_DTO dtoFlashcard)
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

        public List<Flashcard_DTO> GetAllFlashcardsForStack(int stackid)
        {
            List<Flashcard_DTO> viewFlashcards = new List<Flashcard_DTO>();
            var parameters = new DynamicParameters();
            parameters.Add("StackId", stackid);
            List<Flashcard> flashcards = _flashcardsDatabase.GetFlashcardResults("SELECT * FROM dbo.flashcards WHERE StackId = @StackId", parameters);
            for (int i = 0; i < flashcards.Count; i++)
            {
                viewFlashcards.Add(new Flashcard_DTO() { DisplayId = i + 1, Id = flashcards[i].Id, StackId = flashcards[i].StackId, Front = flashcards[i].Front, Back = flashcards[i].Back, Subject = GetSubjectFromStackID(stackid) });
            }
            return viewFlashcards;
        }

        private string GetSubjectFromStackID(int id)
        {
            Stack_DTO theStack = _stackController.GetStack(id);
            return theStack.Subject;
        }

        public bool UpdateFlashcard(Flashcard_DTO dtoFlashcard)
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

        public bool DeleteFlashcard(Flashcard_DTO dtoFlashcard)
        {
            Flashcard newFlashcard = new Flashcard() { Id = dtoFlashcard.Id, StackId = dtoFlashcard.StackId, Front = dtoFlashcard.Front, Back = dtoFlashcard.Back };
            int rows = _flashcardsDatabase.DeleteFlashcard(newFlashcard);
            return rows > 0 ? true : false;
        }
    }
}

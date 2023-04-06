using ConsoleTableExt;
using FlashCardsLibrary;
using FlashCardsLibrary.Models;
using FlashCardsLibrary.Tools;
using sadklouds.FlashCards.Helpers;

namespace sadklouds.FlashCards.Controllers
{
    internal class ControllerFlashCards
    {
        private readonly SQLDataAccess _db;

        public ControllerFlashCards(SQLDataAccess db)
        {
            _db = db;
        }

        public void CreateFlashCard(string stackName)
        {
            string front = UserInputHelper.GetUserStringInput("Enter the front of the card: ");
            string back = UserInputHelper.GetUserStringInput("Enter the back of the card: ");
            _db.InsertFlashCard(stackName, front, back);
        }

        public void UpdateFlashCard(List<FlashCardModel> flashcards)
        {
            int id = UserInputHelper.UserIntInput("Please enter flashcard Id: ");
            id -= 1;
            if (id < flashcards.Count && id >= 0)
            {

                int flashcardsId = flashcards[id - 1].Id;
                string front = UserInputHelper.GetUserStringInput("Enter changes for front: ");
                string back = UserInputHelper.GetUserStringInput("Enter changes for back: ");
                _db.UpdateFlashCardRecord(flashcardsId, front, back);
            }
            else
            {
                Console.WriteLine($"Flashcard with id {id} was not found!");
            }

        }

        public void DeleteFlashCard(List<FlashCardModel> flashcards)
        {
            int id = UserInputHelper.UserIntInput("Please enter flashcard Id: ");
            id -= 1;
            if (id < flashcards.Count && id >= 0)
            {

                int flashcardsId = flashcards[id].Id;
                _db.DeleteFlashCardRecord(flashcardsId);
                Console.WriteLine("\nRecord succussfully deleted");
            }
            else
            {
                Console.WriteLine($"Flashcard with id {id} was not found!");
            }
        }

        public void GetFlashCards(List<FlashCardModel> flashCards)
        {
            var flashCardsDto = CreateDTOHelper.CreateFlashCardDTO(flashCards);
            if (flashCardsDto.Count > 0)
            {
                ConsoleTableBuilder
               .From(flashCardsDto)
               .ExportAndWriteLine();
            }
            else
            {
                Console.WriteLine("\nNo Records were found\n");
            }

        }
    }
}

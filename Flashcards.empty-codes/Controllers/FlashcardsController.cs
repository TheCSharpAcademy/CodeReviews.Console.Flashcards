using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Models;

namespace Flashcards.empty_codes.Controllers;

internal class FlashcardsController
{
    private readonly Database database;

    public FlashcardsController(Database db)
    {
        database = db;
    }

    public void InsertFlashcard(FlashcardDTO card)
    {

    }
    public List<FlashcardDTO> ViewAllFlashcards()
    {

    }

    public void UpdateFlashcard(FlashcardDTO card)
    {

    }

    public void DeleteFlashcard(FlashcardDTO card)
    {

    }
}

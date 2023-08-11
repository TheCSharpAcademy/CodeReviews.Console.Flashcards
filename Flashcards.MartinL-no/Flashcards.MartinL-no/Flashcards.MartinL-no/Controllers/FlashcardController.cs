using Flashcards.MartinL_no.Models;

internal class FlashcardController
{
    private readonly IFlashcardStackRepository _stackRepo;

    public FlashcardController(IFlashcardStackRepository stackRepo)
    {
        _stackRepo = stackRepo;
    }
}
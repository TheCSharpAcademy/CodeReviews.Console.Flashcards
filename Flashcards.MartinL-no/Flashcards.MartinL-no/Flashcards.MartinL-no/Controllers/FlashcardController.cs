using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.Controllers;

internal class FlashcardController
{
    private readonly IFlashcardStackRepository _stackRepo;

    public FlashcardController(IFlashcardStackRepository stackRepo)
    {
        _stackRepo = stackRepo;
    }

    public List<FlashcardStack> GetStacks()
    {
        return _stackRepo.GetStacks();
    }

    public FlashcardStack GetStackByName(string name)
    {
        return _stackRepo.GetStackByName(name);
    }
}
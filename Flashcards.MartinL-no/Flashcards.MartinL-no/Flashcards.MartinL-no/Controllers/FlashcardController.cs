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

    public bool CreateStack(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var stack = new FlashcardStack(name, new Stack<string>());
            return _stackRepo.InsertStack(stack);
        }

        return false;
    }

    public bool CreateFlashcard(string text, string stackName)
    {
        if (!string.IsNullOrWhiteSpace(text) || !string.IsNullOrWhiteSpace(stackName))
        {
            return _stackRepo.InsertFlashcard(text, stackName);
        }

        return false;
    }
}
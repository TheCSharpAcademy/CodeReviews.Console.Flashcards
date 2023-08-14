using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.Controllers;

internal class StackManagerController
{
    private readonly IFlashcardStackRepository _stackRepo;

    public StackManagerController(IFlashcardStackRepository stackRepo)
    {
        _stackRepo = stackRepo;
    }

    public List<FlashcardStackDto> GetStacks()
    {
        return _stackRepo.GetStacks()
            .Select(s => StackToDto(s))
            .ToList();
    }

    public List<string> GetStackNames()
    {
        var stacks = _stackRepo.GetStacks();
        return stacks.Select(s => s.Name).ToList();
    }

    public FlashcardStackDto GetStackByName(string name)
    {
        var stack = _stackRepo.GetStackByName(name);
        return StackToDto(stack);
    }

    public bool CreateStack(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var stack = new FlashcardStack() { Name = name };
            return _stackRepo.InsertStack(stack);
        }

        return false;
    }

    public bool CreateFlashcard(string front, string back, int stackId)
    {
        if (!string.IsNullOrWhiteSpace(front) && !string.IsNullOrWhiteSpace(back))
        {
            var flashcard = new Flashcard() { Front = front, Back = back, StackId = stackId };

            return _stackRepo.InsertFlashcard(flashcard);
        }

        return false;
    }

    public bool UpdateFlashcard(int id, string front, string back, int stackId)
    {
        if (id > 0 && !string.IsNullOrWhiteSpace(front) || !string.IsNullOrWhiteSpace(back) && stackId > 0)
        {
            var flashcard = new Flashcard() { Id = id, Front = front, Back = back, StackId = stackId };

            return _stackRepo.UpdateFlashcard(flashcard);
        }

        return false;
    }

    public bool DeleteFlashcard(int id)
    {
        return _stackRepo.DeleteFlashCard(id);
    }

    public bool DeleteStack(string name)
    {
        var stack = _stackRepo.GetStackByName(name);
        if (stack == null) return false;

        return _stackRepo.DeleteStack(stack.Id);
    }

    private FlashcardStackDto StackToDto(FlashcardStack stack)
    {
        return new FlashcardStackDto()
        {
            Id = stack.Id,
            Name = stack.Name,
            Flashcards = stack.Flashcards
                .Select((f, i) => FlashcardToDto(f, i+1))
                .ToList()
        };
    }

    private FlashcardDto FlashcardToDto(Flashcard flashcard, int viewId)
    {
        return new FlashcardDto()
        {
            Id = flashcard.Id,
            ViewId = viewId,
            Front = flashcard.Front,
            Back = flashcard.Back
        };
    }
}
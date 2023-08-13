using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.Controllers;

internal class FlashcardController
{
    private readonly IFlashcardStackRepository _stackRepo;

    public FlashcardController(IFlashcardStackRepository stackRepo)
    {
        _stackRepo = stackRepo;
    }

    public List<FlashcardStackDTO> GetStacks()
    {
        return _stackRepo.GetStacks()
            .Select(s => StackToDTO(s))
            .ToList();
    }

    public FlashcardStackDTO GetStackByName(string name)
    {
        var stack = _stackRepo.GetStackByName(name);

        return StackToDTO(stack);
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

    public bool CreateFlashcard(string original, string translation, int stackId)
    {
        if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(translation))
        {
            var flashcard = new Flashcard() { Original = original, Translation = translation, StackId = stackId };

            return _stackRepo.InsertFlashcard(flashcard);
        }

        return false;
    }

    public bool UpdateFlashcard(int id, string original, string translation, int stackId)
    {
        if (id > 0 && !string.IsNullOrWhiteSpace(original) || !string.IsNullOrWhiteSpace(translation) && stackId > 0)
        {
            var flashcard = new Flashcard() { Original = original, Translation = translation, StackId = stackId };

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

    private FlashcardStackDTO StackToDTO(FlashcardStack stack)
    {
        return new FlashcardStackDTO()
        {
            Id = stack.Id,
            Name = stack.Name,
            Flashcards = stack.Flashcards
                .Select(f => FlashcardToDto(f))
                .ToList()
        };
    }

    private FlashcardDTO FlashcardToDto(Flashcard flashcard)
    {
        return new FlashcardDTO()
        {
            Id = flashcard.Id,
            Original = flashcard.Original,
            Translation = flashcard.Translation
        };
    }
}
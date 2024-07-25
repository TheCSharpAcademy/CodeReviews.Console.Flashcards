using Flashcards.Repositories;

namespace Flashcards.Services;
public class FlashcardService {
    private readonly IFlashcardRepository _repository;

    public FlashcardService(IFlashcardRepository repository) {
        _repository = repository;
    }
}

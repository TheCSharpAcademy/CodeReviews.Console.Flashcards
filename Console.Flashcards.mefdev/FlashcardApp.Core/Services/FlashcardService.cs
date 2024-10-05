using FlashcardApp.Core.Models;
using FlashcardApp.Core.Repositories;
using FlashcardApp.Core.Repositories.Interfaces;
using FlashcardApp.Core.Services.Interfaces;

namespace FlashcardApp.Core.Services;

public class FlashcardService : IFlashcardService
{
    private readonly IFlashcardRepository _flashcardRepository;

    public FlashcardService(IFlashcardRepository flashcardRepository)
    {
        _flashcardRepository = flashcardRepository;
    }

    public async Task<Result<string>> AddFlashcard(Flashcard flashcard)
    {
        flashcard.Id = GenerateRandomID();
        var existingFlashcard = await _flashcardRepository.GetFlashcard(flashcard.Id);
        if (existingFlashcard != null)
        {
            return Result<string>.Failure("The flashcard is already exists.");
        }
        await _flashcardRepository.AddFlashcard(flashcard);
        return Result<string>.Success("success");
    }

    public async Task<Result<string>> DeleteFlashcard(int id)
    {
        var flashcard = await _flashcardRepository.GetFlashcard(id);
        if (flashcard == null)
        {
            return Result<string>.Failure("The flashcard is not found.");
        }
        await _flashcardRepository.DeleteFlashcard(flashcard.Id);
        return Result<string>.Success("success");
    }

    public async Task<Result<string>> DeleteFlashcardByQuestion(string question)
    {
        var flashcard = await _flashcardRepository.GetFlashcardByQuestion(question);
        if (flashcard == null)
        {
            return Result<string>.Failure("The flashcard is not found.");
        }
        await _flashcardRepository.DeleteFlashcardByQuestion(flashcard.Question);
        return Result<string>.Success("success");
    }

    public async Task<Result<IEnumerable<Flashcard>>> GetAllFlashcards()
    {
        var flashcards = await _flashcardRepository.GetAllFlashcards();
        if (flashcards == null || !flashcards.Any())
        {
            return Result<IEnumerable<Flashcard>>.Failure("Notice: No flashcard found.");
        }
        return Result<IEnumerable<Flashcard>>.Success(flashcards);
    }

    public async Task<Result<IEnumerable<Flashcard>>> GetFlashcardsByStackname(string name)
    {
        var flashcards = await _flashcardRepository.GetFlashcardsByStackname(name);
        if (flashcards == null || !flashcards.Any())
        {
            return Result<IEnumerable<Flashcard>>.Failure("Notice: No flashcard found.");
        }
        return Result<IEnumerable<Flashcard>>.Success(flashcards);
    }

    public Task<Result<Flashcard>> GetFlashcard(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> UpdateFlashcard(Flashcard flashcard)
    {
        throw new NotImplementedException();
    }
    public int GenerateRandomID()
    {
        return Math.Abs(Guid.NewGuid().GetHashCode());
    }

    public async Task<Result<Flashcard>> GetFlashcardByQuestion(string question)
    {
        var flashcard = await _flashcardRepository.GetFlashcardByQuestion(question);
        if (flashcard == null)
        {
            return Result<Flashcard>.Failure("The flashcard is not found.");
        }
        return Result<Flashcard>.Success(flashcard);
    }
}
using Flashcards.Dejmenek.DataAccess.Repositories;
using Flashcards.Dejmenek.Helpers;
using Flashcards.Dejmenek.Models;
using Flashcards.Dejmenek.Services;

namespace Flashcards.Dejmenek.Controllers;

public class FlashcardsController
{
    private readonly FlashcardsRepository _flashcardsRepository;
    private readonly UserInteractionService _userInteractionService;

    public FlashcardsController(FlashcardsRepository flashcardsRepository, UserInteractionService userInteractionService)
    {
        _flashcardsRepository = flashcardsRepository;
        _userInteractionService = userInteractionService;
    }

    public void AddFlashcard()
    {
        int stackId = _userInteractionService.GetId();
        string front = _userInteractionService.GetFlashcardFront();
        string back = _userInteractionService.GetFlashcardBack();

        _flashcardsRepository.AddFlashcard(stackId, front, back);
    }

    public void DeleteFlashcard()
    {
        List<FlashcardDTO> flashcards = GetAllFlashcards();

        string chosenFlashcardFront = _userInteractionService.GetFlashcard(flashcards);
        int chosenFlashcardId = flashcards.Single(f => f.Front == chosenFlashcardFront).Id;

        _flashcardsRepository.DeleteFlashcard(chosenFlashcardId);
    }

    public List<FlashcardDTO> GetAllFlashcards()
    {
        List<FlashcardDTO> flashcardDtos = new List<FlashcardDTO>();
        var flashcards = _flashcardsRepository.GetAllFlashcards();

        foreach (var flashcard in flashcards)
        {
            flashcardDtos.Add(Mapper.ToFlashcardDTO(flashcard));
        }

        return flashcardDtos;
    }

    public void UpdateFlashcard()
    {
        List<FlashcardDTO> flashcards = GetAllFlashcards();

        string chosenFlashcardFront = _userInteractionService.GetFlashcard(flashcards);
        int chosenFlashcardId = flashcards.Single(f => f.Front == chosenFlashcardFront).Id;

        string front = _userInteractionService.GetFlashcardFront();
        string back = _userInteractionService.GetFlashcardBack();

        _flashcardsRepository.UpdateFlashcard(chosenFlashcardId, front, back);
    }
}

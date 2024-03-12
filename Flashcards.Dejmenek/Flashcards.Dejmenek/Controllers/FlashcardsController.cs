using Flashcards.Dejmenek.DataAccess.Repositories;
using Flashcards.Dejmenek.Helpers;
using Flashcards.Dejmenek.Models;
using Flashcards.Dejmenek.Services;
using Spectre.Console;

namespace Flashcards.Dejmenek.Controllers;

public class FlashcardsController
{
    private readonly FlashcardsRepository _flashcardsRepository;
    private readonly UserInteractionService _userInteractionService;
    private readonly StacksRepository _stacksRepository;

    public FlashcardsController(FlashcardsRepository flashcardsRepository, UserInteractionService userInteractionService, StacksRepository stacksRepository)
    {
        _flashcardsRepository = flashcardsRepository;
        _userInteractionService = userInteractionService;
        _stacksRepository = stacksRepository;
    }

    public void AddFlashcard()
    {
        var stacks = _stacksRepository.GetAllStacks();

        if (!_stacksRepository.HasStack())
        {
            AnsiConsole.MarkupLine("No stacks found. Add new stack before creating new flashcard!");
            return;
        }

        List<StackDTO> stackDtos = new List<StackDTO>();

        foreach (var stack in stacks)
        {
            stackDtos.Add(Mapper.ToStackDTO(stack));
        }

        string chosenStackName = _userInteractionService.GetStack(stackDtos);

        int chosenStackId = stacks.Single(s => s.Name == chosenStackName).Id;
        string front = _userInteractionService.GetFlashcardFront();
        string back = _userInteractionService.GetFlashcardBack();

        _flashcardsRepository.AddFlashcard(chosenStackId, front, back);
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

using Ohshie.FlashCards.DataAccess;
using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.CardsManager;

public class FlashcardService
{
    private readonly FlashcardsRepository _flashcardsRepository = new();
    
    public void CreateNewFlashcard(FlashCard newCard)
    {
        if (string.IsNullOrEmpty(newCard.Name) || newCard.DeckId < 1) return;
        
        _flashcardsRepository.CreateNewFlashcard(newCard);
    }

    public List<FlashCardDto> FlashCardDtoList(DeckDto deck)
    {
        var AttachedFlashcards = _flashcardsRepository.FetchAttachedFlashcards(deck.Id);

        int counter = 0;
        List<FlashCardDto> flashCardDtos = new();
        foreach (var flashCard in AttachedFlashcards)
        {
            var flashCardDto = Mapper.FlashcardToDtoMapper(flashCard, ++counter);
            flashCardDtos.Add(flashCardDto);
        }

        return flashCardDtos;
    }

    public void DeleteFlashcard(FlashCardDto flashCardDto)
    {
        var flashcard = Mapper.FlashcardDtoToFlashcardMapper(flashCardDto);
        
        _flashcardsRepository.DeleteFlashcard(flashcard);
    }
}
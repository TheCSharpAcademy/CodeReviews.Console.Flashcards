using Ohshie.FlashCards.DataAccess;
using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.CardsManager;

public class FlashcardService
{
    private readonly DbOperations _dbOperations = new();
    
    public void CreateNewFlashcard(FlashCard newCard)
    {
        if (string.IsNullOrEmpty(newCard.Name) || newCard.DeckId < 1) return;
        
        _dbOperations.CreateNewFlashcard(newCard);
    }

    public List<FlashCardDto> OutputFlashcardsToDisplay(DeckDto deck)
    {
        var AttachedFlashcards = _dbOperations.FetchAttachedFlashcards(deck.Id);

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
        
        _dbOperations.DeleteFlashcard(flashcard);
    }
}
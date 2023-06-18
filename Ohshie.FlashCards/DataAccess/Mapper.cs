using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.DataAccess;

public class Mapper
{
   private DbOperations _dbOperations = new();
   
   public DeckDto DeckToDtoMapper(Deck deck, int counter)
   {
      return new DeckDto
      {
         Id = deck.Id,
         ViewId = counter,
         DeckName = deck.Name,
         DeckDescription = deck.Description,
         AmountOfFlashcards = deck.FlashCards.Count
      };
   }

   public Deck DeckDtoToDeckMapper(DeckDto deckDto)
   {
      return _dbOperations.FetchDeckById(deckDto.Id);
   }
}
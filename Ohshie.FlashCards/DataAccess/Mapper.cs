using Ohshie.FlashCards.CardsManager;
using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.DataAccess;

public static class Mapper
{
   private static readonly FlashcardsRepository FlashcardsRepository = new();
   private static readonly DecksRepository DecksRepository = new();
   
   public static DeckDto DeckToDtoMapper(Deck deck, int counter)
   {
      return new DeckDto
      {
         Id = deck.Id,
         ViewId = counter,
         DeckName = deck.Name,
         DeckDescription = deck.Description,
         AmountOfFlashcards = deck.FlashCards!.Count
      };
   }

   public static Deck DeckDtoToDeckMapper(DeckDto deckDto)
   {
      return DecksRepository.FetchDeckById(deckDto.Id)!;
   }

   public static FlashCardDto FlashcardToDtoMapper(FlashCard flashCard, int counter)
   {
      return new FlashCardDto()
      {
         DtoId = counter,
         FlashCardId = flashCard.Id,
         Name = flashCard.Name,
         Content = flashCard.Content
      };
   }
   
   public static FlashCard FlashcardDtoToFlashcardMapper(FlashCardDto flashCardDto)
   {
      return FlashcardsRepository.FetchFlashcardById(flashCardDto.FlashCardId);
   }
}
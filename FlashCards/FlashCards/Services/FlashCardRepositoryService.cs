using Microsoft.IdentityModel.Tokens;

namespace FlashCards
{
    internal class FlashCardRepositoryService
    {
        public FlashCardRepository FlashCardRepository { get; set; }
        public CardStackRepository CardStackRepository { get; set; }
        public UserInterface UserInterface { get; set; }



        private List<FlashCardDto> _flashCardDtos = new List<FlashCardDto>();
        private Dictionary<int, int> _cardIdMap = new Dictionary<int, int>();
        private List<FlashCardDto> _mappedFlashCardDtos = new List<FlashCardDto>();

        public FlashCardRepositoryService(FlashCardRepository cardRepository, CardStackRepository stackRepository, UserInterface UI)
        {
            FlashCardRepository = cardRepository;
            CardStackRepository = stackRepository;
            UserInterface = UI;
        }

        public void PrepareRepository(List<CardStack> stacks, List<FlashCard> flashCards)
        {
            
            if (!FlashCardRepository.DoesTableExist())
            {
                FlashCardRepository.CreateTable();
                FlashCardRepository.AutoFill(stacks, flashCards);
            }
        }
        private void GetAndMapFlashcards(CardStack stack)
        {
            _flashCardDtos = FlashCardRepository.GetAllCardsInStack(stack).ToList();

            _cardIdMap = _flashCardDtos.Select((card, index) => new { card.CardID, newId = index + 1 }).ToDictionary(x => x.CardID, x => x.newId);

            _mappedFlashCardDtos = _flashCardDtos.Select(card => new FlashCardDto
            {
                CardID = _cardIdMap[card.CardID],
                FrontText = card.FrontText,
                BackText = card.BackText,
            }
            ).ToList();
        }
        public void HandleViewAllCards(CardStack stack)
        {
            if (_flashCardDtos.IsNullOrEmpty())
            {
                GetAndMapFlashcards(stack);
            }

            UserInterface.PrintCards(_mappedFlashCardDtos);
            UserInterface.PrintPressAnyKeyToContinue();
        }
        public void HandleViewXCards(CardStack stack)
        {
            if (_flashCardDtos.IsNullOrEmpty())
            {
                GetAndMapFlashcards(stack);
            }

            int count = UserInterface.GetCount();
            UserInterface.PrintCards(_mappedFlashCardDtos.Take(count).ToList());
            UserInterface.PrintPressAnyKeyToContinue();
        }
        public void HandleCreateNewFlashCard(CardStack stack)
        {
            FlashCard card = UserInterface.GetNewCard();
            card.StackID = stack.StackID;

            bool wasActionSuccessful = FlashCardRepository.Insert(card);
            Console.WriteLine(wasActionSuccessful ? "Card added successfully" : "Error occured, please contact admin");
            GetAndMapFlashcards(stack);
            UserInterface.PrintPressAnyKeyToContinue();

        }
        public void HandleUpdateFlashCard(CardStack stack)
        {
            //get flash card
            int cardId = UserInterface.GetCardID(_mappedFlashCardDtos);
            //get new values
            FlashCard card = UserInterface.GetNewCard();
            card.CardID = _cardIdMap.FirstOrDefault(x => x.Value == cardId).Key;
            card.StackID = stack.StackID;

            //insert to DB
            bool wasActionSuccessful = FlashCardRepository.Update(card);
            Console.WriteLine(wasActionSuccessful ? "Card updated successfully" : "Error occured, please contact admin");
            GetAndMapFlashcards(stack);
            UserInterface.PrintPressAnyKeyToContinue();

        }
        public void HandleDeleteFlashCard(CardStack stack)
        {
            int cardId = UserInterface.GetCardID(_mappedFlashCardDtos);
            
            cardId = _cardIdMap.FirstOrDefault(x => x.Value == cardId).Key;

            bool wasActionSuccessful = FlashCardRepository.Delete(new FlashCard() { CardID = cardId });
            Console.WriteLine(wasActionSuccessful ? "Card deleted successfully" : "Error occured, please contact admin");
            GetAndMapFlashcards(stack);
            UserInterface.PrintPressAnyKeyToContinue();
        }
        public CardStack HandleSwitchStack(List<CardStack> stacks)
        {
            CardStack newStack = UserInterface.StackSelection(stacks);
            GetAndMapFlashcards(newStack);

            return newStack;
        }
        public List<FlashCardDto> GetAllCardsInStack(CardStack stack) => FlashCardRepository.GetAllCardsInStack(stack).ToList();
    }
}

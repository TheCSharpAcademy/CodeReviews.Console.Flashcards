namespace FlashCards
{
    /// <summary>
    /// Represents a service for managing FlashCard entities.
    /// Implements ICardStackService
    /// </summary>
    internal class FlashCardService : IFlashCardService
    {
        /// <inheritdoc/>
        public IFlashCardRepository FlashCardRepository { get; set; }
        /// <inheritdoc/>
        public IFlashCardServiceUi UserInterface { get; set; }

        /// <summary>
        /// Represent list of all FlashCardDto entities
        /// </summary>
        private List<FlashCardDto>? _flashCardDtos = null;
        /// <summary>
        /// Represent map for _flashCardDtos where all FlashCardDto entities are mapped from 1 
        /// </summary>
        private Dictionary<int, int> _cardIdMap = new Dictionary<int, int>();
        /// <summary>
        /// Represent list of mapped FlashCardDto entities
        /// </summary>
        private List<FlashCardDto> _mappedFlashCardDtos = new List<FlashCardDto>();

        /// <summary>
        /// Intializes new object of FlashCardService class
        /// </summary>
        /// <param name="repository">A implementation of IFlashCardRepository for database access</param>
        /// <param name="UI">A implementation of IFlashCardServiceUi for user interaction</param>
        public FlashCardService(IFlashCardRepository repository, IFlashCardServiceUi UI)
        {
            FlashCardRepository = repository;
            UserInterface = UI;
        }
        /// <inheritdoc/>
        public List<FlashCardDto>? GetAllCardsInStack(CardStack stack)
        {
            return FlashCardRepository.GetAllRecordsFromStack(stack)?.ToList();
        }
        /// <inheritdoc/>
        public bool PrepareRepository(List<CardStack> stacks, List<FlashCard> flashCards)
        {
            try
            {
                if (!FlashCardRepository.DoesTableExist())
                {
                    FlashCardRepository.CreateTable();
                    FlashCardRepository.AutoFill(stacks, flashCards);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing the repository\n");
                Console.WriteLine(ex.Message + "\n");
                return false;
            }
        }
        /// <summary>
        /// Retrieves all FlashCards from CardStack and map them so Card ID numbers start at 1
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        private bool GetAndMapFlashcards(CardStack stack)
        {

            var result = FlashCardRepository.GetAllRecordsFromStack(stack);

            if (result == null) { return false; }

            _flashCardDtos = result.ToList();

            _cardIdMap = _flashCardDtos.Select((card, index) => new { card.CardID, newId = index + 1 }).ToDictionary(x => x.CardID, x => x.newId);

            _mappedFlashCardDtos = _flashCardDtos.Select(card => new FlashCardDto
            {
                CardID = _cardIdMap[card.CardID],
                FrontText = card.FrontText,
                BackText = card.BackText,
            }
            ).ToList();

            return true;

        }
        /// <inheritdoc/>
        public void HandleViewAllCards(CardStack stack)
        {
            if (GetAndMapFlashcards(stack))
            {
                UserInterface.PrintCards(_mappedFlashCardDtos);
            }
            else
            {
                Console.WriteLine("Error while retrieving the cards during Handle View All Cards");
            }
            UserInterface.PrintPressAnyKeyToContinue();
        }
        /// <inheritdoc/>
        public void HandleViewXCards(CardStack stack)
        {
            if (GetAndMapFlashcards(stack))
            {
                int count = UserInterface.GetNumberFromUser("Enter number representing number of cards you'd like to show: ");
                UserInterface.PrintCards(_mappedFlashCardDtos.Take(count).ToList());
            }
            else
            {
                Console.WriteLine("Error while retrieving the cards during Handle View X Cards");
            }

            UserInterface.PrintPressAnyKeyToContinue();
        }
        /// <inheritdoc/>
        public void HandleCreateNewFlashCard(CardStack stack)
        {
            FlashCard card = UserInterface.GetNewCard();
            card.StackID = stack.StackID;

            bool wasActionSuccessful = FlashCardRepository.Insert(card);
            Console.WriteLine(wasActionSuccessful ? "Card added successfully" : "Error occured, please contact admin");

            UserInterface.PrintPressAnyKeyToContinue();

        }
        /// <inheritdoc/>
        public void HandleUpdateFlashCard(CardStack stack)
        {
            if (GetAndMapFlashcards(stack))
            {
                int cardId = UserInterface.GetCardID(_mappedFlashCardDtos);
                FlashCard card = UserInterface.GetNewCard();
                card.CardID = _cardIdMap!.FirstOrDefault(x => x.Value == cardId).Key;
                card.StackID = stack.StackID;

                bool wasActionSuccessful = FlashCardRepository.Update(card);
                Console.WriteLine(wasActionSuccessful ? "Card updated successfully" : "Error occured, please contact admin");
            }
            else
            {
                Console.WriteLine("Error while retrieving the cards during Update Flash Card");
            }

            UserInterface.PrintPressAnyKeyToContinue();

        }
        /// <inheritdoc/>
        public void HandleDeleteFlashCard(CardStack stack)
        {
            if (GetAndMapFlashcards(stack))
            {
                int cardId = UserInterface.GetCardID(_mappedFlashCardDtos);
                cardId = _cardIdMap!.FirstOrDefault(x => x.Value == cardId).Key;
                bool wasActionSuccessful = FlashCardRepository.Delete(new FlashCard() { CardID = cardId });
                Console.WriteLine(wasActionSuccessful ? "Card deleted successfully" : "Error occured, please contact admin");
            }
            else
            {
                Console.WriteLine("Error while retrieving the cards during Delete Flash Card");
            }

            UserInterface.PrintPressAnyKeyToContinue();
        }
        /// <inheritdoc/>
        public CardStack HandleSwitchStack(List<CardStack> stacks)
        {
            CardStack newStack = UserInterface.StackSelection(stacks);

            return newStack;
        }


    }
}

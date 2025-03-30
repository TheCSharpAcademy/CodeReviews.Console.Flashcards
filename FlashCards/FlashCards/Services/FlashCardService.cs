using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

namespace FlashCards
{
    internal class FlashCardService : IFlashCardService
    {
        public IFlashCardRepository FlashCardRepository { get; set; }
        public IFlashCardServiceUi UserInterface { get; set; }

        private List<FlashCardDto>? _flashCardDtos = null;
        private Dictionary<int, int> _cardIdMap = new Dictionary<int, int>();
        private List<FlashCardDto> _mappedFlashCardDtos = new List<FlashCardDto>();

        public FlashCardService(IFlashCardRepository cardRepository, IFlashCardServiceUi UI)
        {
            FlashCardRepository = cardRepository;
            UserInterface = UI;
        }
        public List<FlashCardDto>? GetAllCardsInStack(CardStack stack)
        {
            return FlashCardRepository.GetAllRecordsFromStack(stack)?.ToList();
        }

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
        private bool GetAndMapFlashcards(CardStack stack)
        {
            //nothing needs to be done if there was no entry added

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
        public void HandleCreateNewFlashCard(CardStack stack)
        {
            FlashCard card = UserInterface.GetNewCard();
            card.StackID = stack.StackID;

            bool wasActionSuccessful = FlashCardRepository.Insert(card);
            Console.WriteLine(wasActionSuccessful ? "Card added successfully" : "Error occured, please contact admin");

            UserInterface.PrintPressAnyKeyToContinue();

        }
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
        public CardStack HandleSwitchStack(List<CardStack> stacks)
        {
            CardStack newStack = UserInterface.StackSelection(stacks);

            return newStack;
        }


    }
}

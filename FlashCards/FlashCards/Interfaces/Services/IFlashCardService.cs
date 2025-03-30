
namespace FlashCards
{
    internal interface IFlashCardService
    {
        IFlashCardRepository FlashCardRepository { get; set; }
        IFlashCardServiceUi UserInterface { get; set; }

        List<FlashCardDto>? GetAllCardsInStack(CardStack stack);
        void HandleCreateNewFlashCard(CardStack stack);
        void HandleDeleteFlashCard(CardStack stack);
        CardStack HandleSwitchStack(List<CardStack> stacks);
        void HandleUpdateFlashCard(CardStack stack);
        void HandleViewAllCards(CardStack stack);
        void HandleViewXCards(CardStack stack);
        bool PrepareRepository(List<CardStack> stacks, List<FlashCard> flashCards);
    }
}
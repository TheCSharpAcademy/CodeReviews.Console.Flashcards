
namespace FlashCards
{
    internal interface IFlashCardServiceUi : IUserInterface
    {
        int GetCardID(List<FlashCardDto> cards);
        FlashCard GetNewCard();
        void PrintCards(List<FlashCardDto> cards);
    }
}
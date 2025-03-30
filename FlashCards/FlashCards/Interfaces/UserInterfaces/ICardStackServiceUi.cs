
namespace FlashCards
{
    internal interface ICardStackServiceUi :IUserInterface
    {
        void PrintStacks(List<CardStack> stacks);
    }
}
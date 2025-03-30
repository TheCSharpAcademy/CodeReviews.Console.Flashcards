
namespace FlashCards
{
    internal interface ICardStackService
    {
        ICardStackRepository CardStackRepository { get; set; }
        ICardStackServiceUi UserInterface { get; set; }

        List<CardStack> GetAllStacks();
        void HandleCreateNewStack();
        void HandleDeleteStack();
        void HandleRenameStack();
        void HandleViewAllStacks();
        bool PrepareRepository(List<CardStack> defaultData);
    }
}
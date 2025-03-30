namespace FlashCards
{
    internal interface IFlashCardAppUi : IUserInterface
    {
        FlashCardMenuOption GetFlashCardMenuSelection();
        MainMenuOption GetMainMenuSelection();
        StackMenuOption GetStackMenuSelection();
    }
}
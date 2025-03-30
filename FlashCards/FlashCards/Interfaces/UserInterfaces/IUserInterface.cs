
namespace FlashCards
{
    internal interface IUserInterface
    {
        void ClearConsole();
        int GetNumberFromUser(string prompt);
        string GetStringFromUser(string prompt);
        void PrintApplicationHeader();
        void PrintPressAnyKeyToContinue();
        CardStack StackSelection(List<CardStack> stacks);
    }
}
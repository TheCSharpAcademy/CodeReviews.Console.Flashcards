using Flash.Helper.MainHelper;
using Spectre.Console;

namespace Flash.Launching;
internal class ManageFlashcards
{
    internal static void GetManageFlashcards()
    {
        Console.Clear();

        ShowBanner.GetShowBanner("All Flashcards in All Stacks", Color.Plum1);

        ShowAllCardsInAllStacks.GetShowAllCardsInAllStacks();

        ReturnComment.MainMenuReturnComments();
    }
}

using Flash.Helper.MainHelper;
using Flash.Helper.SubManageStacksHelper.ViewAllFlashcardHelper;

namespace Flash.Launching.SubManageStacks;
internal class ViewAllFlashcards
{
    internal static void GetViewAllFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"{currentWorkingStack}");

        ViewAllFlashcardsMethod.GetViewAllFlashcardsMethod(currentWorkingStack);

        ReturnComment.MainMenuReturnComments();
    }
}

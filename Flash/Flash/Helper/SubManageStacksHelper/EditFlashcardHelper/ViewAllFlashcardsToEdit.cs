using Flash.Helper.SubManageStacksHelper.ViewAllFlashcardHelper;

namespace Flash.Helper.SubManageStacksHelper.EditFlashcardHelper;
internal class ViewAllFlashcardsToEdit
{
    internal static void GetViewAllFlashcardsToEdit(string currentWorkingStack)
    {
        Console.WriteLine($"{currentWorkingStack}");

        ViewAllFlashcardsMethod.GetViewAllFlashcardsMethod(currentWorkingStack);

    }
}

using Flash.Helper.MainHelper;
using Flash.Helper.SubManageStacksHelper.ViewXAmountFlashcardHelper;

namespace Flash.Launching.SubManageStacks;
internal class ViewXAmountFlashcards
{
    internal static void GetViewXAmountFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"{currentWorkingStack}");

        Console.WriteLine($"What is your X amount?");

        string xAmountString = Console.ReadLine();

        int xAmount = SelectXAmount.GetSelectXAmount(xAmountString);

        ViewXAmountFlashcardsMethod.GetViewXAmountFlashcardsMethod(currentWorkingStack, xAmount);

        ReturnComment.MainMenuReturnComments();
    }
}

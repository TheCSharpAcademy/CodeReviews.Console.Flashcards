using Flash.Helper.MainHelper;
using Flash.Helper.SubManageStacksHelper.EditFlashcardHelper;
using Flash.Helper.SubManageStacksHelper.ViewAllFlashcardHelper;

namespace Flash.Launching.SubManageStacks;
internal class EditFlashcards
{
    public static void GetEditFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"Current Stack: {currentWorkingStack}\n");

        ViewAllFlashcardsToEdit.GetViewAllFlashcardsToEdit(currentWorkingStack);

        Console.WriteLine("What to edit?");

        int currentWorkingFlashcardId = SelectFlashcardToWorkWith.GetSelectFlashcardToWorkWith();

        ShowFlashcardsInCurrentStack.GetShowFlashcardsInCurrentStack(currentWorkingFlashcardId);

        EditFlashcardsInCurrentStack.GetEditFlashcardsInCurrentStack(currentWorkingFlashcardId);

        ReturnComment.MainMenuReturnComments();
    }


}

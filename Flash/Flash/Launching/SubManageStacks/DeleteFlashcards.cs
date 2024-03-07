using Flash.Helper.MainHelper;
using Flash.Helper.SubManageStacksHelper.DeleteFlashcardHelper;

namespace Flash.Launching.SubManageStacks;
internal class DeleteFlashcards
{
    internal static void GetDeleteFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"Current Stack: {currentWorkingStack}\n");

        ViewAllFlashcards.GetViewAllFlashcards(currentWorkingStack);

        Console.WriteLine("What to delete?");

        int currentWorkingFlashcardId = SelectFlashcardToWorkWith.GetSelectFlashcardToWorkWith();

        DeleteSelectedFlashcard.GetDeleteSelectedFlashcard(currentWorkingFlashcardId); 

        ReturnComment.MainMenuReturnComments();
    }

}


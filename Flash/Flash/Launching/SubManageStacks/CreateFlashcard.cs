using Flash.Helper.MainHelper;
using Flash.Helper.SubManageStacksHelper.CreateFlashcardHelper;
using System.Data.SqlClient;


namespace Flash.Launching.SubManageStacks;
internal class CreateFlashcard
{
    internal static void GetCreateFlashcard(string currentWorkingStack)
    {
        int flashcardsTableCount = CheckFlashcardsTableExists.GetCheckFlashcardsTableExists();

        if (flashcardsTableCount == 0 ) 
        {
            CreateFlashcardsTable.GetCreateFlashcardsTable();
        }
        else
        {
            Console.WriteLine("Flashcards Table alreaday exists");
        }

        CreateAFlashcard.GetCreateAFlashcard(currentWorkingStack);

        ReturnComment.MainMenuReturnComments();
    }
}

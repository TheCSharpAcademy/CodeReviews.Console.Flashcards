namespace Flash.Helper.MainHelper;
internal class SelectFlashcardToWorkWith
{
    internal static int GetSelectFlashcardToWorkWith()
    {
        string currentWorkingFlashcardString = Console.ReadLine();
        int currentWorkingFlashcardId;

        if (int.TryParse(currentWorkingFlashcardString, out currentWorkingFlashcardId))
        {

            Console.WriteLine($"Selected Flashcard_Primary_Id: {currentWorkingFlashcardId}");
        }
        else
        {
            Console.WriteLine("Unable to convert the string to an integer.");
        }

        return currentWorkingFlashcardId;
    }
}

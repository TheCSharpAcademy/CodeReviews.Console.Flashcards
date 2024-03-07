namespace Flash.Helper.Renumber;
internal class RenumberFlashcards
{
    internal static void GetRenumberFlashcards(List<FlashcardDto> flashcards)
    {
        for (int i = 0; i < flashcards.Count; i++)
        {
            flashcards[i].Flashcard_Primary_Id = i + 1;
        }
    }
}

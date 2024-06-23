namespace FlashcardsProgram.Flashcards;

public class FlashcardMapping
{
    public static FlashcardDTO ToDTO(FlashcardDAO dao)
    {
        return new FlashcardDTO(dao.Id, dao.Front, dao.Back);
    }
}
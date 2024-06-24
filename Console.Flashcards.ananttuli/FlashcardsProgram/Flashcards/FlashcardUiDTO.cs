namespace FlashcardsProgram.Flashcards;

public class FlashcardUiDTO(FlashcardDAO card, string order)
{
    public FlashcardDAO Card = card;
    public string Order = order;

    public override string ToString()
    {
        return $"#{Order}\t{Card.Front}\t{Card.Back}";
    }
}
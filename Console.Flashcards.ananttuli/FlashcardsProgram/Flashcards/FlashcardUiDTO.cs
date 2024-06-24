namespace FlashcardsProgram.Flashcards;

public class FlashcardUiDto(FlashcardDao card, string order)
{
    public FlashcardDao Card = card;
    public string Order = order;

    public override string ToString()
    {
        return $"#{Order}\t{Card.Front}\t{Card.Back}";
    }
}